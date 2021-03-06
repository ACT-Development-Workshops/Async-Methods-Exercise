﻿using System;
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
            return View(departmentRepository.GetDepartments());
        }

        public ActionResult Details(int id)
        {
            return View(departmentRepository.GetDepartment(id));
        }

        public ActionResult Create()
        {
            ListInstructors();
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

            ListInstructors(department.InstructorId);
            return View(department);
        }

        public ActionResult Edit(int id)
        {
            var department = departmentRepository.GetDepartment(id);
            ListInstructors(department.InstructorId);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string name, decimal budget, DateTime startDate, int? instructorId, byte[] rowVersion)
        {
            departmentRepository.Update(id, name, budget, startDate, instructorId, rowVersion);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            return View(departmentRepository.GetDepartment(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Department department)
        {
            departmentRepository.Delete(department.Id);
            return RedirectToAction("Index");
        }

        private void ListInstructors(int? currentInstructorId = null)
        {
            ViewBag.InstructorId = new SelectList(instructorRepository.GetInstructors(), "Id", "FullName", currentInstructorId);
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