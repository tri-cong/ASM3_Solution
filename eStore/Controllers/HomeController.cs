using eStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Http;
using BussinessObject.Models;
using DataAccess.Repository;

namespace eStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IMemberRepository _memberRepository;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _memberRepository = new MemberRepository();
        }

        public IActionResult Index(string email, string password)
        {
            if (email != null && password != null)
            {
                @ViewData["email"] = email.Trim();
                @ViewData["password"] = password.Trim();
            }
            else
            {
                return View();
            }

            if (email.Length > 0 && password.Length > 0)
            {
                string role = "";
                Login login = new Login();
                try
                {
                    role = login.CheckLogin(email, password);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Please input right email and password";
                }
                if (role == "admin")
                {
                    HttpContext.Session.SetString("Role", role);
                    HttpContext.Session.SetString("Email", email);
                    return RedirectToAction("Index", "Products");
                }
                else if (role == "user")
                {
                    var member = _memberRepository.GetMemberByEmail(email);
                    HttpContext.Session.SetString("Role", role);
                    HttpContext.Session.SetString("Email", email);
                    return RedirectToAction("Details", "Members", new {id = member.MemberId});
                }
            }
            else
            {
                ViewBag.Error = "Please input right email and password";
            }

            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Role");
            return RedirectToAction(nameof(Index));
        }
    }
}
