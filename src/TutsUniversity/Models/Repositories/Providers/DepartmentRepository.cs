using System;
using System.Collections.Generic;
using System.Linq;
using TutsUniversity.Infrastructure.Data;

namespace TutsUniversity.Models.Repositories.Providers
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly TutsUniversityContext context = new TutsUniversityContext();

        public void Add(Department department)
        {
            context.Departments.Add(department);
            context.SaveChanges();
        }

        public void Delete(int departmentId)
        {
            context.Departments.Remove(GetDepartment(departmentId));
            context.SaveChanges();
        }

        public Department GetDepartment(int departmentId)
        {
            return context.Departments.Single(d => d.DepartmentID == departmentId);
        }

        public IEnumerable<Department> GetDepartments()
        {
            return context.Departments.OrderBy(q => q.Name).ToList();
        }

        public void Update(int id, string name, decimal budget, DateTime startDate, int instructorId, byte[] version)
        {
            var department = GetDepartment(id);

            department.Budget = budget;
            department.InstructorID = instructorId;
            department.Name = name;
            department.RowVersion = version;
            department.StartDate = startDate;

            context.SaveChanges();
        }

        public void Dispose() => context.Dispose();
    }
}