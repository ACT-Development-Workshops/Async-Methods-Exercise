using System;
using System.Web.Mvc;
using TutsUniversity.Models;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository departmentRepository = RepositoryFactory.Departments;
        private readonly IInstructorRepository instructorRepository = RepositoryFactory.Instructors;

        public ActionResult Index()
        {
            var departments = departmentRepository.GetDepartments();
            return View(departments);
        }

        public ActionResult Details(int id)
        {
            var department = departmentRepository.GetDepartment(id);
            return View(department);
        }

        public ActionResult Create()
        {
            ViewBag.InstructorId = new SelectList(instructorRepository.GetInstructors(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Budget,StartDate,InstructorId")] Department department)
        {
            if (ModelState.IsValid)
            {
                departmentRepository.Add(department);
                return RedirectToAction("Index");
            }

            ViewBag.InstructorId = new SelectList(instructorRepository.GetInstructors(), "Id", "FullName", department.InstructorId);
            return View(department);
        }

        public ActionResult Edit(int id)
        {
            var department = departmentRepository.GetDepartment(id);
            ViewBag.InstructorId = new SelectList(instructorRepository.GetInstructors(), "Id", "FullName", department.InstructorId);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string name, decimal budget, DateTime startDate, int instructorId, byte[] rowVersion)
        {
            if (ModelState.IsValid)
            {
                departmentRepository.Update(id, name, budget, startDate, instructorId, rowVersion);
                return RedirectToAction("Index");
            }
            
            ViewBag.InstructorId = new SelectList(instructorRepository.GetInstructors(), "Id", "FullName", instructorId);
            return View(new Department { Budget = budget, Id = id, InstructorId = instructorId, Name = name, RowVersion = rowVersion, StartDate = startDate });
        }

        public ActionResult Delete(int id)
        {
            var department = departmentRepository.GetDepartment(id);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Department department)
        {
            departmentRepository.Delete(department.Id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                departmentRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}