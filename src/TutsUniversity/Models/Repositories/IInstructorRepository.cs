using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TutsUniversity.Models.Repositories
{
    public interface IInstructorRepository : IDisposable
    {
        Task Add(Instructor instructor, int[] selectedCourseIds);

        Task Delete(int instructorId);

        Task<Instructor> GetInstructor(int instructorId);

        Task<IEnumerable<Instructor>> GetInstructors();

        Task Update(int instructorId, string lastName, string firstMidName, DateTime hireDate, string location, IEnumerable<int> selectedCourseIds);
    }
}