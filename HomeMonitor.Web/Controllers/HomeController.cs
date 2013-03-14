using System.Linq;
using System.Web.Mvc;
using HomeMonitor.Models;

namespace HomeMonitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly MeasurementRepository _repo;
        
        public HomeController()
        {
            _repo = new MeasurementRepository();
        }
        
        public ActionResult Index()
        {
            ViewBag.IsBDSetUp = _repo.IsDatabaseSetUp();
            
            return View();
        }
        
        public ActionResult Monitor()
        {
            
            var model = new TemperatureViewModel()
                {
                    Batches = _repo.Get10MostRecentsBatches(1).OrderBy(b => b.BatchDate).ToList() //1 = temperature 
                };
            
            return View(model);
        }

        public ActionResult Setup()
        {
            var dbSetUp = _repo.IsDatabaseSetUp();
            if (dbSetUp)
            {
                ViewBag.Status = "Already exist";
            }
            else
            {
                _repo.SetupDB();
                ViewBag.Status = "DB set up successfully";
            }
            
            return View();
        }
    }
}
