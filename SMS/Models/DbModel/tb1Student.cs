//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMS.Models.DbModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb1Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb1Student()
        {
            this.tblStudentSubjects = new HashSet<tblStudentSubject>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string FatherName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> FacultyId { get; set; }
    
        public virtual TblFaculty TblFaculty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblStudentSubject> tblStudentSubjects { get; set; }
        public string Gender { get; internal set; }
        public DateTime DateOfBirth { get; internal set; }
    }
}