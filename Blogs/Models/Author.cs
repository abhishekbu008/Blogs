using Contentful.Core.Models;

namespace Blogs.Models
{
    public class Author : IEntity
    {
        public SystemProperties Sys { get; set; }

        public string Name { get; set; }
        public Asset ProfilePicture { get; set; }
        public string Bio { get; set; }
    }
}
