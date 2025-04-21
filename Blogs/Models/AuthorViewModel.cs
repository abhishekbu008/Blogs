using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogs.Models
{
    public class AuthorViewModel
    {
        public Author Author { get; set; }

        public List<SelectListItem> ProfilePictures { get; set; }
        public string SelectedProfileId { get; set; }
    }
}
