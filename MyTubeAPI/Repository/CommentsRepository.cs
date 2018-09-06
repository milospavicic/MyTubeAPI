using MyTubeAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TestProject.Models;

namespace MyTube.Repository
{
    public class CommentsRepository : ICommentsRepository
    {
        private MyDBContext db;

        // GET: Comments
        public CommentsRepository(MyDBContext db)
        {
            this.db = db;
        }

        public IEnumerable<Comment> GetAllCommentsForVideo(long id, string orderBy)
        {
            try
            {
                var sqlString = String.Format("SELECT * FROM Comments WHERE VideoId = '{0}' AND Deleted = 0 {1}", id, SortCommentsString(orderBy));
                return db.Comments.SqlQuery(sqlString).ToList();
            }
            catch
            {
                return null;
            }

        }
        public string SortCommentsString(string sortOrder)
        {
            switch (sortOrder)
            {
                case "latest":
                    return "ORDER BY DatePosted DESC";
                case "oldest":
                    return "ORDER BY DatePosted ASC";
                case "most_popular":
                    return "ORDER BY LikesCount DESC, DislikesCount ASC ";
                case "least_popular":
                    return "ORDER BY DislikesCount DESC, LikesCount ASC";
                default:
                    return "ORDER BY DatePosted DESC";
            }
        }
        public Comment GetCommentById(long id)
        {
            try
            {
                return db.Comments.Single(x => x.CommentID == id && x.Deleted == false);
            }
            catch
            {
                return null;
            }
        }
        public void InsertComment(Comment comment)
        {
            if (comment != null)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
        }
        public void UpdateComment(Comment comment)
        {
            if (comment != null)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteComment(long id)
        {
            Comment comment = GetCommentById(id);
            if (comment != null)
            {
                comment.Deleted = true;
                db.SaveChanges();
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}