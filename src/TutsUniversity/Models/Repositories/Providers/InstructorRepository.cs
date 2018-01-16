using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();

        public async Task Add(Instructor instructor, int[] selectedCourseIds)
        {
            foreach (var course in await context.Courses.Where(course => selectedCourseIds.Contains(course.Id)).ToListAsync().ConfigureAwait(false))
                instructor.Courses.Add(course);

            context.Instructors.Add(instructor);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int instructorId)
        {
            var instructor = await GetInstructor(instructorId).ConfigureAwait(false);

            instructor.OfficeAssignment = null;
            context.Instructors.Remove(instructor);

            var department = await context.Departments.SingleOrDefaultAsync(d => d.InstructorId == instructorId).ConfigureAwait(false);
            if (department != null)
                department.InstructorId = null;

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<Instructor> GetInstructor(int instructorId)
        {
            return context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .SingleAsync(i => i.Id == instructorId);
        }

        public Task<List<Instructor>> GetInstructors()
        {
            return context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses.Select(c => c.Department))
                .OrderBy(i => i.LastName)
                .ToListAsync();
        }

        public async Task Update(int instructorId, string lastName, string firstMidName, DateTime hireDate, string location, IEnumerable<int> selectedCourseIds)
        {
            var instructor = await GetInstructor(instructorId).ConfigureAwait(false);

            instructor.FirstMidName = firstMidName;
            instructor.LastName = lastName;
            instructor.HireDate = hireDate;
            instructor.OfficeAssignment = !string.IsNullOrWhiteSpace(location) ? new OfficeAssignment {Location = location} : null;

            await UpdateCourses().ConfigureAwait(false);

            await context.SaveChangesAsync().ConfigureAwait(false);

            async Task UpdateCourses()
            {
                if (selectedCourseIds == null)
                {
                    instructor.Courses = new List<Course>();
                    return;
                }

                var currentCourseIds = instructor.Courses
                    .Select(c => c.Id)
                    .ToList();

                foreach (var course in await context.Courses.ToListAsync().ConfigureAwait(false))
                {
                    if (selectedCourseIds.Contains(course.Id))
                    {
                        if (!currentCourseIds.Contains(course.Id))
                            instructor.Courses.Add(course);
                    }
                    else
                    {
                        if (currentCourseIds.Contains(course.Id))
                            instructor.Courses.Remove(course);
                    }
                }
            }
        }

        public void Dispose() => context.Dispose();
    }
}