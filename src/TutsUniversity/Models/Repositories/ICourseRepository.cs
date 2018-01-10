using System;
using System.Collections.Generic;

namespace TutsUniversity.Models.Repositories
{
    public interface ICourseRepository : IDisposable
    {
        void Add(Course course);

        void Delete(int courseId);

        Course GetCourse(int courseId);

        IEnumerable<Course> GetCourses(int? departmentId = null);

        void Update(int courseId, int credits);

        void Update(int courseId, string title, int credits, int departmentId);
    }
}