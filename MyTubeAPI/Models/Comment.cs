using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace TestProject.Models
{
    public class Comment
    {
        [Key]
        public long CommentID { get; set; }
        [ForeignKey("Video")]
        public long VideoID { get; set; }
        [ForeignKey("User")]
        public string CommentOwner { get; set; }
        [Required]
        public string CommentText { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime DatePosted { get; set; }
        public long LikesCount { get; set; }
        public long DislikesCount { get; set; }
        public bool Deleted { get; set; }
        public virtual User User { get; set; }
        public virtual Video Video { get; set; }
        public virtual ICollection<CommentRating> CommentRatings { get; set; }

        public string DatePostedString { get { return DatePosted.ToShortDateString(); } }

        private static SelectList commentsSortOrderSelectList = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Selected = true, Text = "Latest", Value = "latest"},
            new SelectListItem { Selected = true, Text = "Oldest", Value = "oldest"},
            new SelectListItem { Selected = true, Text = "Most popular", Value = "most_popular"},
            new SelectListItem { Selected = true, Text = "Least popular", Value = "least_popular"},
        }, "Value", "Text", 1);

        public static SelectList CommentsSortOrderSelectList() { return commentsSortOrderSelectList; }
    }
}