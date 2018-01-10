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

        public void Delete(int courseId)
        {
            context.Courses.Remove(GetCourse(courseId));
            context.SaveChanges();
        }

        public Course GetCourse(int courseId)
        {
            return context.Courses.Single(c => c.Id == courseId);
        }

        public IEnumerable<Course> GetCourses(int? departmentId)
        {
            return context.Courses
                .Where(c => !departmentId.HasValue || c.DepartmentId == departmentId)
                .OrderBy(d => d.Id)
                .Include(d => d.Department)
                .ToList();
        }

        public void Update(int courseId, int credits)
        {
            var course = GetCourse(courseId);
            course.Credits = credits;
            context.SaveChanges();
        }

        public void Update(int courseId, string title, int credits, int departmentId)
        {
            var course = GetCourse(courseId);

            course.Title = title;
            course.Credits = credits;
            course.DepartmentId = departmentId;

            context.SaveChanges();
        }

        public void Dispose() => context.Dispose();
    }
}