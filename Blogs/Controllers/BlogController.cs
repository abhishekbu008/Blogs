using System.Diagnostics;
using Blogs.Models;
using Blogs.Services;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Blogs.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IContentfulService _contentfulService;
        private readonly ContentfulOptions _contentfulOptions;

        public BlogController(ILogger<BlogController> logger, IContentfulService contentfulService, IOptions<ContentfulOptions> contentfulOptions)
        {
            _logger = logger;
            _contentfulService = contentfulService;
            _contentfulOptions = contentfulOptions.Value;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                await _contentfulService.UpdateSpaceAsync(_contentfulOptions.SpaceId, "blogs");

                var blogs = await _contentfulService.GetAllBlogPostsAsync();
                return View(blogs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var authors = await _contentfulService.GetAllAuthorsAsync();
            var assets = await _contentfulService.GetAllAssetsAsync();

            var viewModel = new BlogPostViewModel
            {
                BlogPost = new BlogPost { PublishedDate = DateTime.Now },
                Authors = authors.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Sys.Id
                }).ToList(),

                Assets = assets.Select(a => new SelectListItem
                {
                    Text = a.Title.FirstOrDefault().Value,
                    Value = a.SystemProperties.Id
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogPostViewModel viewModel)
        {
            viewModel.BlogPost.Author = new Author
            {
                Sys = new SystemProperties { Id = viewModel.SelectedAuthorId }
            };

            viewModel.BlogPost.FeaturedImage = new Asset
            {
                SystemProperties = new SystemProperties { Id = viewModel.SelectedAssetId }
            };
            viewModel.BlogPost.Content = ConvertHtmlToDocument(viewModel.BlogPostContent);


            var id = await _contentfulService.CreateBlogPostAsync(viewModel.BlogPost);
            return RedirectToAction("Index");
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


        private Document ConvertHtmlToDocument(string html)
        {
            var doc = new Document
            {
                Content = new List<IContent>
                {
                    new Paragraph
                    {
                        Content = new List<IContent>
                        {
                            new Text
                            {
                                Value = html,
                                Marks = new List<Mark>(),
                                NodeType = "text",
                                Data = new GenericStructureData()
                            },
                        },
                        NodeType = "paragraph",
                        Data = new GenericStructureData()
                    }
                },
                NodeType = "document",
                Data = new GenericStructureData()
            };

            return doc;
        }

    }
}
