using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models.ViewModel
{
    public class SubjectSelectViewModel
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public bool IsSelected { get; set; }
    }
}