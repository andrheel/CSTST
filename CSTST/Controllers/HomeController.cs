using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSTST.Models;
using CSTST.Data;

namespace CSTST.Controllers
{
    public partial class HomeController : Controller
    {
        IDAL dal;

        public HomeController(IDAL _dal)
        {
            dal = _dal;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllUsers()
        {
            return View();
        }

        public IActionResult AllGroups()
        {
            return View();
        }

        public IActionResult AUser(int id)
        {
            return View(id);
        }

        public IActionResult AGroup(int id)
        {
            return View(id);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
