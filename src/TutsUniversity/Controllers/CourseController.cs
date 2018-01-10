using System.Web.Mvc;
using TutsUniversity.Infrastructure.Messaging;
using TutsUniversity.Models;
using TutsUniversity.Models.Commands;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Controllers
{
    public class CourseController : Controller
    {
        private readonly Bus bus = Bus.Instance;
        private readonly ICourseRepository courseRepository = RepositoryFactory.Courses;
        private readonly IDepartmentRepository departmentRepository = RepositoryFactory.Departments;

        public ActionResult Index(int? departmentId)
        {
            var departments = departmentRepository.GetDepartments();
            ViewBag.SelectedDepartment = new SelectList(departments, "Id", "Name", departmentId);

            return View(courseRepository.GetCourses(departmentId));
        }

        public ActionResult Details(int id)
        {
            return View(courseRepository.GetCourse(id));
        }

        public ActionResult Create()
        {
            ListDepartments();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Credits,DepartmentId")]Course course)
        {
            if (ModelState.IsValid)
            {
                courseRepository.Add(course);
                return RedirectToAction("Index");
            }

            ListDepartments(course.DepartmentId);
            return View(course);
        }

        public ActionResult Edit(int id)
        {
            var course = courseRepository.GetCourse(id);
            ListDepartments(course.DepartmentId);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id, string title, int credits, int departmentId)
        {
            if (ModelState.IsValid)
            {
                courseRepository.Update(id, title, credits, departmentId);
                return RedirectToAction("Index");
            }

            ListDepartments(departmentId);
            return View(new Course { Id = id, Credits = credits, DepartmentId = departmentId, Title = title });
        }

        public ActionResult Delete(int id)
        {
            return View(courseRepository.GetCourse(id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            courseRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCourseCredits()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCourseCredits(int multiplier)
        {
            bus.Send(new MultiplyCourseCredits { Multiplier = multiplier });
            return View();
        }

        private void ListDepartments(int? selectedDepartment = null)
        {
            var departmentsQuery = departmentRepository.GetDepartments();
            ViewBag.DepartmentId = new SelectList(departmentsQuery, "Id", "Name", selectedDepartment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                courseRepository.Dispose();
                departmentRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}