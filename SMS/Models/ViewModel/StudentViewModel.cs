using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMS.Models.ViewModel
{
    public class StudentViewModel
    {
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string FatherName { get; set; }
        public Nullable<int> FacultyId {get; set;}
        public string FacultyName { get; set; }
        public string Section { get; set; }
        public List<SubjectSelectViewModel> Subjects { get; set; }
        public string SubjectsNames { get; set; }
        public int Id { get; set; }
    }
}