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


        // =========================================================
        // CREATE - GET
        // URL: /CaseStudy/Create
        // =========================================================

        [HttpGet]
        public IActionResult Create()
        {
            var userDbId =
                HttpContext.Session.GetInt32("UserDbId");

            if (!userDbId.HasValue)
            {
                return RedirectToAction(
                    "Login",
                    "UserManagement");
            }

            return View();
        }


        // =========================================================
        // CREATE - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CaseStudy caseStudy,

            string Solution1,
            string Competency1,

            string Solution2,
            string Competency2,

            string Solution3,
            string Competency3,

            string Solution4,
            string Competency4)
        {
            // -----------------------------------------------------
            // CHECK LOGIN
            // -----------------------------------------------------

            var userDbId =
                HttpContext.Session.GetInt32("UserDbId");

            if (!userDbId.HasValue)
            {
                return RedirectToAction(
                    "Login",
                    "UserManagement");
            }


            // -----------------------------------------------------
            // MODEL VALIDATION
            // -----------------------------------------------------

            if (!ModelState.IsValid)
            {
                return View(caseStudy);
            }


            // -----------------------------------------------------
            // CASE STUDY DETAILS
            // -----------------------------------------------------

            caseStudy.CreatedBy =
                userDbId.Value;

            caseStudy.Status =
                CaseStudyStatus.Pending;

            caseStudy.CreatedDate =
                DateTime.UtcNow;

            caseStudy.ModifiedDate = null;

            caseStudy.ReviewComment = null;

            caseStudy.ReviewedBy = null;

            caseStudy.ReviewedDate = null;


            // -----------------------------------------------------
            // SOLUTIONS
            // -----------------------------------------------------

            caseStudy.Solutions =
                new List<CaseStudySolution>();


            // -----------------------------------------------------
            // SOLUTION 1
            // -----------------------------------------------------

            if (!string.IsNullOrWhiteSpace(Solution1))
            {
                var solution1 =
                    new CaseStudySolution
                    {
                        SolutionNumber = 1,
                        SolutionText = Solution1.Trim()
                    };

                if (!string.IsNullOrWhiteSpace(Competency1))
                {
                    solution1.Competencies.Add(
                        new CaseStudyCompetency
                        {
                            CompetencyName =
                                Competency1.Trim()
                        });
                }

                caseStudy.Solutions.Add(solution1);
            }


            // -----------------------------------------------------
            // SOLUTION 2
            // -----------------------------------------------------

            if (!string.IsNullOrWhiteSpace(Solution2))
            {
                var solution2 =
                    new CaseStudySolution
                    {
                        SolutionNumber = 2,
                        SolutionText = Solution2.Trim()
                    };

                if (!string.IsNullOrWhiteSpace(Competency2))
                {
                    solution2.Competencies.Add(
                        new CaseStudyCompetency
                        {
                            CompetencyName =
                                Competency2.Trim()
                        });
                }

                caseStudy.Solutions.Add(solution2);
            }


            // -----------------------------------------------------
            // SOLUTION 3
            // -----------------------------------------------------

            if (!string.IsNullOrWhiteSpace(Solution3))
            {
                var solution3 =
                    new CaseStudySolution
                    {
                        SolutionNumber = 3,
                        SolutionText = Solution3.Trim()
                    };

                if (!string.IsNullOrWhiteSpace(Competency3))
                {
                    solution3.Competencies.Add(
                        new CaseStudyCompetency
                        {
                            CompetencyName =
                                Competency3.Trim()
                        });
                }

                caseStudy.Solutions.Add(solution3);
            }


            // -----------------------------------------------------
            // SOLUTION 4
            // -----------------------------------------------------

            if (!string.IsNullOrWhiteSpace(Solution4))
            {
                var solution4 =
                    new CaseStudySolution
                    {
                        SolutionNumber = 4,
                        SolutionText = Solution4.Trim()
                    };

                if (!string.IsNullOrWhiteSpace(Competency4))
                {
                    solution4.Competencies.Add(
                        new CaseStudyCompetency
                        {
                            CompetencyName =
                                Competency4.Trim()
                        });
                }

                caseStudy.Solutions.Add(solution4);
            }


            // -----------------------------------------------------
            // SAVE
            // -----------------------------------------------------

            await _caseStudyService
                .CreateAsync(caseStudy);


            // -----------------------------------------------------
            // REDIRECT TO INDEX
            // IMPORTANT:
            // We are NOT redirecting to ViewList.
            // -----------------------------------------------------

            return RedirectToAction(
                nameof(Index));
        }


        // =========================================================
        // INDEX
        // URL: /CaseStudy/Index
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userDbId =
                HttpContext.Session.GetInt32("UserDbId");

            if (!userDbId.HasValue)
            {
                return RedirectToAction(
                    "Login",
                    "UserManagement");
            }


            var caseStudies =
                await _caseStudyService
                    .GetByUserIdAsync(
                        userDbId.Value);


            return View(caseStudies);
        }


        // =========================================================
        // DETAILS
        // URL: /CaseStudy/Details/5
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userDbId =
                HttpContext.Session.GetInt32("UserDbId");

            if (!userDbId.HasValue)
            {
                return RedirectToAction(
                    "Login",
                    "UserManagement");
            }


            var caseStudy =
                await _caseStudyService
                    .GetByIdAsync(id);


            if (caseStudy == null)
            {
                return NotFound();
            }


            if (caseStudy.CreatedBy != userDbId.Value)
            {
                return Forbid();
            }


            return View(caseStudy);
        }


        // =========================================================
        // REVIEW LIST
        // URL: /CaseStudy/ReviewList
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> ReviewList()
        {
            var userDbId =
                HttpContext.Session.GetInt32("UserDbId");

            if (!userDbId.HasValue)
            {
                return RedirectToAction(
                    "Login",
                    "UserManagement");
            }


            var caseStudies =
                await _caseStudyService
                    .GetAllAsync();


            var pending =
                caseStudies
                    .Where(x =>
                        x.Status ==
                        CaseStudyStatus.Pending)
                    .ToList();


            return View(pending);
        }


        // =========================================================
        // REVIEW - GET
        // Uses EXISTING Review.cshtml
        // No new page required.
        // URL: /CaseStudy/Review/5
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            var reviewerId =
                HttpContext.Session.GetInt32(
                    "UserDbId");

            if (!reviewerId.HasValue)
            {
                return RedirectToAction(
                    "Login",
                    "UserManagement");
            }


            var caseStudy =
                await _caseStudyService
                    .GetByIdAsync(id);


            if (caseStudy == null)
            {
                return NotFound();
            }


            return View("Review", caseStudy);
        }


        // =========================================================
        // REVIEW - POST
        // Uses EXISTING Review.cshtml
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(
            int caseStudyId,
            bool approved,
            string? reviewComment)
        {
            var reviewerId =
                HttpContext.Session.GetInt32(
                    "UserDbId");

            if (!reviewerId.HasValue)
            {
                return RedirectToAction(
                    "Login",
                    "UserManagement");
            }


            var result =
                await _caseStudyService
                    .ReviewAsync(
                        caseStudyId,
                        reviewerId.Value,
                        approved,
                        reviewComment);


            if (!result)
            {
                return NotFound();
            }


            return RedirectToAction(
                nameof(ReviewList));
        }
    }
}