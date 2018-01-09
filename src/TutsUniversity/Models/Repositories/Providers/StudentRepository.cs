using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class StudentRepository : IStudentRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();
        
        public void Add(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
        }

        public void Delete(int studentId)
        {
            context.Students.Remove(GetStudent(studentId));
            context.SaveChanges();
        }

        public IEnumerable<DailyEnrollmentTotals> GetDailyEnrollmentTotals()
        {
            return context.Students
                .GroupBy(s => s.EnrollmentDate)
                .Select(dateGrouping => new DailyEnrollmentTotals
                {
                    EnrollmentDate = dateGrouping.Key,
                    StudentCount = dateGrouping.Count()
                })
                .ToList();
        }

        public Student GetStudent(int studentId)
        {
            return context.Students.Single(s => s.ID == studentId);
        }

        public IPagedList<Student> Search(StudentSearchOptions searchOptions)
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

            return students.ToPagedList(searchOptions.PageNumber, searchOptions.PageSize);
        }

        public void Update(int studentId, string lastName, string firstMidName, DateTime enrollmentDate)
        {
            var student = GetStudent(studentId);

            student.EnrollmentDate = enrollmentDate;
            student.FirstMidName = firstMidName;
            student.LastName = lastName;

            context.SaveChanges();
        }

        public void Dispose() => context.Dispose();
    }
}