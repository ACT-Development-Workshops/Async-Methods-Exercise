using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class StudentRepository : IStudentRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();
        
        public Task Add(Student student)
        {
            context.Students.Add(student);
            return context.SaveChangesAsync();
        }

        public async Task Delete(int studentId)
        {
            context.Students.Remove(await GetStudent(studentId).ConfigureAwait(false));
            await context.SaveChangesAsync();
        }

        public Task<List<DailyEnrollmentTotals>> GetDailyEnrollmentTotals()
        {
            return context.Students
                .GroupBy(s => s.EnrollmentDate)
                .Select(dateGrouping => new DailyEnrollmentTotals
                {
                    EnrollmentDate = dateGrouping.Key,
                    StudentCount = dateGrouping.Count()
                })
                .ToListAsync();
        }

        public Task<Student> GetStudent(int studentId)
        {
            return context.Students.SingleAsync(s => s.Id == studentId);
        }

        public async Task<IPagedList<Student>> Search(StudentSearchOptions searchOptions)
        {
            var students = context.Students.Select(s => s);
            if (!string.IsNullOrEmpty(searchOptions.NameSearch))
                students = students.Where(s => s.LastName.Contains(searchOptions.NameSearch) || s.FirstMidName.Contains(searchOptions.NameSearch));

            switch (searchOptions.SortOptions)
            {
                default:
                case StudentSortOptions.NameAscending:
                    students = students.OrderBy(s => s.LastName);
                    break;
                case StudentSortOptions.NameDescending:
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case StudentSortOptions.DateAscending:
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case StudentSortOptions.DateDescending:
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
            }

            return (await students.ToListAsync().ConfigureAwait(false)).ToPagedList(searchOptions.PageNumber, searchOptions.PageSize);
        }

        public async Task Update(int studentId, string lastName, string firstMidName, DateTime enrollmentDate)
        {
            var student = await GetStudent(studentId).ConfigureAwait(false);

            student.EnrollmentDate = enrollmentDate;
            student.FirstMidName = firstMidName;
            student.LastName = lastName;

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose() => context.Dispose();
    }
}