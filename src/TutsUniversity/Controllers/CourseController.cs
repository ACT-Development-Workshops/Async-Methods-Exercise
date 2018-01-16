using System.Threading.Tasks;
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

        public async Task<ActionResult> Index([Bind(Prefix = "SelectedDepartment")]int? selectedDepartmentId)
        {
            var departments = await departmentRepository.GetDepartments();
            ViewBag.SelectedDepartment = new SelectList(departments, "Id", "Name", selectedDepartmentId);

            return View(await courseRepository.GetCourses(selectedDepartmentId));
        }

        public async Task<ActionResult> Details(int id)
        {
            return View(await courseRepository.GetCourse(id));
        }

        public async Task<ActionResult> Create()
        {
            await ListDepartments();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Credits,DepartmentId")]Course course)
        {
            if (ModelState.IsValid)
            {
                await courseRepository.Add(course);
                return RedirectToAction("Index");
            }

            await ListDepartments(course.DepartmentId);
            return View(course);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var course = await courseRepository.GetCourse(id);
            await ListDepartments(course.DepartmentId);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int id, string title, int credits, int departmentId)
        {
            await courseRepository.Update(id, title, credits, departmentId);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            return View(await courseRepository.GetCourse(id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await courseRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCourseCredits()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCourseCredits(int multiplier)
        {
            await bus.Send(new MultiplyCourseCredits { Multiplier = multiplier });
            return RedirectToAction("Index");
        }

        private async Task ListDepartments(int? selectedDepartment = null)
        {
            var departmentsQuery = await departmentRepository.GetDepartments();
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