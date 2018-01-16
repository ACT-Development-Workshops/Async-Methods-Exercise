using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TutsUniversity.Models.Repositories
{
    public interface IDepartmentRepository : IDisposable
    {
        Task Add(Department department);

        Task Delete(int departmentId);

        Task<Department> GetDepartment(int departmentId);

        Task<IEnumerable<Department>> GetDepartments();

        Task Update(int departmentId, string name, decimal budget, DateTime startDate, int? instructorId, byte[] version);
    }
}