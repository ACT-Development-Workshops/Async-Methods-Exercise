using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TutsUniversity.Models;

namespace TutsUniversity.Infrastructure.Data
{
    public class TutsUniversityContext : DbContext
    {
        static TutsUniversityContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TutsUniversityContext>());
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseId")
                    .MapRightKey("InstructorId")
                    .ToTable("CourseInstructor"));
        }       
    }
}