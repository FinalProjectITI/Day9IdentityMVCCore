
using AdminDashBoard.Models;
using AdminDashBoard.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AdminDashBoard.Controllers
{
   
    public class AccountController : Controller
    {
        AlaslyfactoryContext _context;

        public AccountController(AlaslyfactoryContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Admin admin)
        {
            try
            {
                Admin admim1 = _context.Admins.FirstOrDefault(A => A.UserName == admin.UserName);
                if (admim1 != null && admim1.Password == admin.Password)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login");
            }
        }
    }
}
