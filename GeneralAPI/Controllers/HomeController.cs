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
using System.Text.Encodings.Web;
using GeneralAPI.Interfaces;

namespace GeneralAPI.Controllers
{
	[Route("api/home")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		private readonly RenderPlatformXContext _context;
		private readonly IConfiguration _configuration;
		private readonly HtmlEncoder _htmlEncoder;
		private readonly IRateLimitingService _rateLimiter;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<HomeController> _logger;
		private readonly ISecurityLoggingService _securityLoggingService;
		private readonly string _frontendUrl;

		public HomeController(RenderPlatformXContext context, 
			IConfiguration configuration, 
			HtmlEncoder htmlEncoder, 
			IHttpContextAccessor httpContextAccessor, 
			IRateLimitingService rateLimiter,
			ISecurityLoggingService securityLoggingService)
		{
			_context = context;
			_configuration = configuration;
			_htmlEncoder = htmlEncoder;
			_httpContextAccessor = httpContextAccessor;
			_rateLimiter = rateLimiter;
			_frontendUrl = configuration["FrontendSettings:BaseUrl"] ?? throw new InvalidOperationException("Frontend Base URL not found");
			_securityLoggingService = securityLoggingService;
		}

		// POST:
		[HttpPost("contact")]
		public async Task<ActionResult<bool>> Contact([FromForm] ContactDTO dto)
		{
			// Get IP Address
			string? ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
			string emailLowercase = dto?.Email.ToLowerInvariant() ?? "";
			
			// Rate limit check
			if (ipAddress != null && _rateLimiter.IsRateLimitExceeded(ipAddress))
			{
				_securityLoggingService.LogWarning(emailLowercase, "Status code 429", 1);
				return StatusCode(429, "You have submitted too recently. Please wait a minute.");
			}

			// Check form input against model
			if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Message))
			{
				_securityLoggingService.LogFailure(emailLowercase, "Bad request");
				return BadRequest(ModelState);
			}

			//Check honeypot field (bot detection)
			if (Request.Form.ContainsKey("hp-field") && !string.IsNullOrWhiteSpace(Request.Form["hp-field"]))
			{
				//Console.WriteLine("Suspicious activity detected (Honeypot field filled). Blocking submission");
				_securityLoggingService.LogWarning(emailLowercase, "Blocking submission", 2);
				// Bot detected
				return Ok("ThankYou");
			}

			// Sanitize form field data before sending via email
			string safeName = _htmlEncoder.Encode(dto.Name);
			string safeEmail = _htmlEncoder.Encode(dto.Email);
			string safeMessage = _htmlEncoder.Encode(dto.Message);
			string timeStamp = DateTime.Now.ToString("yyyy-MMM-dd HH:mm");
			string subjectLine = $"New Contact Form Message from {safeName} ({safeEmail}) - [{timeStamp}]";

			// Send email with contact form data
			try
			{
				SmtpClient client = new SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]))
				{
					Credentials = new NetworkCredential(_configuration["Smtp:User"], _configuration["Smtp:Pass"]),
					EnableSsl = true // Use SSL/TLS
				};

				MailMessage mailMessage = new MailMessage
				{
					From = new MailAddress(_configuration["Mail:FromAddress"] ?? ""),
					Subject = subjectLine,
					Body = $"Name: {safeName}\nEmail: {safeEmail}\nMessage:\n{safeMessage}",
					IsBodyHtml = false // Treat body as plain text not html
				};
				mailMessage.To.Add(_configuration["Mail:ToAddress"] ?? "");
				await client.SendMailAsync(mailMessage);
				_securityLoggingService.LogSuccess(emailLowercase);
				return Ok("Email sent");
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
