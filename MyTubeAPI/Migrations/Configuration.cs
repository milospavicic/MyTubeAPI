namespace MyTubeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using TestProject.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MyTubeAPI.Models.MyDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyTubeAPI.Models.MyDBContext context)
        {
            var ProfilePicture = "http://www.personalbrandingblog.com/wp-content/uploads/2017/08/blank-profile-picture-973460_640-300x300.png";

            context.Users.AddOrUpdate(p => p.Username,
                new User() { Username = "123", Pass = "123", Firstname = "Marko", Lastname = "Markovic", UserType = UserType.USER, Email = "marko@gmail.com", UserDescription = null, RegistrationDate = DateTime.Now, Blocked = false, Deleted = false, ProfilePictureUrl = ProfilePicture, SubscribersCount = 1 },
                new User() { Username = "1234", Pass = "123", Firstname = "Pera", Lastname = "Peric", UserType = UserType.ADMIN, Email = "pera@gmail.com", UserDescription = null, RegistrationDate = DateTime.Now, Blocked = false, Deleted = false, ProfilePictureUrl = ProfilePicture, SubscribersCount = 0 }
                );
            context.SaveChanges();

            context.Subscribers.AddOrUpdate(p => p.ChannelSubscribedUsername,
                new Subscriber() { ChannelSubscribedUsername = "123", SubscriberUsername = "1234" });
            context.SaveChanges();

            context.Videos.AddOrUpdate(p => p.VideoID,
            new Video() { VideoID = 1, VideoUrl = "https://www.youtube.com/embed/Q0CbN8sfihY", ThumbnailUrl = "https://s.aolcdn.com/hss/storage/midas/8c786b6e2ab90b7d527621886ee9ff4d/205751517/sw-tlj-ed.jpg", VideoName = "Star Wars: The Last Jedi Trailer", VideoDescription = "Watch the new trailer for Star Wars: The Last Jedi and see it in theaters December 15.", VideoType = VideoType.PUBLIC, VideoOwner = "123", DatePosted = DateTime.Now, CommentsEnabled = true, RatingEnabled = true },
            new Video() { VideoID = 2, VideoUrl = "https://www.youtube.com/embed/qxjPjPzQ1iU", ThumbnailUrl = "https://i.ytimg.com/vi/h5XQq1ulspc/maxresdefault.jpg", VideoName = "War for the Planet of the Apes", VideoDescription = "War for the Planet of the Apes','Directed By Matt Reeves Cast: Andy Serkis, Woody Harrelson, Steve Zahn, Amiah Miller, Karin Konoval, Judy Greer and Terry Notary", VideoType = VideoType.PUBLIC, VideoOwner = "123", DatePosted = DateTime.Now, CommentsEnabled = true, RatingEnabled = true },
            new Video() { VideoID = 3, VideoUrl = "https://www.youtube.com/embed/5OVY7MmSSYs", ThumbnailUrl = "http://demofest.org/wp-content/uploads/2017/03/Bad-Copy-4-2.jpg", VideoName = "bad copy - esi mi dobar", VideoDescription = "\"I kad mi pridje neki smarac ja mu kazem odma, esi mi Boban, e, esi mi Boban..\"", VideoType = VideoType.PRIVATE, VideoOwner = "123", DatePosted = DateTime.Now, CommentsEnabled = true, RatingEnabled = true });
            context.SaveChanges();

            context.VideoRatings.AddOrUpdate(p => p.LikeID,
            new VideoRating() { LikeID = 1, VideoID = 1, LikeOwner = "123", IsLike = true, LikeDate = DateTime.Now },
            new VideoRating() { LikeID = 2, VideoID = 1, LikeOwner = "1234", IsLike = false, LikeDate = DateTime.Now },
            new VideoRating() { LikeID = 3, VideoID = 2, LikeOwner = "123", IsLike = true, LikeDate = DateTime.Now },
            new VideoRating() { LikeID = 4, VideoID = 2, LikeOwner = "1234", IsLike = false, LikeDate = DateTime.Now });
            context.SaveChanges();

            context.Comments.AddOrUpdate(p => p.CommentID,
            new Comment() { CommentID = 1, VideoID = 1, CommentOwner = "123", CommentText = "WOW", DatePosted = DateTime.Now },
            new Comment() { CommentID = 2, VideoID = 1, CommentOwner = "1234", CommentText = "AWESOME!", DatePosted = DateTime.Now },
            new Comment() { CommentID = 3, VideoID = 1, CommentOwner = "123", CommentText = "LAME", DatePosted = DateTime.Now },
            new Comment() { CommentID = 4, VideoID = 2, CommentOwner = "1234", CommentText = "WOW", DatePosted = DateTime.Now },
            new Comment() { CommentID = 5, VideoID = 2, CommentOwner = "123", CommentText = "AWESOME!", DatePosted = DateTime.Now },
            new Comment() { CommentID = 6, VideoID = 2, CommentOwner = "1234", CommentText = "LAME", DatePosted = DateTime.Now });
            context.SaveChanges();

            context.CommentRatings.AddOrUpdate(p => p.LikeID,
            new CommentRating() { LikeID = 1, CommentId = 1, LikeOwner = "123", IsLike = true, LikeDate = DateTime.Now },
            new CommentRating() { LikeID = 2, CommentId = 1, LikeOwner = "1234", IsLike = false, LikeDate = DateTime.Now },
            new CommentRating() { LikeID = 3, CommentId = 2, LikeOwner = "123", IsLike = true, LikeDate = DateTime.Now },
            new CommentRating() { LikeID = 4, CommentId = 2, LikeOwner = "1234", IsLike = false, LikeDate = DateTime.Now });
            context.SaveChanges();
        }
    }
}
