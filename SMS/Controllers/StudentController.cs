using SMS.Common;
using SMS.Models.DbModel;
using SMS.Models.Repository;
using SMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace SMS.Controllers
{
  //  [CookieFilterAttribute]
    public class StudentController : Controller
    {
        // GET: Student
        //[SessionStateFilter]
        
        public ActionResult Index()
        {          
           
            /*if(Session["email"]==null)
            {
                return RedirectToAction("Login","Account");
            }*/
            StudentRepository studentRepository = new StudentRepository();
            var students = studentRepository.GetAllStudents();
            return View(students);
        }
        public ActionResult Create()
        {
            SchoolManagementSystemEntities _db = new SchoolManagementSystemEntities(); 
            StudentRepository studentRepository = new StudentRepository();
            StudentViewModel studentViewModel = new StudentViewModel();
            ViewBag.facultyList = (from faculty in _db.TblFaculties
                                   select new SelectListItem
                                   {
                                       Text = faculty.FacultyName+" "+faculty.Section,
                                       Value = faculty.Id.ToString()
                                   });
            
            studentViewModel.Subjects = studentRepository.GetAllSubject();
            return View(studentViewModel);
        }
      
        [HttpPost]
        public ActionResult Create(StudentViewModel student)
        {
            StudentRepository repo = new StudentRepository();
            repo.InsertStudent(student);
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            StudentRepository repo = new StudentRepository();
            SchoolManagementSystemEntities _db = new SchoolManagementSystemEntities();
            ViewBag.facultyList = (from faculty in _db.TblFaculties
                                   select new SelectListItem
                                   {
                                       Text = faculty.FacultyName + "-" + faculty.Section,
                                       Value = faculty.Id.ToString()
                                   });
            var student = repo.GetStudentById(id);
            return View(student);
        }
        [HttpPost]
        public ActionResult Edit(StudentViewModel student)
        {
            StudentRepository repo = new StudentRepository();
            repo.UpdateStudent(student);
            return RedirectToAction("Index");           
        }

        public ActionResult Delete(int Id)
        {
            StudentRepository repo = new StudentRepository();
            repo.DeleteStudent(Id);
            return RedirectToAction("Index");
        }
        public ActionResult IndexAjax()
        {
            return View();
        }
        public ActionResult _IndexPartial()
        {
            StudentRepository studentRepository = new StudentRepository();
            var students = studentRepository.GetAllStudents();
            return View(students);
        }
    }
}
