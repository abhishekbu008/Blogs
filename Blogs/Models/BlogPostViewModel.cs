using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogs.Models
{
    public class BlogPostViewModel
    {
        public BlogPost BlogPost { get; set; }
        public List<SelectListItem> Authors { get; set; }
        public string SelectedAuthorId { get; set; }
        public List<SelectListItem> Assets { get; set; }
        public string SelectedAssetId { get; set; }
        public string BlogPostContent { get; set; }
    }
}
