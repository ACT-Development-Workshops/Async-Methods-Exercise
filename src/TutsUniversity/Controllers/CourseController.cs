using System.Linq;
using System.Web.Mvc;
using TutsUniversity.Infrastructure.Data;
using TutsUniversity.Infrastructure.Messaging;
using TutsUniversity.Infrastructure.Messaging.Providers;
using TutsUniversity.Models;
using TutsUniversity.Models.Commands;
using TutsUniversity.Models.Repositories;
using TutsUniversity.Models.Repositories.Providers;

namespace TutsUniversity.Controllers
{
    public class CourseController : Controller
    {
        private readonly IBus bus = new InMemoryBus();
        private readonly ICourseRepository repository = new CourseRepository();
        private TutsUniversityContext db = new TutsUniversityContext();

        public ActionResult Index(int? departmentId)
        {
            var departments = db.Departments.OrderBy(q => q.Name).ToList();
            ViewBag.SelectedDepartment = new SelectList(departments, "DepartmentID", "Name", departmentId);

            var courses = repository.GetCourses(departmentId);
            return View(courses.ToList());
        }

        public ActionResult Details(int id)
        {
            var course = db.Courses.Find(id);
            return View(course);
        }

        public ActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")]Course course)
        {
            if (ModelState.IsValid)
            {
                repository.Add(course);
                return RedirectToAction("Index");
            }

            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        public ActionResult Edit(int id)
        {
            var course = repository.GetCourse(id);
            
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id, string title, int credits, int departmentID)
        {
            if (ModelState.IsValid)
            {
                repository.Update(id, title, credits, departmentID);
                return RedirectToAction("Index");
            }

            PopulateDepartmentsDropDownList(departmentID);
            return View(new Course { CourseID = id, Credits = credits, DepartmentID = departmentID, Title = title });
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in db.Departments
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
        }

        public ActionResult Delete(int id)
        {
            var course = repository.GetCourse(id);
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCourseCredits()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCourseCredits(int multiplier)
        {
            foreach (var course in repository.GetCourses())
                bus.Send(new UpdateCourseCredits { CourseId = course.CourseID, Credits = course.Credits * multiplier });

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                repository.Dispose();

            base.Dispose(disposing);
        }
    }
}