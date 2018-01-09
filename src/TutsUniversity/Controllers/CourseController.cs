using System.Linq;
using System.Web.Mvc;
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
        private readonly ICourseRepository courseRepository = new CourseRepository();
        private readonly IDepartmentRepository departmentRepository = new DepartmentRepository();

        public ActionResult Index(int? departmentId)
        {
            var departments = departmentRepository.GetDepartments();
            ViewBag.SelectedDepartment = new SelectList(departments, "DepartmentID", "Name", departmentId);

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
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")]Course course)
        {
            if (ModelState.IsValid)
            {
                courseRepository.Add(course);
                return RedirectToAction("Index");
            }

            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        public ActionResult Edit(int id)
        {
            var course = courseRepository.GetCourse(id);
            
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id, string title, int credits, int departmentID)
        {
            if (ModelState.IsValid)
            {
                courseRepository.Update(id, title, credits, departmentID);
                return RedirectToAction("Index");
            }

            PopulateDepartmentsDropDownList(departmentID);
            return View(new Course { CourseID = id, Credits = credits, DepartmentID = departmentID, Title = title });
        }

        private void PopulateDepartmentsDropDownList(int? selectedDepartment = null)
        {
            var departmentsQuery = departmentRepository.GetDepartments();
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
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