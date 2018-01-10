using System;
using System.Web.Mvc;
using TutsUniversity.Infrastructure.Messaging;
using TutsUniversity.Models;
using TutsUniversity.Models.Commands;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Controllers
{
    public class StudentController : Controller
    {
        private readonly Bus bus = Bus.Instance;
        private readonly IStudentRepository studentRepository = RepositoryFactory.Students;

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var searchOptions = new StudentSearchOptions();

            ConfigureFilter();
            ConfigureSortOrder();
            ConfigurePaging();

            return View(studentRepository.Search(searchOptions));

            void ConfigureFilter()
            {
                searchOptions.NameSearch = searchString;
                if (searchOptions.NameSearch != null)
                    searchOptions.PageNumber = 1;
                else
                    searchOptions.NameSearch = currentFilter;

                ViewBag.CurrentFilter = searchOptions.NameSearch;
            }

            void ConfigureSortOrder()
            {
                ViewBag.CurrentSort = sortOrder;
                searchOptions.SortOptions = !string.IsNullOrEmpty(sortOrder) ? (StudentSortOptions) Enum.Parse(typeof(StudentSortOptions), sortOrder) : StudentSortOptions.NameAscending;

                switch (searchOptions.SortOptions)
                {
                    case StudentSortOptions.NameAscending:
                        ViewBag.NameSortParm = StudentSortOptions.NameDescending.ToString();
                        ViewBag.DateSortParm = StudentSortOptions.DateAscending.ToString();
                        break;
                    case StudentSortOptions.NameDescending:
                        ViewBag.NameSortParm = StudentSortOptions.NameAscending.ToString();
                        ViewBag.DateSortParm = StudentSortOptions.DateAscending.ToString();
                        break;
                    case StudentSortOptions.DateAscending:
                        ViewBag.DateSortParm = StudentSortOptions.DateDescending.ToString();
                        ViewBag.NameSortParm = StudentSortOptions.NameAscending.ToString();
                        break;
                    case StudentSortOptions.DateDescending:
                        ViewBag.DateSortParm = StudentSortOptions.DateAscending.ToString();
                        ViewBag.NameSortParm = StudentSortOptions.NameAscending.ToString();
                        break;
                }
            }

            void ConfigurePaging()
            {
                if (page.HasValue)
                    searchOptions.PageNumber = page.Value;
            }
        }

        public ActionResult Details(int id)
        {
            var student = studentRepository.GetStudent(id);
            return View(student);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName, FirstMidName, EnrollmentDate")]Student student)
        {
            if (ModelState.IsValid)
            {
                studentRepository.Add(student);
                bus.Send(new ScheduleOrientation { StudentId = student.Id, FullName = student.FullName });
                return RedirectToAction("Index");
            }

            return View(student);
        }

        public ActionResult Edit(int id)
        {
            var student = studentRepository.GetStudent(id);
            return View(student);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id, string lastName, string firstMidName, DateTime enrollmentDate)
        {
            if (ModelState.IsValid)
            {
                studentRepository.Update(id, lastName, firstMidName, enrollmentDate);
                return RedirectToAction("Index");
            }

            return View(new Student{ EnrollmentDate = enrollmentDate, FirstMidName = firstMidName, Id = id, LastName = lastName});
        }

        public ActionResult Delete(int id)
        {
            var student = studentRepository.GetStudent(id);
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            studentRepository.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                studentRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}