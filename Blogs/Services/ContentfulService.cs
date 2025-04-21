using Blogs.Models;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;

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
            await _contentfulManagementClient.UpdateSpaceName(spaceId, newSpaceName, version.Value);
        }

        public async Task CreateOrUpdateContentType(string id, string name, string desc, List<Field> fields, string displayField)
        {
            var contentType = new ContentType();
            contentType.SystemProperties = new SystemProperties();
            contentType.SystemProperties.Id = id;
            contentType.Name = name;
            contentType.Description = desc;
            contentType.Fields = fields;
            contentType.DisplayField = displayField;

            var existingContentType = await _contentfulManagementClient.GetContentType(id);
            var version = existingContentType?.SystemProperties?.Version ?? 1;
            var updatedContentType = await _contentfulManagementClient.CreateOrUpdateContentType(contentType, version: version);
            await _contentfulManagementClient.ActivateContentType(updatedContentType.SystemProperties.Id, updatedContentType.SystemProperties.Version.Value);
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

            await _contentfulManagementClient.PublishEntry(created.SystemProperties.Id, created.SystemProperties.Version.Value);
            return created?.SystemProperties?.Id;
        }

    }
}
