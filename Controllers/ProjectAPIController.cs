using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Entities;

namespace Task_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectEntity>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/ProjectAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectEntity>> GetProjectEntity(int id)
        {
            var projectEntity = await _context.Projects.FindAsync(id);

            if (projectEntity == null)
            {
                return NotFound();
            }

            return projectEntity;
        }

        // PUT: api/ProjectAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectEntity(int id, ProjectEntity projectEntity)
        {
            if (id != projectEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectEntityExists(id))
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

        // POST: api/ProjectAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectEntity>> PostProjectEntity(ProjectEntity projectEntity)
        {
            _context.Projects.Add(projectEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectEntity", new { id = projectEntity.Id }, projectEntity);
        }

        // DELETE: api/ProjectAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectEntity(int id)
        {
            var projectEntity = await _context.Projects.FindAsync(id);
            if (projectEntity == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(projectEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectEntityExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
