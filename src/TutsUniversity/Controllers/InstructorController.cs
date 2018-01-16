using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TutsUniversity.Models;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ICourseRepository courseRepository = RepositoryFactory.Courses;
        private readonly IInstructorRepository instructorRepository = RepositoryFactory.Instructors;

        public ActionResult Index(int? id, int? courseId)
        {
            var viewModel = new InstructorList();
            viewModel.Instructors = instructorRepository.GetInstructors();

            if (id != null)
            {
                ViewBag.InstructorId = id.Value;
                viewModel.Courses = viewModel.Instructors.Single(i => i.Id == id.Value).Courses;
            }

            if (courseId != null)
            {
                ViewBag.CourseId = courseId.Value;
                viewModel.Enrollments = viewModel.Courses.Single(x => x.Id == courseId).Enrollments;
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
        public ActionResult Edit(int id, string lastName, string firstMidName, DateTime hireDate, OfficeAssignment officeAssignment, string[] selectedCourses)
        {
            instructorRepository.Update(id, lastName, firstMidName, hireDate, officeAssignment.Location, selectedCourses?.Select(int.Parse).ToArray());
            return RedirectToAction("Index");
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
                    CourseId = course.Id,
                    Title = course.Title,
                    Assigned = currentlyAssignedCourses.Select(c => c.Id).Contains(course.Id)
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