namespace MyTubeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentRatings",
                c => new
                    {
                        LikeID = c.Long(nullable: false, identity: true),
                        LikeOwner = c.String(maxLength: 128),
                        CommentId = c.Long(nullable: false),
                        IsLike = c.Boolean(nullable: false),
                        LikeDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LikeID)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.LikeOwner)
                .Index(t => t.LikeOwner)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Long(nullable: false, identity: true),
                        VideoID = c.Long(nullable: false),
                        CommentOwner = c.String(maxLength: 128),
                        CommentText = c.String(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        LikesCount = c.Long(nullable: false),
                        DislikesCount = c.Long(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Users", t => t.CommentOwner)
                .ForeignKey("dbo.Videos", t => t.VideoID, cascadeDelete: true)
                .Index(t => t.VideoID)
                .Index(t => t.CommentOwner);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Pass = c.String(nullable: false),
                        Firstname = c.String(nullable: false),
                        Lastname = c.String(nullable: false),
                        UserType = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        UserDescription = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        Blocked = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ProfilePictureUrl = c.String(),
                        SubscribersCount = c.Long(nullable: false),
                        VideosCount = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.Subscribers",
                c => new
                    {
                        SubID = c.Long(nullable: false, identity: true),
                        ChannelSubscribedUsername = c.String(maxLength: 128),
                        SubscriberUsername = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SubID)
                .ForeignKey("dbo.Users", t => t.ChannelSubscribedUsername)
                .ForeignKey("dbo.Users", t => t.SubscriberUsername)
                .Index(t => t.ChannelSubscribedUsername)
                .Index(t => t.SubscriberUsername);
            
            CreateTable(
                "dbo.VideoRatings",
                c => new
                    {
                        LikeID = c.Long(nullable: false, identity: true),
                        VideoID = c.Long(nullable: false),
                        LikeOwner = c.String(maxLength: 128),
                        IsLike = c.Boolean(nullable: false),
                        LikeDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LikeID)
                .ForeignKey("dbo.Users", t => t.LikeOwner)
                .ForeignKey("dbo.Videos", t => t.VideoID, cascadeDelete: true)
                .Index(t => t.VideoID)
                .Index(t => t.LikeOwner);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        VideoID = c.Long(nullable: false, identity: true),
                        VideoUrl = c.String(nullable: false),
                        ThumbnailUrl = c.String(),
                        VideoName = c.String(nullable: false),
                        VideoDescription = c.String(nullable: false),
                        VideoType = c.Int(nullable: false),
                        Blocked = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CommentsEnabled = c.Boolean(nullable: false),
                        RatingEnabled = c.Boolean(nullable: false),
                        LikesCount = c.Long(nullable: false),
                        DislikesCount = c.Long(nullable: false),
                        ViewsCount = c.Long(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        VideoOwner = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VideoID)
                .ForeignKey("dbo.Users", t => t.VideoOwner)
                .Index(t => t.VideoOwner);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentRatings", "LikeOwner", "dbo.Users");
            DropForeignKey("dbo.CommentRatings", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.Comments", "VideoID", "dbo.Videos");
            DropForeignKey("dbo.Comments", "CommentOwner", "dbo.Users");
            DropForeignKey("dbo.VideoRatings", "VideoID", "dbo.Videos");
            DropForeignKey("dbo.Videos", "VideoOwner", "dbo.Users");
            DropForeignKey("dbo.VideoRatings", "LikeOwner", "dbo.Users");
            DropForeignKey("dbo.Subscribers", "SubscriberUsername", "dbo.Users");
            DropForeignKey("dbo.Subscribers", "ChannelSubscribedUsername", "dbo.Users");
            DropIndex("dbo.Videos", new[] { "VideoOwner" });
            DropIndex("dbo.VideoRatings", new[] { "LikeOwner" });
            DropIndex("dbo.VideoRatings", new[] { "VideoID" });
            DropIndex("dbo.Subscribers", new[] { "SubscriberUsername" });
            DropIndex("dbo.Subscribers", new[] { "ChannelSubscribedUsername" });
            DropIndex("dbo.Comments", new[] { "CommentOwner" });
            DropIndex("dbo.Comments", new[] { "VideoID" });
            DropIndex("dbo.CommentRatings", new[] { "CommentId" });
            DropIndex("dbo.CommentRatings", new[] { "LikeOwner" });
            DropTable("dbo.Videos");
            DropTable("dbo.VideoRatings");
            DropTable("dbo.Subscribers");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.CommentRatings");
        }
    }
}
