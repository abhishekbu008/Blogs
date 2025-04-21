using System.Diagnostics;
using Blogs.Models;
using Blogs.Services;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;

// Links
// https://www.contentful.com/developers/docs/concepts/apis/
// https://www.contentful.com/developers/docs/net/tutorials/using-net-cda-sdk/
// https://github.com/contentful/contentful.net
// https://www.contentful.com/developers/docs/net/tutorials/


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

                // Uncomment when running it for the first time

                // Create Blog and Author content type 
                //List<Field> blogFields = GetBlogFields();
                //await _contentfulService.CreateOrUpdateContentType("blogPost", "Blog Post", "", blogFields, "title");

                //List<Field> authorFields = GetAuthorFields();
                //await _contentfulService.CreateOrUpdateContentType("author", "Author", "", authorFields, "name");

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

        private List<Field> GetAuthorFields() => [
                new Field
                {
                    Id = "name",
                    Name = "Name",
                    Type = SystemFieldTypes.Symbol,
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations = []
                },
                new Field
                {
                    Id = "profilePicture",
                    Name = "Profile Picture",
                    Type = SystemFieldTypes.Link,
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations = [],
                    LinkType = SystemLinkTypes.Asset
                },
                new Field
                {
                    Id = "bio",
                    Name = "Bio",
                    Type = SystemFieldTypes.Text,
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations = []
                },
            ];

        private List<Field> GetBlogFields() => [
                new Field
                {
                    Id = "title",
                    Name = "Title",
                    Type = "Symbol",
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations = []
                },
                new Field
                {
                    Id = "slug",
                    Name = "Slug",
                    Type = "Symbol",
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations = []
                },
                new Field
                {
                    Id = "featuredImage",
                    Name = "Featured Image",
                    Type = "Link",
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations = [],
                    LinkType = "Asset"
                },
                new Field
                {
                    Id = "author",
                    Name = "Author",
                    Type = "Link",
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations = [],
                    LinkType = "Entry"
                },
                new Field
                {
                    Id = "publishedDate",
                    Name = "Published Date",
                    Type = "Date",
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations = [],
                },
                new Field
                {
                    Id = "content",
                    Name = "Content",
                    Type = "RichText",
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations =
                    [
                        new EnabledMarksValidator(
                            enabledMarks:
                            [
                                EnabledMarkRestrictions.bold,
                                EnabledMarkRestrictions.italic,
                                EnabledMarkRestrictions.underline,
                                EnabledMarkRestrictions.code,
                                EnabledMarkRestrictions.superscript,
                                EnabledMarkRestrictions.subscript,
                                EnabledMarkRestrictions.strikethrough
                            ],
                            message: "Only bold, italic, underline, code, superscript, subscript, and strikethrough marks are allowed"
                        ),
                        new EnabledNodeTypesValidator(
                            enabledNodeTypes:
                            [
                                "heading-1",
                                "heading-2",
                                "heading-3",
                                "heading-4",
                                "heading-5",
                                "heading-6",
                                "ordered-list",
                                "unordered-list",
                                "hr",
                                "blockquote",
                                "embedded-asset-block",
                                "table",
                                "asset-hyperlink",
                                "hyperlink"
                            ],
                            message: "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to asset, inline entry, link to entry, and link to Url nodes are allowed"
                        )
                    ],
                },
                new Field
                {
                    Id = "tags",
                    Name = "Tags",
                    Type = "Symbol",
                    Localized = false,
                    Required = false,
                    Disabled = false,
                    Omitted = false,
                    Validations = [],
                }
            ];
    }
}
