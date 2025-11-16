using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeneralAPI.Data;
using GeneralAPI.Models;
using GeneralAPI.TransferObjects;
using GeneralAPI.Models.PostgresSql;
using System.Net.Mail;
using System.Net;

namespace GeneralAPI.Controllers
{
	[Route("api/home")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		private readonly RenderPlatformXContext _context;
		private readonly IConfiguration _configuration;

		public HomeController(RenderPlatformXContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		// POST:
		[HttpPost("contact")]
		public async Task<ActionResult<bool>> Contact([FromForm] ContactDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Message))
			{
				return BadRequest(ModelState);
			}

			//Check honeypot field (bot detection)
			if (Request.Form.ContainsKey("hp-field") && !string.IsNullOrWhiteSpace(Request.Form["hp-field"]))
			{
				Console.WriteLine("Suspicious activity detected (Honeypot field filled). Blocking submission");
				// Bot detected
				return Redirect("https://localhost:7223/Home/ThankYou");
			}
			// Send email
			try
			{
				var client = new SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]))
				{
					Credentials = new NetworkCredential(_configuration["Smtp:User"], _configuration["Smtp:Pass"]),
					EnableSsl = true // Use SSL/TLS
				};

				var mailMessage = new MailMessage
				{
					From = new MailAddress(_configuration["Mail:FromAddress"]),
					Subject = $"New Portfolio message from {dto.Name}",
					Body = $"Name: {dto.Name}\nEmail: {dto.Email}\nMessage:\n{dto.Message}",
					IsBodyHtml = false
				};
				mailMessage.To.Add(_configuration["Mail:ToAddress"]);
				await client.SendMailAsync(mailMessage);

				return Redirect("https://localhost:7223/Home/ThankYou");
			}
			catch (Exception exception)
			{
				Console.WriteLine($"Email send error: {exception.Message}");
				return StatusCode(500, "Error sending message. Please try again.");
			}
		}
		// GET: 
		[HttpGet("framework")]
		public async Task<ActionResult<List<FrameworkResponseDTO>>> GetFramework()
		{
			List<Framework2> list = await _context.Framework.Where(i => i.IsDisplay == true).ToListAsync();
			if (list.Count > 0)
			{
				return list
								.OrderBy(i => i.Order)
								.Select(i => new FrameworkResponseDTO()
								{
									ID = i.ID,
									Name = i.Name,
									Order = i.Order,
									IconClassPath = i.IconClassPath,
									IconType = i.IconType,
									BackgroundColour = i.BackgroundColour
								})
								.ToList();
			}

			return NotFound(new List<FrameworkResponseDTO>());
		}

		// GET: 
		[HttpGet("about")]
		public async Task<ActionResult<AboutResponseDTO>> GetAboutData()
		{
			List<WorkExperienceResponseDTO> workList = await _context.WorkExperience
																								.Where(i => i.IsDisplay == true)
																								.OrderByDescending(i => i.DatesOfEmployment)
																								.Select(i => new WorkExperienceResponseDTO()
																								{
																									ID = i.ID,
																									Position = i.Position,
																									Company = i.Company,
																									DatesOfEmployment = i.DatesOfEmployment,
																									Description = i.Description
																								})
																								.ToListAsync();

			if (workList.Count == 0)
			{
				workList = new List<WorkExperienceResponseDTO>();
			}

			List<EducationResponseDTO> educationList = await _context.Education
																														.Where(i => i.IsDisplay == true)
																														.OrderByDescending(i => i.YearComplete)
																														.Select(i => new EducationResponseDTO()
																														{
																															Institution = i.Institution,
																															DegreeCourse = i.DegreeCourse,
																															YearComplete = i.YearComplete,
																															Link = i.Link,
																															LinkText = i.LinkText
																														})
																														.ToListAsync();

			if (educationList.Count == 0)
			{
				educationList = new List<EducationResponseDTO>();
			}
			string aboutText = "";
			string? aboutTextRaw = await _context.ContentSource.Where(i => i.IsDisplay == true && i.ContentName == "About").Select(i => i.ContentBody).FirstOrDefaultAsync();

			if (aboutTextRaw != null)
			{
				aboutText = aboutTextRaw;
			}
			AboutResponseDTO dto = new AboutResponseDTO()
			{
				AboutText = aboutText ?? "",
				Education = educationList,
				Work = workList
			};

			// success
			return dto;
		}

		// GET: 
		[HttpGet("projects")]
		public async Task<ActionResult<List<ProjectDTO>>> GetProjects()
		{
			List<Project2> list = await _context.Project.Where(i => i.IsDisplay == true).ToListAsync();
			if (list.Count > 0)
			{
				return list
								.OrderBy(i => i.Order)
								.Select(i => new ProjectDTO()
								{
									ID = i.ID,
									Name = i.Name,
									GithubLink = i.GithubLink,
									LiveLink = i.LiveLink,
									Description = i.Description,
									Technologies = i.Technologies,
									ImagePath = i.ImagePath
								})
								.ToList();
			}

			return NotFound(new List<ProjectDTO>());
		}
	}
}
