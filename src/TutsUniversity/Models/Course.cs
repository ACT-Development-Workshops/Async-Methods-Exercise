﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutsUniversity.Models
{
    public partial class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Department is Required")]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}