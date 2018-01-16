using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TutsUniversity.Models.Repositories
{
    public interface ICourseRepository : IDisposable
    {
        Task Add(Course course);

        Task Delete(int courseId);

        Task<Course> GetCourse(int courseId);

        Task<List<Course>> GetCourses(int? departmentId = null);

        Task Update(int courseId, int credits);

        Task Update(int courseId, string title, int credits, int departmentId);
    }
}