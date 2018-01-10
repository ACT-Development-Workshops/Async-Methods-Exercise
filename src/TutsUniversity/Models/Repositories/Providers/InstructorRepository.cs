using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();

        public void Add(Instructor instructor)
        {
            context.Instructors.Add(instructor);
            context.SaveChanges();
        }

        public void Delete(int instructorId)
        {
            var instructor = GetInstructor(instructorId);

            instructor.OfficeAssignment = null;
            context.Instructors.Remove(instructor);

            var department = context.Departments.SingleOrDefault(d => d.InstructorId == instructorId);
            if (department != null)
                department.InstructorId = null;

            context.SaveChanges();
        }

        public Instructor GetInstructor(int instructorId)
        {
            return context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Single(i => i.Id == instructorId);
        }

        public IEnumerable<Instructor> GetInstructors()
        {
            return context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses.Select(c => c.Department))
                .OrderBy(i => i.LastName);
        }

        public void Update(int instructorId, string lastName, string firstMidName, DateTime hireDate, string location, IEnumerable<int> selectedCourseIds)
        {
            var instructor = GetInstructor(instructorId);

            if (string.IsNullOrWhiteSpace(location))
                instructor.OfficeAssignment = null;

            UpdateCourses();

            context.SaveChanges();

            void UpdateCourses()
            {
                if (selectedCourseIds == null)
                {
                    instructor.Courses = new List<Course>();
                    return;
                }

                var currentCourseIds = instructor.Courses
                    .Select(c => c.Id)
                    .ToList();

                foreach (var course in context.Courses)
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