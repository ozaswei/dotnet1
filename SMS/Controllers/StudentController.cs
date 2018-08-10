using Microsoft.Reporting.WebForms;
using SMS.Common;
using SMS.Models.DbModel;
using SMS.Models.Repository;
using SMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
            return Json(true);
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
        public ActionResult _Create()
        {
            SchoolManagementSystemEntities _db = new SchoolManagementSystemEntities();
            StudentRepository studentRepository = new StudentRepository();
            StudentViewModel studentViewModel = new StudentViewModel();
            ViewBag.facultyList = (from faculty in _db.TblFaculties
                                   select new SelectListItem
                                   {
                                       Text = faculty.FacultyName + " " + faculty.Section,
                                       Value = faculty.Id.ToString()
                                   });

            studentViewModel.Subjects = studentRepository.GetAllSubject();
            return View(studentViewModel);
        }
        public ActionResult DownloadReport()
        {
            LocalReport lr = new LocalReport();
            StudentRepository repository = new StudentRepository();
            string path = Path.Combine(Server.MapPath("~/Views/Reports"), "StudentDetails.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {

            }
            var students = repository.GetAllStudents();
            ReportDataSource rd = new ReportDataSource("DataSet1", students);
            lr.DataSources.Add(rd);
            IList<ReportParameter> reportParameters = new List<ReportParameter>()
            {
                new ReportParameter("title","Test Report"),
                new ReportParameter("footer","Generated on"+DateTime.Now)
            };
            lr.SetParameters(reportParameters);
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderBytes;

            lr.Refresh();
            renderBytes = lr.Render(
                "PDF",
                null,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
                );
            Response.AddHeader("Content-Disposition", "attachment; filename=Test.pdf");
            return new FileContentResult(renderBytes, mimeType);
        }
    }
}
