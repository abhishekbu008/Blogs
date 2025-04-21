using Blogs.Models;
using Contentful.Core;
using Contentful.Core.Errors;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Blogs.Services
{
    public class ContentfulService : IContentfulService
    {
        private readonly IContentfulClient _contentfulClient;
        private readonly IContentfulManagementClient _contentfulManagementClient;

        public ContentfulService(IContentfulClient contentfulClient, IContentfulManagementClient contentfulManagementClient)
        {
            _contentfulClient = contentfulClient ?? throw new ArgumentNullException(nameof(contentfulClient));
            _contentfulManagementClient = contentfulManagementClient ?? throw new ArgumentNullException(nameof(contentfulManagementClient));
        }

        public async Task CreateSpaceAsync(string spaceId)
        {
            await _contentfulManagementClient.CreateSpace(spaceId, "en-US");
        }

        public async Task DeleteSpaceAsync(string spaceId)
        {
            await _contentfulManagementClient.DeleteSpace(spaceId);
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            var query = QueryBuilder<Author>.New.ContentTypeIs("author");
            var authors = await _contentfulClient.GetEntries(query);
            return authors.Items.ToList();
        }

        public async Task<List<BlogPost>> GetAllBlogPostsAsync()
        {
            var query = QueryBuilder<BlogPost>.New.ContentTypeIs("blogPost").Include(3);
            var blogs = await _contentfulClient.GetEntries(query);
            return blogs.Items.ToList();
        }

        public async Task UpdateSpaceAsync(string spaceId, string newSpaceName)
        {
            var space = await _contentfulManagementClient.GetSpace(spaceId);
            var version = space.SystemProperties.Version;
            await _contentfulManagementClient.UpdateSpaceName(spaceId, newSpaceName, version ?? 1);
        }

        public async Task CreateOrUpdateContentType(string id, string name, string desc, List<Field> fields, string displayField)
        {
            var contentType = new ContentType
            {
                SystemProperties = new SystemProperties { Id = id },
                Name = name,
                Description = desc,
                Fields = fields,
                DisplayField = displayField
            };

            try
            {
                ContentType existingContentType = null;

                try
                {
                    // Attempt to fetch the existing content type
                    existingContentType = await _contentfulManagementClient.GetContentType(id);
                }
                catch (ContentfulException ex) when (ex.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    // Handle the "resource not found" error
                    Console.WriteLine($"Content type with ID '{id}' not found. Proceeding to create a new content type.");
                }

                var version = existingContentType?.SystemProperties?.Version ?? 1;

                // Create or update the content type
                var updatedContentType = await _contentfulManagementClient.CreateOrUpdateContentType(contentType, version: version);

                // Activate the content type
                if (updatedContentType != null && updatedContentType.SystemProperties.Version.HasValue)
                {
                    await _contentfulManagementClient.ActivateContentType(updatedContentType.SystemProperties.Id, updatedContentType.SystemProperties.Version.Value);
                }
                else
                {
                    throw new Exception("Failed to create or update the content type.");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task<List<ManagementAsset>> GetAllAssetsAsync()
        {
            var publishedAssets = await _contentfulManagementClient.GetPublishedAssetsCollection();
            return publishedAssets.Items.ToList();
        }


        public async Task<string> CreateBlogPostAsync(BlogPost post)
        {
            var entry = new Entry<dynamic>();
            entry.SystemProperties = new SystemProperties
            {
                ContentType = new ContentType { SystemProperties = new SystemProperties { Id = "blogPost" } }
            };
            entry.Fields = new 
            {
                title = new Dictionary<string, string> { { "en-US", post.Title } },
                slug = new Dictionary<string, string> { { "en-US", post.Slug } },
                publishedDate = new Dictionary<string, DateTime> { { "en-US", post.PublishedDate } },
                content = new Dictionary<string, Document> { { "en-US", post.Content } },
                author = new Dictionary<string, Reference>
                {
                    {
                        "en-US",
                        new Reference(linkType: SystemLinkTypes.Entry, id: post.Author.Sys.Id)
                    }
                },
                featuredImage = new Dictionary<string, Reference>
                {
                    {
                        "en-US",
                        new Reference(linkType: SystemLinkTypes.Asset, id: post.FeaturedImage?.SystemProperties?.Id)
                    }
                }
            };

            var created = await _contentfulManagementClient.CreateEntry(entry, contentTypeId: "blogPost");

            await _contentfulManagementClient.PublishEntry(created.SystemProperties.Id, created.SystemProperties.Version ?? 1);
            return created.SystemProperties.Id;
        }

        public async Task<string> CreateAuthorAsync(Author author)
        {
            var entry = new Entry<dynamic>();
            entry.SystemProperties = new SystemProperties
            {
                ContentType = new ContentType { SystemProperties = new SystemProperties { Id = "author" } }
            };

            entry.Fields = new
            {
                Name = new Dictionary<string, string> { { "en-US", author.Name } },
                Bio = new Dictionary<string, string> { { "en-US", author.Bio } },
                ProfilePicture = new Dictionary<string, Reference>
                {
                    {
                        "en-US",
                        new Reference(linkType: SystemLinkTypes.Asset, id: author.ProfilePicture?.SystemProperties?.Id)
                    }
                }
            };

            var created = await _contentfulManagementClient.CreateEntry(entry, contentTypeId: "author");

            await _contentfulManagementClient.PublishEntry(created.SystemProperties.Id, created.SystemProperties.Version ?? 1);
            return created.SystemProperties.Id;
        }
    }
}
