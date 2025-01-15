using Microsoft.AspNetCore.Mvc;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Task_Manager.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProjectController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Index()
        {
            // var userId = _userManager.GetUserId(User); 
            // var projects = _context.Projects.Where(p => p.UserId == userId).ToList(); 

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
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            _context.Projects.Add(new ProjectEntity
            {
                Name = name,
                Description = description,
                UserId = userId 
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
            return RedirectToAction("Details", new { taskId = project.Id });
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
