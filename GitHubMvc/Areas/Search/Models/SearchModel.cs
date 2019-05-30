using System.ComponentModel.DataAnnotations;

namespace GitHubMvc.Areas.Search.Models
{
    public class SearchModel
    {
        [Required]
        public string Name { get; set; }
    }
}