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
    public class TasksAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TasksAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/TasksAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskEntity>> GetTaskEntity(int id)
        {
            var taskEntity = await _context.Tasks.FindAsync(id);

            if (taskEntity == null)
            {
                return NotFound();
            }

            return taskEntity;
        }

        // PUT: api/TasksAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskEntity(int id, TaskEntity taskEntity)
        {
            if (id != taskEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskEntityExists(id))
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

        // POST: api/TasksAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskEntity>> PostTaskEntity(TaskEntity taskEntity)
        {
            _context.Tasks.Add(taskEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskEntity", new { id = taskEntity.Id }, taskEntity);
        }

        // DELETE: api/TasksAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskEntity(int id)
        {
            var taskEntity = await _context.Tasks.FindAsync(id);
            if (taskEntity == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(taskEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskEntityExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
