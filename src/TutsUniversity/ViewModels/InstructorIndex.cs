using System.Collections.Generic;
using TutsUniversity.Models;

namespace TutsUniversity.ViewModels
{
    public class InstructorIndex
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}

