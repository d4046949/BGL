using System.Collections.Generic;

namespace GitHubMvc.Areas.Search.Models
{
    public class ProjectViewModel
    {
        public string Name { get; set; }
        public int Rating { get; set; }
    }

    public class UserProfileViewModel
    {
        public string FullName { get; set; }
        public string Location { get; set; }
        public string AvatarPath { get; set; }
        public List<ProjectViewModel> Projects { get; set; }
    }
}