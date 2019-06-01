using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GitHubMvc.Areas.Search.Models
{
    public class SearchModel
    {
        [DisplayName("Project Name:")]
        [Required(ErrorMessage = "Please enter a Project Name.")]
        public string Name { get; set; }
    }
}