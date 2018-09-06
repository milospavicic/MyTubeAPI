using MyTubeAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TestProject.Models;

namespace MyTube.Repository
{
    public class VideosRepository : IVideosRepository
    {
        private MyDBContext db;
        private readonly VideoType PUBLIC_VIDEO = VideoType.PUBLIC;

        public VideosRepository(MyDBContext db)
        {
            this.db = db;
        }
        public IEnumerable<Video> GetNRandomVideos(int n)
        {
            var sqlString = String.Format("SELECT TOP {0} V.* FROM Videos AS V INNER JOIN Users AS U ON V.VideoOwner = U.Username " +
                                            " WHERE V.Deleted = 0 AND U.Deleted = 0" +
                                            " ORDER BY NEWID()", n);
            return db.Videos.SqlQuery(sqlString).ToList();
        }
        public IEnumerable<Video> GetNRandomPublicVideos(int n)
        {
            var sqlString = String.Format("SELECT TOP {0} V.* FROM Videos AS V INNER JOIN Users AS U ON V.VideoOwner = U.Username " +
                                            " WHERE V.Deleted = 0 AND U.Deleted = 0 AND V.Blocked = 0 AND U.Blocked = 0 AND V.VideoType = 0 " +
                                            " ORDER BY NEWID()", n);
            return db.Videos.SqlQuery(sqlString).ToList();
        }
        public IEnumerable<Video> GetVideosAll()
        {
            var sqlString = String.Format("SELECT V.* FROM Videos AS V INNER JOIN Users AS U ON V.VideoOwner = U.Username " +
                                " WHERE V.Deleted = 0 AND U.Deleted = 0");
            return db.Videos.SqlQuery(sqlString).ToList();
        }

        public IEnumerable<Video> GetVideosPublic()
        {
            var sqlString = String.Format("SELECT V.* FROM Videos AS V INNER JOIN Users AS U ON V.VideoOwner = U.Username " +
                    " WHERE V.Deleted = 0 AND U.Deleted = 0 AND V.Blocked = 0 AND U.Blocked = 0 AND V.VideoType = 0 ");
            return db.Videos.SqlQuery(sqlString).ToList();
        }

        public IEnumerable<Video> GetVideosAllSearchAndSort(string searchString, string orderBy)
        {
            try
            {
                var searchParam = String.Format("'%{0}%'", searchString);
                var sqlString = String.Format("SELECT * FROM Videos WHERE Deleted = 0 AND ( VideoName LIKE {0} OR VideoDescription LIKE {0} OR VideoOwner LIKE {0} ) {1}", searchParam, VideosSortString(orderBy));
                return db.Videos.SqlQuery(sqlString).ToList();
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Video> GetVideosPublicSearchAndSort(string searchString, string orderBy)
        {
            try
            {
                var searchParam = String.Format("'%{0}%'", searchString);
                var sqlString = String.Format("SELECT * FROM Videos WHERE Deleted = 0 AND VideoType = {1} AND ( VideoName LIKE {0} OR VideoDescription LIKE {0} OR VideoOwner LIKE {0} ) {2}", searchParam, (int)PUBLIC_VIDEO, VideosSortString(orderBy));
                return db.Videos.SqlQuery(sqlString).ToList();
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Video> GetVideosAllOwnedByUserAndSort(string username, string orderBy)
        {
            try
            {
                var sqlString = String.Format("SELECT * FROM Videos WHERE VideoOwner = '{0}' AND Deleted = 0 {1}", username, VideosSortString(orderBy));
                return db.Videos.SqlQuery(sqlString).ToList();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Video> GetVideosPublicOwnedByUserAndSort(string username, string orderBy)
        {
            try
            {
                var sqlString = String.Format("SELECT * FROM Videos WHERE VideoOwner = '{0}' AND Deleted = 0 AND VideoType = {1} {2}", username, (int)PUBLIC_VIDEO, VideosSortString(orderBy));
                return db.Videos.SqlQuery(sqlString).ToList();
            }
            catch
            {
                return null;
            }
        }
        public string VideosSortString(string sortBy)
        {
            switch (sortBy)
            {
                case "latest":
                    return "ORDER BY DatePosted DESC";
                case "oldest":
                    return "ORDER BY DatePosted ASC";
                case "most_viewed":
                    return "ORDER BY ViewsCount DESC";
                case "least_viewed":
                    return "ORDER BY ViewsCount ASC";

                default:
                    return "ORDER BY DatePosted DESC";
            }
        }
        public IEnumerable<Video> GetVideosAllLikedByUser(string username)
        {
            var usernameParam = String.Format("'{0}'", username);
            var sqlString = String.Format("SELECT V.* FROM Videos AS V INNER JOIN Users AS U ON V.VideoOwner = U.Username INNER JOIN VideoRatings AS VR ON V.VideoID = VR.VideoID " +
                                            " WHERE V.Deleted = 0 AND VR.Deleted = 0 AND U.Deleted = 0 AND VR.LikeOwner = {0}", usernameParam);
            return db.Videos.SqlQuery(sqlString).ToList();
        }

        public IEnumerable<Video> GetVideosPublicLikedByUser(string username)
        {
            var usernameParam = String.Format("'{0}'", username);
            var sqlString = String.Format("SELECT V.* FROM Videos AS V INNER JOIN Users AS U ON V.VideoOwner = U.Username INNER JOIN VideoRatings AS VR ON V.VideoID = VR.VideoID " +
                                            " WHERE V.Deleted = 0 AND VR.Deleted = 0 AND U.Deleted = 0 AND V.Blocked = 0 AND U.Blocked = 0 AND V.VideoType = 0 AND VR.LikeOwner = {0}", usernameParam);
            return db.Videos.SqlQuery(sqlString).ToList();
        }

        public Video GetVideoById(long? id)
        {
            try
            {
                return db.Videos.Single(x => x.VideoID == id && x.Deleted == false);
            }
            catch
            {
                return null;
            }
        }

        public void InsertVideo(Video video)
        {
            if (video != null)
            {
                db.Videos.Add(video);
                db.SaveChanges();
            }
        }

        public void UpdateVideo(Video video)
        {
            if (video != null)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void BlockVideo(long? id)
        {
            Video video = GetVideoById(id);
            if (video != null)
            {
                video.Blocked = true;
                db.SaveChanges();
            }
        }
        public void UnblockVideo(long? id)
        {
            Video video = GetVideoById(id);
            if (video != null)
            {
                video.Blocked = false;
                db.SaveChanges();
            }
        }

        public void DeleteVideo(long? id)
        {
            Video video = GetVideoById(id);
            if (video != null)
            {
                video.Deleted = true;
                db.SaveChanges();
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}