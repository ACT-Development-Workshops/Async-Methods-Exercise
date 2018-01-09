using System;
using System.Collections.Generic;

namespace TutsUniversity.Models.Repositories
{
    public interface ICourseRepository : IDisposable
    {
        void Add(Course course);
        void Delete(int id);
        Course GetCourse(int id);
        IEnumerable<Course> GetCourses(int? departmentId);
        void Update(int courseId, string title, int credits, int departmentId);
    }
}