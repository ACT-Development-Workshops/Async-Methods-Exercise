using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TutsUniversity.Models;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ICourseRepository courseRepository = RepositoryFactory.Courses;
        private readonly IInstructorRepository instructorRepository = RepositoryFactory.Instructors;

        public async Task<ActionResult> Index(int? id, int? courseId)
        {
            var viewModel = new InstructorList();
            viewModel.Instructors = await instructorRepository.GetInstructors();

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

        public async Task<ActionResult> Details(int id)
        {
            var instructor = await instructorRepository.GetInstructor(id);
            return View(instructor);
        }

        public async Task<ActionResult> Create()
        {
            await ListAssignableCourses(Enumerable.Empty<Course>());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LastName,FirstMidName,HireDate,OfficeAssignment")]Instructor instructor, string[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                await instructorRepository.Add(instructor, selectedCourses?.Select(int.Parse).ToArray() ?? new int[] { });
                return RedirectToAction("Index");
            }

            await ListAssignableCourses(instructor.Courses);
            return View(instructor);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var instructor = await instructorRepository.GetInstructor(id);
            await ListAssignableCourses(instructor.Courses);
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, string lastName, string firstMidName, DateTime hireDate, OfficeAssignment officeAssignment, string[] selectedCourses)
        {
            await instructorRepository.Update(id, lastName, firstMidName, hireDate, officeAssignment.Location, selectedCourses?.Select(int.Parse).ToArray());
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var instructor = await instructorRepository.GetInstructor(id);
            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await instructorRepository.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task ListAssignableCourses(IEnumerable<Course> currentlyAssignedCourses)
        {
            ViewBag.Courses = (await courseRepository
                .GetCourses())
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