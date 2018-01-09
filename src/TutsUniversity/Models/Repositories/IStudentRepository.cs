using System;
using System.Collections.Generic;
using PagedList;

namespace TutsUniversity.Models.Repositories
{
    public interface IStudentRepository : IDisposable
    {
        void Add(Student student);

        void Delete(int studentId);

        IEnumerable<DailyEnrollmentTotals> GetDailyEnrollmentTotals();

        Student GetStudent(int studentId);

        IPagedList<Student> Search(StudentSearchOptions searchOptions);

        void Update(int studentId, string lastName, string firstMidName, DateTime enrollmentDate);
    }

    public class StudentSearchOptions
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 3;

        public string NameSearch { get; set; }

        public StudentSortOptions SortOptions { get; set; }
    }

    public enum StudentSortOptions
    {
        NameDescending,
        NameAscending,
        DateDescending,
        DateAscending
    }
}