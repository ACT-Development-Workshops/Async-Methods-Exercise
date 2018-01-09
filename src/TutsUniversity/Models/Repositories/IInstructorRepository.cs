using System;
using System.Collections.Generic;

namespace TutsUniversity.Models.Repositories
{
    public interface IInstructorRepository : IDisposable
    {
        void Add(Instructor instructor);

        void Delete(int instructorId);

        Instructor GetInstructor(int instructorId);

        IEnumerable<Instructor> GetInstructors();

        void Update(int instructorId, string lastName, string firstMidName, DateTime hireDate, string location, IEnumerable<int> selectedCourseIds);
    }
}