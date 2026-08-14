using Microsoft.AspNetCore.Mvc;

namespace SmartAppraisal.Controllers
{
    public class UserManagementController :Controller
    {
        public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult ChangePassword()
    {
        return View();
    }
}
}
