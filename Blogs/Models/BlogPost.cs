using Contentful.Core.Models;

namespace Blogs.Models
{
    public class BlogPost : IEntity
    {
        public SystemProperties Sys { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public DateTime PublishedDate { get; set; }
        public Document Content { get; set; }
        public Author Author { get; set; }
        public Asset FeaturedImage { get; set; }
    }
}
