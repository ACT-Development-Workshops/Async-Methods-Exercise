using System.Threading.Tasks;
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

        public async Task<ActionResult> About()
        {
            return View(await studentRepository.GetDailyEnrollmentTotals());
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