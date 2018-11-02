using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iSTLDevEx.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //return View();
                return RedirectToAction("Index", "Operator");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


    }
}