using System.Diagnostics;
using Blogs.Models;
using Blogs.Services;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Mvc;

// Links
// https://www.contentful.com/developers/docs/net/tutorials/using-net-cda-sdk/


namespace Blogs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContentfulService _contentfulService;

        public HomeController(ILogger<HomeController> logger, IContentfulService contentfulService)
        {
            _logger = logger;
            _contentfulService = contentfulService;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _contentfulService.GetAllBlogPostsAsync();

            return View(blogs);
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
