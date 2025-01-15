using Microsoft.AspNetCore.Mvc;
using Data;
using Data.Entities;

namespace Task_Manager.Controllers
{
    public class SubtaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubtaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Details(int taskId)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return NotFound();

            var subtasks = _context.Subtasks.Where(s => s.TaskId == taskId).ToList();
            ViewBag.TaskId = taskId;
            ViewBag.ProjectId = task.ProjectId; 
            return View(subtasks);
        }


        [HttpGet]
        public IActionResult Create(int taskId)
        {
            var subtask = new SubtaskEntity
            {
                TaskId = taskId
            };

            return View(subtask);
        }

        [HttpPost]
        public IActionResult Create(int taskId, string name, string description)
        {

            var subtask = new SubtaskEntity
            {
                Name = name,
                Description = description,
                Status = "To Do",
                TaskId = taskId
            };

            _context.Subtasks.Add(subtask);
            _context.SaveChanges();

            return RedirectToAction("Details", "Subtask", new { taskId = taskId });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subtask = _context.Subtasks.FirstOrDefault(s => s.Id == id);
            if (subtask == null)
                return NotFound();

            return View(subtask);
        }

        [HttpPost]
        public IActionResult Edit(int id, string name, string description, string status)
        {
            var subtask = _context.Subtasks.FirstOrDefault(s => s.Id == id);
            if (subtask != null)
            {
                subtask.Name = name;
                subtask.Description = description;
                subtask.Status = status;
                _context.SaveChanges();
            }
            return RedirectToAction("Details", "Subtask", new { taskId = subtask.TaskId });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var subtask = _context.Subtasks.FirstOrDefault(s => s.Id == id);
            if (subtask != null)
            {
                _context.Subtasks.Remove(subtask);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", "Subtask", new { taskId = subtask?.TaskId });
        }

    }
}
