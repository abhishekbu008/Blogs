using Blogs.Models;
using Contentful.Core.Configuration;

namespace Blogs.Resolvers
{
    public class EntityResolver : IContentTypeResolver
    {
        public Dictionary<string, Type> _types = new Dictionary<string, Type>
        {
            { "blogPost", typeof(BlogPost) },
            { "author", typeof (Author) },
        };

        public Type Resolve(string contentTypeId)
        {
            return _types.TryGetValue(contentTypeId, out var type) ? type : null;
        }
    }
}
