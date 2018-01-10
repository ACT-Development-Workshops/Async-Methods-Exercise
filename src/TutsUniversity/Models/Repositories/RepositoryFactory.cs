using TutsUniversity.Models.Repositories.Providers;

namespace TutsUniversity.Models.Repositories
{
    public static class RepositoryFactory
    {
        public static ICourseRepository Courses => new CourseRepository();

        public static IDepartmentRepository Departments = new DepartmentRepository();

        public static IInstructorRepository Instructors = new InstructorRepository();

        public static IStudentRepository Students = new StudentRepository();

        public static IUpdateRepository Updates = new UpdateRepository();
    }
}