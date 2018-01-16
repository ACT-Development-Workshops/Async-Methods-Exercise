using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TutsUniversity.Models;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository departmentRepository = RepositoryFactory.Departments;
        private readonly IInstructorRepository instructorRepository = RepositoryFactory.Instructors;

        public async Task<ActionResult> Index()
        {
            return View(await departmentRepository.GetDepartments());
        }

        public async Task<ActionResult> Details(int id)
        {
            return View(await departmentRepository.GetDepartment(id));
        }

        public async Task<ActionResult> Create()
        {
            await ListInstructors();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Budget,StartDate,InstructorId")] Department department)
        {
            if (ModelState.IsValid)
            {
                await departmentRepository.Add(department);
                return RedirectToAction("Index");
            }

            await ListInstructors(department.InstructorId);
            return View(department);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var department = await departmentRepository.GetDepartment(id);
            await ListInstructors(department.InstructorId);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, string name, decimal budget, DateTime startDate, int? instructorId, byte[] rowVersion)
        {
            await departmentRepository.Update(id, name, budget, startDate, instructorId, rowVersion);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            return View(await departmentRepository.GetDepartment(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Department department)
        {
            await departmentRepository.Delete(department.Id);
            return RedirectToAction("Index");
        }

        private async Task ListInstructors(int? currentInstructorId = null)
        {
            ViewBag.InstructorId = new SelectList(await instructorRepository.GetInstructors(), "Id", "FullName", currentInstructorId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                departmentRepository.Dispose();
                instructorRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}