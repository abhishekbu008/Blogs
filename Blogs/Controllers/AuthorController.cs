using Blogs.Models;
using Blogs.Services;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogs.Controllers
{
    public class AuthorController : Controller
    {

        private readonly IContentfulService _contentfulService;

        public AuthorController(IContentfulService contentfulService)
        {
            _contentfulService = contentfulService;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _contentfulService.GetAllAuthorsAsync();
            return View(authors);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var assets = await _contentfulService.GetAllAssetsAsync();

            var authorViewModel = new AuthorViewModel
            {
                ProfilePictures = assets.Select(a => new SelectListItem
                {
                    Text = a.Title.FirstOrDefault().Value,
                    Value = a.SystemProperties.Id
                }).ToList()
            };

            return View(authorViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorViewModel viewModel)
        {
            viewModel.Author.ProfilePicture = new Asset
            {
                SystemProperties = new SystemProperties { Id = viewModel.SelectedProfileId }
            };

            var id = _contentfulService.CreateAuthorAsync(viewModel.Author);
            return RedirectToAction("Index");
        }
    }
}
