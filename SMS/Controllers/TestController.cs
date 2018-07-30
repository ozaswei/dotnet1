using SMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [Authorize(Roles ="Admin")]
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string studentName, string studentAddress,string dob)
        {
            ViewBag.studentname = studentName;
            ViewBag.studentaddress = studentAddress;
            ViewBag.dob = dob;
            return View();
        }
        public ActionResult Create2()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create2(StudentViewModel student)
        {
            if (!ModelState.IsValid)
                return View();
            return RedirectToAction("Index");
            

        }
    }
}