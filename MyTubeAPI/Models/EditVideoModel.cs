using System.ComponentModel.DataAnnotations;
using TestProject.Models;

namespace MyTubeAPI.Models
{
    public class EditVideoModel
    {
        public long VideoID { get; set; }

        [Required]
        [Display(Name = "Video Name")]
        public string VideoName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string VideoDescription { get; set; }

        [Required]
        [Display(Name = "Type")]
        public VideoType VideoType { get; set; }

        [Required]
        [Display(Name = "Comments Enabled")]
        public bool CommentsEnabled { get; set; }

        [Required]
        [Display(Name = "Rating Enabled")]
        public bool RatingEnabled { get; set; }
        [Required]
        [Url]
        [Display(Name = "Thumbnail Url")]
        public string ThumbnailUrl { get; set; }

        public static EditVideoModel EditVideoModalFromVideo(Video video)
        {
            EditVideoModel videoForEdit = new EditVideoModel
            {
                VideoID = video.VideoID,
                VideoName = video.VideoName,
                VideoDescription = video.VideoDescription,
                VideoType = video.VideoType,
                CommentsEnabled = video.CommentsEnabled,
                RatingEnabled = video.RatingEnabled,
                ThumbnailUrl = video.ThumbnailUrl
            };
            return videoForEdit;
        }
    }
}