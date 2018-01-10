using System.Web.Mvc;
using TutsUniversity.Models.Repositories;

namespace TutsUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository studentRepository = RepositoryFactory.Students;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View(studentRepository.GetDailyEnrollmentTotals());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                studentRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}