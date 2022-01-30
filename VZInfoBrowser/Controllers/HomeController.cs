using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VZInfoBrowser.ApplicationCore.Model;
using VZInfoBrowser.Infrastructure;
using VZInfoBrowser.Models;

namespace VZInfoBrowser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISettings<CurrencyRatesInfo> _data;

        public HomeController(ISettings<CurrencyRatesInfo> data) 
        {
            _data = data;
        }

        public IActionResult Index()
        {
            return View(_data.Data);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
