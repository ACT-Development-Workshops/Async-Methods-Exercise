using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class CourseRepository : ICourseRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();

        public Task Add(Course course)
        {
            context.Courses.Add(course);
            return context.SaveChangesAsync();
        }

        public async Task Delete(int courseId)
        {
            context.Courses.Remove(await GetCourse(courseId).ConfigureAwait(false));
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<Course> GetCourse(int courseId)
        {
            return context.Courses.SingleAsync(c => c.Id == courseId);
        }

        public Task<List<Course>> GetCourses(int? departmentId = null)
        {
            return context.Courses
                .Where(c => !departmentId.HasValue || c.DepartmentId == departmentId)
                .OrderBy(d => d.Id)
                .Include(d => d.Department)
                .ToListAsync();
        }

        public async Task Update(int courseId, int credits)
        {
            var course = await GetCourse(courseId).ConfigureAwait(false);
            course.Credits = credits;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(int courseId, string title, int credits, int departmentId)
        {
            var course = await GetCourse(courseId).ConfigureAwait(false);

            course.Title = title;
            course.Credits = credits;
            course.DepartmentId = departmentId;

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose() => context.Dispose();
    }
}