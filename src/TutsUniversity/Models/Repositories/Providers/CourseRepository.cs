using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class CourseRepository : ICourseRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();

        public void Add(Course course)
        {
            context.Courses.Add(course);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            context.Courses.Remove(GetCourse(id));
            context.SaveChanges();
        }

        public Course GetCourse(int id)
        {
            return context.Courses.Single(c => c.CourseID == id);
        }

        public IEnumerable<Course> GetCourses(int? departmentId)
        {
            return context.Courses
                .Where(c => !departmentId.HasValue || c.DepartmentID == departmentId)
                .OrderBy(d => d.CourseID)
                .Include(d => d.Department)
                .ToList();
        }

        public void Update(int courseId, string title, int credits, int departmentId)
        {
            var course = GetCourse(courseId);

            course.Title = title;
            course.Credits = credits;
            course.DepartmentID = departmentId;

            context.SaveChanges();
        }

        public void Dispose() => context?.Dispose();
    }
}