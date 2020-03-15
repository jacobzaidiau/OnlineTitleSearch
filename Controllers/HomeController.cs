using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineTitleSearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Online Title Search Automation Tool";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me!";

            return View();
        }
    }
}