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
            //_repo.SetupDB();
            var model = new TemperatureViewModel()
                {
                    Batches = _repo.Get10MostRecentsBatches(1).OrderBy(b => b.BatchDate).ToList() //1 = temperature 
                };
            
            return View(model);
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}
