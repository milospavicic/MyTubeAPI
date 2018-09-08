using MyTube.Repository;
using MyTubeAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestProject.Models;

namespace MyTube.DTO
{
    public class VideoDTO
    {
        public VideoDTO()
        {

        }

        public long VideoID { get; set; }

        [Required]
        [Display(Name = "Video Url")]
        public string VideoUrl { get; set; }

        [Display(Name = "Thumbnail Url")]
        public string ThumbnailUrl { get; set; }

        [Required]
        [Display(Name = "Video Name")]
        public string VideoName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string VideoDescription { get; set; }

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

        public string DatePostedString { get; set; }

        public string VideoOwner { get; set; }

        public UserDTO VideoOwnerDTO { get; set; }

        public string VideoTypeName { get; set; }


        public static VideoDTO ConvertVideoToDTO(Video video)
        {
            VideoDTO newVDTO = new VideoDTO();
            newVDTO.VideoID = video.VideoID;
            newVDTO.VideoUrl = video.VideoUrl;
            newVDTO.ThumbnailUrl = video.ThumbnailUrl;
            newVDTO.VideoName = video.VideoName;
            newVDTO.VideoDescription = video.VideoDescription;
            newVDTO.VideoTypeName = video.VideoType.ToString();
            newVDTO.Blocked = video.Blocked;
            newVDTO.Deleted = video.Deleted;
            newVDTO.CommentsEnabled = video.CommentsEnabled;
            newVDTO.RatingEnabled = video.RatingEnabled;
            newVDTO.LikesCount = video.LikesCount;
            newVDTO.DislikesCount = video.DislikesCount;
            newVDTO.ViewsCount = video.ViewsCount;
            newVDTO.DatePostedString = video.DatePostedString;
            newVDTO.VideoOwner = video.VideoOwner;

            using (var userRepo = new UsersRepository(new MyDBContext()))
            {
                User user = userRepo.GetUserByUsername(video.VideoOwner);
                newVDTO.VideoOwnerDTO = UserDTO.ConvertUserToDTO(user);
            }

            return newVDTO;
        }
        public static IEnumerable<VideoDTO> ConvertCollectionVideoToDTO(IEnumerable<Video> videos)
        {
            List<VideoDTO> listDTO = new List<VideoDTO>();
            foreach (var item in videos)
            {
                listDTO.Add(ConvertVideoToDTO(item));
            }
            IEnumerable<VideoDTO> iListDTO = listDTO;
            return iListDTO;
        }
    }
}