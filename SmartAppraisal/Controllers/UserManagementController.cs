using Microsoft.AspNetCore.Mvc;

namespace SmartAppraisal.Controllers
{
    public class UserManagementController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}