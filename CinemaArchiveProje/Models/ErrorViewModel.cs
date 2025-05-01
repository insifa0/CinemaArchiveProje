using Microsoft.AspNetCore.Mvc;
using CinemaArchiveProje.Models;
using System.Diagnostics;

namespace CinemaArchiveProje.Controllers
{
    public class ErrorController : Controller
    {
        // Genel hata sayfas�
        public IActionResult Index()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // 404 sayfas�
        public new IActionResult NotFound() // 'new' anahtar kelimesi eklendi
        {
            return View();
        }
    }
}
