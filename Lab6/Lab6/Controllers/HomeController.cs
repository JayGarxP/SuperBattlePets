using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Super Battle Pets";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "We're NOT currently hiring OR accepting invitations to war";

            return View();
        }
    }
}