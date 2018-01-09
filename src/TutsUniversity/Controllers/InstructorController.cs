﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TutsUniversity.Models;
using TutsUniversity.Models.Repositories;
using TutsUniversity.ViewModels;

namespace TutsUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ICourseRepository courseRepository = RepositoryFactory.Courses;
        private readonly IInstructorRepository instructorRepository = RepositoryFactory.Instructors;

        public ActionResult Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndex();
            viewModel.Instructors = instructorRepository.GetInstructors();

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Single(i => i.ID == id.Value).Courses;
            }

            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                viewModel.Enrollments = viewModel.Courses.Single(x => x.CourseID == courseID).Enrollments;
            }

            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            var instructor = instructorRepository.GetInstructor(id);
            return View(instructor);
        }

        public ActionResult Create()
        {
            ListAssignableCourses(Enumerable.Empty<Course>());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName,HireDate,OfficeAssignment")]Instructor instructor, string[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                if (selectedCourses != null)
                {
                    foreach (var course in selectedCourses)
                        instructor.Courses.Add(courseRepository.GetCourse(int.Parse(course)));
                }

                instructorRepository.Add(instructor);
                return RedirectToAction("Index");
            }

            ListAssignableCourses(instructor.Courses);
            return View(instructor);
        }

        public ActionResult Edit(int id)
        {
            var instructor = instructorRepository.GetInstructor(id);
            ListAssignableCourses(instructor.Courses);
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string lastName, string firstMidName, DateTime hireDate, string location, string[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                instructorRepository.Update(id, lastName, firstMidName, hireDate, location, selectedCourses.Select(int.Parse).ToArray());
                return RedirectToAction("Index");
            }

            var instructor = instructorRepository.GetInstructor(id);
            ListAssignableCourses(instructor.Courses);
            return View(instructor);
        }

        public ActionResult Delete(int id)
        {
            var instructor = instructorRepository.GetInstructor(id);
            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            instructorRepository.Delete(id);
            return RedirectToAction("Index");
        }

        private void ListAssignableCourses(IEnumerable<Course> currentlyAssignedCourses)
        {
            ViewBag.Courses = courseRepository
                .GetCourses()
                .Select(course => new AssignableCourse
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = currentlyAssignedCourses.Select(c => c.CourseID).Contains(course.CourseID)
                })
                .ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                courseRepository.Dispose();
                instructorRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}