
using Blogs.Models;
using Contentful.Core;
using Contentful.Core.Search;
using Newtonsoft.Json;

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
    }
}
