using MyTubeAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace TestProject.Models
{
    public enum UserType { USER, ADMIN };
    public class User
    {

        [Key]
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
        [Display(Name = "Type")]
        public UserType UserType { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Description")]
        public string UserDescription { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime RegistrationDate { get; set; }
        public bool Blocked { get; set; }
        public bool Deleted { get; set; }
        [Display(Name = "Picture Url")]
        public string ProfilePictureUrl { get; set; }
        public long SubscribersCount { get; set; }
        public string RegistrationDateString { get { return RegistrationDate.ToShortDateString(); } }

        public virtual ICollection<CommentRating> CommentRatings { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<VideoRating> VideoRatings { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        [ForeignKey("ChannelSubscribedUsername")]
        public virtual ICollection<Subscriber> Subscribers { get; set; }
        [ForeignKey("SubscriberUsername")]
        public virtual ICollection<Subscriber> ThisUserSubscribedTo { get; set; }


        public static SelectList usersSortOrderSelectList = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Selected = true, Text = "Username Asc", Value = "username_asc"},
            new SelectListItem { Selected = true, Text = "Username Desc", Value = "username_desc"},
            new SelectListItem { Selected = true, Text = "Firstname Asc", Value = "firstname_asc"},
            new SelectListItem { Selected = true, Text = "Firstname Desc", Value = "firstname_desc"},
            new SelectListItem { Selected = true, Text = "Lastname Asc", Value = "lastname_asc"},
            new SelectListItem { Selected = true, Text = "Lastname Desc", Value = "lastname_desc"},
            new SelectListItem { Selected = true, Text = "Email Asc", Value = "email_asc"},
            new SelectListItem { Selected = true, Text = "Email Desc", Value = "email_desc"},
            new SelectListItem { Selected = true, Text = "User Type Asc", Value = "user_type_asc"},
            new SelectListItem { Selected = true, Text = "User Type Desc", Value = "user_type_desc"},
            new SelectListItem { Selected = true, Text = "Status Asc", Value = "status_asc"},
            new SelectListItem { Selected = true, Text = "Status Desc", Value = "status_desc"},
        }, "Value", "Text", 1);
        public static SelectList UsersSortOrderSelectList() { return usersSortOrderSelectList; }

        public User UpdateUserFromEditUserModel(EditUserModel eum)
        {
            this.Firstname = eum.Firstname;
            this.Lastname = eum.Lastname;
            this.Email = eum.Email;
            this.UserType = eum.UserType;
            this.UserDescription = eum.UserDescription;
            return this;
        }
    }
}