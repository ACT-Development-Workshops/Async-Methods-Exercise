using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TutsUniversity.Models
{
    public class Instructor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}