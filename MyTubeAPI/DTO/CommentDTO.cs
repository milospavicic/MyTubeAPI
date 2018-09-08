using MyTube.Repository;
using MyTubeAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestProject.Models;

namespace MyTube.DTO
{
    public class CommentDTO
    {
        public long CommentID { get; set; }
        public long VideoID { get; set; }
        public string CommentOwner { get; set; }
        [Required]
        public string CommentText { get; set; }
        public System.DateTime DatePosted { get; set; }
        public long LikesCount { get; set; }
        public long DislikesCount { get; set; }
        public bool Deleted { get; set; }
        public UserDTO CommentOwnerDTO { get; set; }

        public string DatePostedString { get { return DatePosted.ToShortDateString(); } }

        public static CommentDTO ConvertCommentToDTO(Comment comment)
        {
            CommentDTO newCDTO = new CommentDTO();
            newCDTO.CommentID = comment.CommentID;
            newCDTO.VideoID = comment.VideoID;
            newCDTO.CommentOwner = comment.CommentOwner;
            newCDTO.CommentText = comment.CommentText;
            newCDTO.DatePosted = comment.DatePosted;
            newCDTO.LikesCount = comment.LikesCount;
            newCDTO.DislikesCount = comment.DislikesCount;
            newCDTO.Deleted = comment.Deleted;
            using (var userRepo = new UsersRepository(new MyDBContext()))
            {
                User user = userRepo.GetUserByUsername(comment.CommentOwner);
                newCDTO.CommentOwnerDTO = UserDTO.ConvertUserToDTO(user);
            }

            return newCDTO;
        }
        public static IEnumerable<CommentDTO> ConvertCollectionCommentToDTO(IEnumerable<Comment> comments)
        {
            List<CommentDTO> listDTO = new List<CommentDTO>();
            foreach (var item in comments)
            {
                listDTO.Add(ConvertCommentToDTO(item));
            }
            IEnumerable<CommentDTO> iListDTO = listDTO;
            return iListDTO;
        }

    }
}