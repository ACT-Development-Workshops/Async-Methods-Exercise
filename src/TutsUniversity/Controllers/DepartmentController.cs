using System;
using System.Web.Mvc;
using TutsUniversity.Infrastructure.Data;
using TutsUniversity.Models;
using TutsUniversity.Models.Repositories;
using TutsUniversity.Models.Repositories.Providers;

namespace TutsUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository repository = new DepartmentRepository();
        private TutsUniversityContext db = new TutsUniversityContext();

        public ActionResult Index()
        {
            var departments = repository.GetDepartments();
            return View(departments);
        }

        public ActionResult Details(int id)
        {
            var department = repository.GetDepartment(id);
            return View(department);
        }

        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                repository.Add(department);
                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        public ActionResult Edit(int id)
        {
            var department = repository.GetDepartment(id);
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string name, decimal budget, DateTime startDate, int instructorId, byte[] rowVersion)
        {
            if (ModelState.IsValid)
            {
                repository.Update(id, name, budget, startDate, instructorId, rowVersion);
                return RedirectToAction("Index");
            }
            
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", instructorId);
            return View(new Department { Budget = budget, DepartmentID = id, InstructorID = instructorId, Name = name, RowVersion = rowVersion, StartDate = startDate });
        }

        public ActionResult Delete(int id)
        {
            var department = repository.GetDepartment(id);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Department department)
        {
            repository.Delete(department.DepartmentID);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                repository.Dispose();

            base.Dispose(disposing);
        }
    }
}