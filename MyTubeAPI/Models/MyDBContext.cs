using System.Data.Entity;
using TestProject.Models;

namespace MyTubeAPI.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("name=MyDBContext")
        {
        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentRating> CommentRatings { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoRating> VideoRatings { get; set; }
    }
}