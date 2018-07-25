using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class Test2Controller : Controller
    {
        // GET: Test2
        public ActionResult Index()
        {
            return View();
        }
       public ActionResult Create2()
        {
            return View();
        }
        public ActionResult Create2get(string studentName, string studentAddress, string dob)
        {
            return RedirectToAction("Index");
        }
    }
}