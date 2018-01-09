using TutsUniversity.Models.Repositories.Providers;

namespace TutsUniversity.Models.Repositories
{
    public static class RepositoryFactory
    {
        public static ICourseRepository Courses => new CourseRepository();
        public static IDepartmentRepository Departments = new DepartmentRepository();
    }
}