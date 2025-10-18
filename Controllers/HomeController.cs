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

namespace GeneralAPI.Controllers
{
	[Route("api/home")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		private readonly PlatformXContext _context;

		public HomeController(PlatformXContext context)
		{
			_context = context;
		}

		// GET: 
		[HttpGet("framework")]
		public async Task<ActionResult<List<FrameworkResponseDTO>>> GetFramework()
		{
			List<Framework> list = await _context.Framework.Where(i => i.IsDisplay == true).ToListAsync();
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
									IconType = i.IconType
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
																								.OrderByDescending(i => i.ID)
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
																														.OrderByDescending(i => i.ID)
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

		// GET: api/Home/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Framework>> GetFramework(int id)
		{
			var framework = await _context.Framework.FindAsync(id);

			if (framework == null)
			{
				return NotFound();
			}

			return framework;
		}

		// GET: 
		[HttpGet("projects")]
		public async Task<ActionResult<List<ProjectDTO>>> GetProjects()
		{
			List<Project> list = await _context.Projects.Where(i => i.IsDisplay == true).ToListAsync();
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

		// PUT: api/Home/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutFramework(int id, Framework framework)
		{
			if (id != framework.ID)
			{
				return BadRequest();
			}

			_context.Entry(framework).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FrameworkExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Home
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Framework>> PostFramework(Framework framework)
		{
			_context.Framework.Add(framework);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetFramework", new { id = framework.ID }, framework);
		}

		// DELETE: api/Home/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFramework(int id)
		{
			var framework = await _context.Framework.FindAsync(id);
			if (framework == null)
			{
				return NotFound();
			}

			_context.Framework.Remove(framework);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool FrameworkExists(int id)
		{
			return _context.Framework.Any(e => e.ID == id);
		}
	}
}
