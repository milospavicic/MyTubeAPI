using System.ComponentModel.DataAnnotations;
using TestProject.Models;

namespace MyTubeAPI.Models
{
    public class EditUserModel
    {
        public string Username { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string Firstname { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string Lastname { get; set; }
        [Display(Name = "User type")]
        public UserType UserType { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Description")]
        public string UserDescription { get; set; }
        public static EditUserModel EditUserModalFromUser(User user)
        {
            EditUserModel userForEdit = new EditUserModel
            {
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                UserType = user.UserType,
                UserDescription = user.UserDescription
            };
            return userForEdit;
        }
    }
}