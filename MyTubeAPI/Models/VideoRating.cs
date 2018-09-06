using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    public class VideoRating
    {
        [Key]
        public long LikeID { get; set; }
        [ForeignKey("Video")]
        public long VideoID { get; set; }
        [ForeignKey("User")]
        public string LikeOwner { get; set; }
        public bool IsLike { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime LikeDate { get; set; }
        public bool Deleted { get; set; }
        public virtual User User { get; set; }
        public virtual Video Video { get; set; }
    }
}