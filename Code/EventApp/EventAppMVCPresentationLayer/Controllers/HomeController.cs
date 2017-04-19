using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventAppMVCPresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to Hotel Inn";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "We want to know!";

            return View();
        }
    }
}