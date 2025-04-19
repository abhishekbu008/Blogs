using Blogs.Models;

namespace Blogs.Services
{
    public interface IContentfulService
    {
        Task<List<Author>> GetAllAuthorsAsync();

        Task<List<BlogPost>> GetAllBlogPostsAsync();
    }
}
