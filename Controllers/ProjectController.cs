using Microsoft.AspNetCore.Mvc;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Task_Manager.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var projects = _context.Projects.ToList();
            return View(projects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string description)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized();
            }

            _context.Projects.Add(new ProjectEntity
            {
                Name = name,
                Description = description,
                UserId = userId.Value
            });
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound();

            var tasks = _context.Tasks.Where(t => t.ProjectId == id).ToList();
            ViewBag.Tasks = tasks;

            var projects = _context.Projects.ToList();
            ViewBag.Projects = projects; 
            ViewBag.CurrentProjectId = id; 

            return View(project); 
        }




        [HttpGet]
        public IActionResult Edit(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound();

            return View(project);
        }

        [HttpPost]
        public IActionResult Edit(int id, string name, string description)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound();

            project.Name = name;
            project.Description = description;

            _context.SaveChanges();
            return RedirectToAction("Details", new { id = project.Id });
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}
