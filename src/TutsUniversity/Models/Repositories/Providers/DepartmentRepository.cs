using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();

        public Task Add(Department department)
        {
            context.Departments.Add(department);
            return context.SaveChangesAsync();
        }

        public async Task Delete(int departmentId)
        {
            context.Departments.Remove(await GetDepartment(departmentId).ConfigureAwait(false));
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<Department> GetDepartment(int departmentId)
        {
            return context.Departments.SingleAsync(d => d.Id == departmentId);
        }

        public Task<List<Department>> GetDepartments()
        {
            return context.Departments.OrderBy(d => d.Name).ToListAsync();
        }

        public async Task Update(int departmentId, string name, decimal budget, DateTime startDate, int? instructorId, byte[] version)
        {
            var department = await GetDepartment(departmentId).ConfigureAwait(false);

            department.Budget = budget;
            department.InstructorId = instructorId;
            department.Name = name;
            department.RowVersion = version;
            department.StartDate = startDate;

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose() => context.Dispose();
    }
}