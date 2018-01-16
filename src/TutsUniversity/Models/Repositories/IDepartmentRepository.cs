using System;
using System.Collections.Generic;

namespace TutsUniversity.Models.Repositories
{
    public interface IDepartmentRepository : IDisposable
    {
        void Add(Department department);

        void Delete(int departmentId);

        Department GetDepartment(int departmentId);

        IEnumerable<Department> GetDepartments();

        void Update(int departmentId, string name, decimal budget, DateTime startDate, int? instructorId, byte[] version);
    }
}