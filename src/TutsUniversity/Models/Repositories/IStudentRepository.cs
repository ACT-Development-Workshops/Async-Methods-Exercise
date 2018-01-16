using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagedList;

namespace TutsUniversity.Models.Repositories
{
    public interface IStudentRepository : IDisposable
    {
        Task Add(Student student);

        Task Delete(int studentId);

        Task<List<DailyEnrollmentTotals>> GetDailyEnrollmentTotals();

        Task<Student> GetStudent(int studentId);

        Task<IPagedList<Student>> Search(StudentSearchOptions searchOptions);

        Task Update(int studentId, string lastName, string firstMidName, DateTime enrollmentDate);
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