using Blogs.Models;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;

namespace Blogs.Services
{
    public interface IContentfulService
    {
        Task<List<Author>> GetAllAuthorsAsync();

        Task<List<BlogPost>> GetAllBlogPostsAsync();

        Task CreateSpaceAsync(string spaceId);

        Task DeleteSpaceAsync(string spaceId);

        Task UpdateSpaceAsync(string spaceId, string newSpaceName);

        Task<string> CreateBlogPostAsync(BlogPost post);

        Task<List<ManagementAsset>> GetAllAssetsAsync();

        Task CreateOrUpdateContentType(string Id, string name, string desc, List<Field> fields, string displayField);

    }
}
