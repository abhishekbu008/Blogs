using Blogs.Services;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Microsoft.AspNetCore.Mvc;

// Links
// https://www.contentful.com/developers/docs/concepts/apis/
// https://www.contentful.com/developers/docs/net/tutorials/using-net-cda-sdk/
// https://github.com/contentful/contentful.net
// https://www.contentful.com/developers/docs/net/tutorials/


namespace Blogs.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContentfulService _contentfulService;

        public HomeController(IContentfulService contentfulService)
        {
            _contentfulService = contentfulService;
        }

        public async Task<IActionResult> Index()
        {
            // Create Blog and Author content type 

            List<Field> blogFields = GetBlogFields();
            await _contentfulService.CreateOrUpdateContentType("blogPost", "Blog Post", "", blogFields, "title");

            List<Field> authorFields = GetAuthorFields();
            await _contentfulService.CreateOrUpdateContentType("author", "Author", "", authorFields, "name");

            return View();
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
