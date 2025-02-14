using BulkyBook.DependencyInjection.Models;
using BulkyBook.DependencyInjection.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace BulkyBook.DependencyInjection.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ITransientGuidService _transientGuidService1;
        private readonly ITransientGuidService _transientGuidService2;
        private readonly IScopedGuidService _scopedGuidService1;
        private readonly IScopedGuidService _scopedGuidService2;
        private readonly ISingletonGuidService _singletonGuidService1;
        private readonly ISingletonGuidService _singletonGuidService2;

        public HomeController(ITransientGuidService transientGuidService1, ITransientGuidService transientGuidService2,
            IScopedGuidService scopedGuidService1, IScopedGuidService scopedGuidService2,
            ISingletonGuidService singletonGuidService1, ISingletonGuidService singletonGuidService2)
        {
            _transientGuidService1 = transientGuidService1;
            _transientGuidService2 = transientGuidService2;
            _scopedGuidService1 = scopedGuidService1;
            _scopedGuidService2 = scopedGuidService2;
            _singletonGuidService1 = singletonGuidService1;
            _singletonGuidService2 = singletonGuidService2;
        }

        public IActionResult Index()
        {
            StringBuilder builder=new StringBuilder();
            builder.Append($"Transient1: {_transientGuidService1.GetGuid()} \n");
            builder.Append($"Transient2: {_transientGuidService2.GetGuid()} \n\n");

            builder.Append($"Scoped1: {_scopedGuidService1.GetGuid()} \n");
            builder.Append($"Scoped2: {_scopedGuidService2.GetGuid()} \n\n");

            builder.Append($"Singleton1: {_singletonGuidService1.GetGuid()} \n");
            builder.Append($"Singleton2: {_singletonGuidService2.GetGuid()} \n\n");

            return Ok(builder.ToString());
        }

        public IActionResult Privacy()
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
