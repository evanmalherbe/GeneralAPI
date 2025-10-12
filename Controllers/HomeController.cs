using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeneralAPI.Data;
using GeneralAPI.Models;

namespace GeneralAPI.Controllers
{
    [Route("api/framework")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly PlatformAlphaContext _context;

        public HomeController(PlatformAlphaContext context)
        {
            _context = context;
        }

        // GET: 
        [HttpGet]
        public async Task<ActionResult<List<FrameworkResponseDTO>>> GetFramework()
        {
          List<Framework> list = await _context.Framework.Where(i => i.IsDisplay == true).ToListAsync();
          if (list.Count > 0)
          {
            return list.Select(i => new FrameworkResponseDTO() { ID = i.ID, Name = i.Name}).ToList();
          }

          return NotFound(new List<FrameworkResponseDTO>());
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
