using MyTubeAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace TestProject.Models
{
    public enum VideoType { PUBLIC, PRIVATE, UNLISTED };
    public class Video
    {
        [Key]
        public long VideoID { get; set; }
        [Required]
        [Url]
        [Display(Name = "Video Url")]
        public string VideoUrl { get; set; }
        [Url]
        [Display(Name = "Thumbnail Url")]
        public string ThumbnailUrl { get; set; }
        [Required]
        [Display(Name = "Video Name")]
        public string VideoName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string VideoDescription { get; set; }
        [Required]
        [Display(Name = "Type")]
        public VideoType VideoType { get; set; }
        public bool Blocked { get; set; }
        public bool Deleted { get; set; }
        [Required]
        [Display(Name = "Comments Enabled")]
        public bool CommentsEnabled { get; set; }
        [Required]
        [Display(Name = "Rating Enabled")]
        public bool RatingEnabled { get; set; }
        public long LikesCount { get; set; }
        public long DislikesCount { get; set; }
        public long ViewsCount { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime DatePosted { get; set; }
        [ForeignKey("User")]
        public string VideoOwner { get; set; }
        public string DatePostedString { get { return DatePosted.ToShortDateString(); } }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<VideoRating> VideoRatings { get; set; }


        private static SelectList videosSortOrderSelectList = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Selected = true, Text = "Latest", Value = "latest"},
            new SelectListItem { Selected = true, Text = "Oldest", Value = "oldest"},
            new SelectListItem { Selected = true, Text = "Most Viewed", Value = "most_viewed"},
            new SelectListItem { Selected = true, Text = "Least Viewed", Value = "least_viewed"},
        }, "Value", "Text", 1);

        public static SelectList VideosSortOrderSelectList() { return videosSortOrderSelectList; }

        public Video UpdateVideoFromEVM(EditVideoModel evm)
        {
            this.VideoName = evm.VideoName;
            this.VideoDescription = evm.VideoDescription;
            this.VideoType = evm.VideoType;
            this.CommentsEnabled = evm.CommentsEnabled;
            this.RatingEnabled = evm.RatingEnabled;
            return this;
        }
    }
}