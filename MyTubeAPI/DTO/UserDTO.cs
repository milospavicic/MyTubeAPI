using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestProject.Models;

namespace MyTube.DTO
{
    public class UserDTO
    {

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Pass { get; set; }

        [Required]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Description")]
        public string UserDescription { get; set; }

        public System.DateTime RegistrationDate { get; set; }
        public bool Blocked { get; set; }
        public bool Deleted { get; set; }

        [Display(Name = "Picture Url")]
        public string ProfilePictureUrl { get; set; }

        public string RegistrationDateString { get { return RegistrationDate.ToShortDateString(); } }

        public long SubscribersCount { get; set; }

        public long VideosCount { get; set; }

        public string UserTypeName { get; set; }

        public static UserDTO ConvertUserToDTO(User user)
        {
            UserDTO newUDTO = new UserDTO();
            newUDTO.Username = user.Username;
            newUDTO.Pass = user.Pass;
            newUDTO.Firstname = user.Firstname;
            newUDTO.Lastname = user.Lastname;
            newUDTO.UserTypeName = user.UserType.ToString();
            newUDTO.Email = user.Email;
            newUDTO.UserDescription = user.UserDescription;
            newUDTO.RegistrationDate = user.RegistrationDate;
            newUDTO.Blocked = user.Blocked;
            newUDTO.Deleted = user.Deleted;
            newUDTO.ProfilePictureUrl = user.ProfilePictureUrl;
            newUDTO.SubscribersCount = user.SubscribersCount;
            newUDTO.VideosCount = user.VideosCount;

            return newUDTO;
        }
        public static IEnumerable<UserDTO> ConvertCollectionUserToDTO(IEnumerable<User> users)
        {
            List<UserDTO> listDTO = new List<UserDTO>();
            foreach (var item in users)
            {
                listDTO.Add(ConvertUserToDTO(item));
            }
            IEnumerable<UserDTO> iListDTO = listDTO;
            return iListDTO;
        }
    }
}