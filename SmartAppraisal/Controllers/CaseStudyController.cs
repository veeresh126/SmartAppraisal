using BL_SmartAppraisal.Interfaces;
using DL_SmartAppraisal.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SmartAppraisal.Controllers
{
    public class CaseStudyController : Controller
    {
        private readonly ICaseStudyService _caseStudyService;

        public CaseStudyController(
            ICaseStudyService caseStudyService)
        {
            _caseStudyService = caseStudyService;
        }


        // ==========================================
        // View Case Studies of logged-in user
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userDbId = HttpContext.Session.GetInt32("UserDbId");

            if (!userDbId.HasValue)
            {
                return RedirectToAction("Login", "UserManagement");
            }

            var caseStudies =
                await _caseStudyService.GetByUserIdAsync(userDbId.Value);

            return View(caseStudies);
        }


        // ==========================================
        // Show Create Page
        // ==========================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // ==========================================
        // Save Case Study
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> Create(CaseStudy model)
        {
            var userDbId = HttpContext.Session.GetInt32("UserDbId");

            if (!userDbId.HasValue)
            {
                return RedirectToAction("Login", "UserManagement");
            }

            // Link CaseStudy to UserDetail.Id
            model.CreatedBy = userDbId.Value;

            model.Solutions ??= new List<CaseStudySolution>();

            model.Solutions = model.Solutions
                .Where(x => !string.IsNullOrWhiteSpace(x.SolutionText))
                .ToList();

            for (int i = 0; i < model.Solutions.Count; i++)
            {
                model.Solutions[i].SolutionNumber = i + 1;
            }

            await _caseStudyService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }


        // ==========================================
        // Review Case Study
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            var caseStudy =
                await _caseStudyService.GetByIdAsync(id);

            if (caseStudy == null)
            {
                return NotFound();
            }

            return View(caseStudy);
        }
    }
}