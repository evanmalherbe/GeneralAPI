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

namespace GeneralAPI.Controllers
{
	[Route("api/home")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		private readonly RenderPlatformXContext _context;
		public HomeController(RenderPlatformXContext context)
		{
			_context = context;
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
				return RedirectToAction("ThankYou");
			}

			return RedirectToAction("ThankYou");
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
