using MyTubeAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TestProject.Models;

namespace MyTube.Repository
{
    public class CommentRatingsRepository : ICommentRatingsRepository
    {
        private MyDBContext db;

        public CommentRatingsRepository(MyDBContext db)
        {
            this.db = db;
        }
        public CommentRating GetCommentRatingById(long id)
        {
            try
            {
                return db.CommentRatings.Single(x => x.LikeID == id && x.Deleted == false);
            }
            catch
            {
                return null;
            }
        }

        public CommentRating GetCommentRating(long commentId, string username)
        {
            try
            {
                return db.CommentRatings.Single(x => x.CommentId == commentId && x.LikeOwner == username && x.Deleted == false);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<CommentRating> GetCommentRatingsForVideo(long videoId, string username)
        {
            var idParam = String.Format("'{0}'", videoId);
            var usernameParam = String.Format("'{0}'", username);
            var queryString = String.Format("SELECT CR.* FROM CommentRatings AS CR INNER JOIN Comments as C ON CR.CommentId = C.CommentID INNER JOIN Videos AS V ON C.VideoID = V.VideoId" +
                                                " WHERE V.Deleted = 0 AND C.Deleted = 0 AND CR.Deleted = 0 AND V.VideoID = {0} AND CR.LikeOwner = {1}", idParam, usernameParam);
            return db.Database.SqlQuery<CommentRating>(queryString).ToList();
        }

        public bool RatingExistsForComment(long commentId, string username)
        {
            return db.CommentRatings.Any(x => x.CommentId == commentId && x.LikeOwner == username && x.Deleted == false);
        }

        public void CreateCommentRating(CommentRating cr)
        {
            if (cr != null)
            {
                db.CommentRatings.Add(cr);
                db.SaveChanges();
            }
        }

        public void UpdateCommentRating(CommentRating cr)
        {
            if (cr != null)
            {
                db.Entry(cr).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void DeleteCommentRating(long ratingId)
        {
            var cr = GetCommentRatingById(ratingId);
            if (cr != null)
            {
                cr.Deleted = true;
                db.SaveChanges();
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}