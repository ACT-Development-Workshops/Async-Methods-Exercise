using System.Linq;
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

            var courses = courseRepository.GetCourses(departmentId);
            return View(courses.ToList());
        }

        public ActionResult Details(int id)
        {
            var course = courseRepository.GetCourse(id);
            return View(course);
        }

        public ActionResult Create()
        {
            PopulateDepartmentsDropDownList();
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

            PopulateDepartmentsDropDownList(course.DepartmentId);
            return View(course);
        }

        public ActionResult Edit(int id)
        {
            var course = courseRepository.GetCourse(id);
            
            PopulateDepartmentsDropDownList(course.DepartmentId);
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

            PopulateDepartmentsDropDownList(departmentId);
            return View(new Course { Id = id, Credits = credits, DepartmentId = departmentId, Title = title });
        }

        private void PopulateDepartmentsDropDownList(int? selectedDepartment = null)
        {
            var departmentsQuery = departmentRepository.GetDepartments();
            ViewBag.DepartmentId = new SelectList(departmentsQuery, "Id", "Name", selectedDepartment);
        }

        public ActionResult Delete(int id)
        {
            var course = courseRepository.GetCourse(id);
            return View(course);
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