using Microsoft.AspNetCore.Mvc;
using Data; 
using Data.Entities; 


namespace Task_Manager.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int projectId)
        {
            var tasks = _context.Tasks.Where(t => t.ProjectId == projectId).ToList();
            ViewBag.ProjectId = projectId;
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create(int projectId)
        {
            var task = new TaskEntity
            {
                ProjectId = projectId
            };

            return View(task);
        }

        [HttpPost]
        public IActionResult Create(int projectId, string name, string description)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId);

            var task = new TaskEntity
            {
                Name = name,
                Description = description,
                Status = "To Do",
                ProjectId = projectId
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction("Details", "Project", new { id = projectId });
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return NotFound();

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskEntity model)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == model.Id);
            if (task != null)
            {
                task.Name = model.Name;
                task.Description = model.Description;
                task.Status = model.Status;
                _context.SaveChanges();
            }
            return RedirectToAction("Details", "Project", new { taskId = model.ProjectId });
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                var projectId = task.ProjectId;

                _context.Tasks.Remove(task);
                _context.SaveChanges();

                return RedirectToAction("Details", "Project", new { taskId = projectId });
            }

            return NotFound();
        }


        [HttpPost]
        public IActionResult UpdateStatus([FromBody] TaskUpdateModel model)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == model.TaskId);
            if (task != null)
            {
                task.Status = model.Status;
                _context.SaveChanges();
            }
            return Ok();
        }

        public class TaskUpdateModel
        {
            public int TaskId { get; set; }
            public string Status { get; set; }
        }

[HttpGet]
public IActionResult Details(int id)
{
    var task = _context.Tasks
        .FirstOrDefault(t => t.Id == id);

    if (task == null)
        return NotFound();

    var project = _context.Projects
        .FirstOrDefault(p => p.Id == task.ProjectId);

    if (project == null)
        return NotFound();

    var subtasks = _context.Subtasks
        .Where(s => s.TaskId == id)
        .ToList();

    ViewBag.Subtasks = subtasks;
    ViewBag.ProjectId = project.Id; // Pass the project ID to the view
    ViewBag.TaskId = id;

    return View(task); // Pass the task model to the view
}




    }
}
