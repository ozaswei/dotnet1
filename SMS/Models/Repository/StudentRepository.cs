using SMS.Models.DbModel;
using SMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models.Repository
{
    public class StudentRepository
    {
        private readonly SchoolManagementSystemEntities _db = new SchoolManagementSystemEntities();
        public List<StudentViewModel>GetAllStudents()
        {
            //List<tb1Student> Students = new List<tb1Student>();
            //LINQ(language Intergragted Query) type
            //Students = (from student in _db.tb1students select student).ToList();
            //Lamda Expression type
            //var student = _db.tb1Student.Select(x => x).ToList();
            List<StudentViewModel> studentViewModels = (from student in _db.tb1Student select new StudentViewModel()
            {
                Id=student.Id,
                Name=student.Name,
                Address=student.Address,
                PhoneNumber=student.PhoneNumber,
                FatherName=student.FatherName,
                Gender=student.Gender,
                DateOfBirth=student.DateOfBirth,
                FacultyId=student.FacultyId,
                FacultyName=student.TblFaculty.FacultyName,
                Section=student.TblFaculty.Section
            }).ToList();
            foreach(var student in studentViewModels)
            {
                student.SubjectsNames = GetSubjectsById(student.Id);
            }
            return studentViewModels;
        }
        public List<SubjectSelectViewModel> GetAllSubject()
            {
                var subjects=(from subject in _db.tblSubjects select new SubjectSelectViewModel()
                    {
                    Id= subject.Id,
                    SubjectName=subject.Name
                }).ToList();
            return subjects;
        }

        public bool InsertStudent(StudentViewModel studentVm)
        {
            tb1Student student = new tb1Student()
            {
                Name = studentVm.Name,
                Address = studentVm.Address,
                DateOfBirth = studentVm.DateOfBirth,
                Gender = studentVm.Gender,
                FacultyId = studentVm.FacultyId,
                FatherName = studentVm.FatherName,
                PhoneNumber = studentVm.PhoneNumber
            };
            _db.tb1Student.Add(student);
            int id = _db.SaveChanges();
            if(id>0)
            {
                List<int> subjectIds = studentVm.Subjects.Where(x => x.IsSelected==true).Select(x => x.Id).ToList();
                InsertStudentSubjects(subjectIds, student.Id);
            }
            return true;
        }
        public bool InsertStudentSubjects(List<int> subjectIds,int studentId)
        {
            bool status = false;
            foreach (var subjectId in subjectIds)
            {
                tblStudentSubject studentSubject = new tblStudentSubject()
                {
                    StudentId = studentId,
                    SubjectId = subjectId

                };
                _db.tblStudentSubjects.Add(studentSubject);
                _db.SaveChanges();
                status = true;
            }
            return status;
        }
        public string GetSubjectsById(int studentId)
        {
            List<string> subjectNames = _db.tblStudentSubjects.Where(x => x.StudentId==studentId).Select(x => x.tblSubject.Name).ToList();
            return string.Join(" ,", subjectNames);
        }
        public StudentViewModel GetStudentById(int id)
        {
            var student = (from stud in _db.tb1Student
                           where stud.Id == id
                           select new StudentViewModel()
                           {
                               Id=stud.Id,
                               Name = stud.Name,
                               Address = stud.Address,
                               PhoneNumber = stud.PhoneNumber,
                               FatherName = stud.FatherName,
                               Gender = stud.Gender,
                               DateOfBirth = stud.DateOfBirth,
                               FacultyId = stud.FacultyId,
                               FacultyName = stud.TblFaculty.FacultyName,
                               Section = stud.TblFaculty.Section
                           }).FirstOrDefault();
            if(student != null)
            student.Subjects = GetAssignedSubjectsById(id);
            return student;
        }
        public List<SubjectSelectViewModel> GetAssignedSubjectsById(int studentId)
        {
            List<SubjectSelectViewModel> subjects = GetAllSubject();
            List<int?> selectedIds = _db.tblStudentSubjects.Where(x => x.StudentId == studentId)
                .Select(x => x.SubjectId).ToList();
            foreach(var id in selectedIds)
            {
                foreach(var subject in subjects)
                {
                    if(subject.Id==id)
                    {
                        subject.IsSelected = true;
                    }
                }
            }
            return subjects;
        }
        public bool UpdateStudent(StudentViewModel student)
        {
            tb1Student oldStudent = _db.tb1Student.Find(student.Id);
            tb1Student newStudent = new tb1Student
            {
                Id = student.Id,
                Name = student.Name,
                Address = student.Address,
                PhoneNumber = student.PhoneNumber,
                Gender = student.Gender,
                FatherName=student.FatherName,
                DateOfBirth = student.DateOfBirth,
                FacultyId = student.FacultyId
            };
            _db.Entry(oldStudent).CurrentValues.SetValues(newStudent);
            UpdateStudentSubjects(student.Subjects, student.Id);
            return _db.SaveChanges() >0;
        }
        public bool UpdateStudentSubjects(List<SubjectSelectViewModel>subjectSelectViewModels,int studentId)
        {
            List<int> selectedValues = subjectSelectViewModels.Where(x => x.IsSelected == true).Select(x=>x.Id).ToList();
            List<int> deselectedValues = subjectSelectViewModels.Where(x => x.IsSelected == false).Select(x => x.Id).ToList();
            foreach(var id in selectedValues)
            {
                var subject = (from substud in _db.tblStudentSubjects
                               where substud.StudentId == studentId && substud.SubjectId == id
                               select substud).FirstOrDefault();
                if(subject ==null)
                {
                    tblStudentSubject studentSubject = new tblStudentSubject()
                    {
                        StudentId = studentId,
                        SubjectId=id

                    };
                    _db.tblStudentSubjects.Add(studentSubject);

                }
                    
            }
            foreach(var subjectId in deselectedValues)
            {
                var subject = (from substud in _db.tblStudentSubjects
                               where substud.StudentId == studentId && substud.SubjectId == subjectId
                               select substud).FirstOrDefault();
                if (subject != null)
                {
                    _db.tblStudentSubjects.Remove(subject);
                    _db.SaveChanges();
                }
            }
            return true;
        }
        public bool DeleteStudent(int id)
        {
            tb1Student student = _db.tb1Student.Find(id);
            List<tblStudentSubject> studentSubjects = (from ss in _db.tblStudentSubjects where ss.StudentId == id select ss).ToList();
            foreach(var studentSubject in studentSubjects)
            {
                _db.tblStudentSubjects.Attach(studentSubject);
                _db.tblStudentSubjects.Remove(studentSubject);
            }
            if(student !=null)
            {
                _db.tb1Student.Attach(student);
                _db.tb1Student.Remove(student);
            }
            return _db.SaveChanges()>0;
        }
    }
}