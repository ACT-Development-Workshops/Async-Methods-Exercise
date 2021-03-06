﻿using System.Collections.Generic;

namespace TutsUniversity.Models
{
    public class InstructorList
    {
        public IEnumerable<Instructor> Instructors { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}