using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    public class CommentRating
    {
        [Key]
        public long LikeID { get; set; }
        [ForeignKey("User")]
        public string LikeOwner { get; set; }
        [ForeignKey("Comment")]
        public long CommentId { get; set; }
        public bool IsLike { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime LikeDate { get; set; }
        public bool Deleted { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }
    }
}