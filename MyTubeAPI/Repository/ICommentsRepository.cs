using System;
using System.Collections.Generic;
using TestProject.Models;

namespace MyTube.Repository
{
    interface ICommentsRepository : IDisposable
    {
        IEnumerable<Comment> GetAllCommentsForVideo(long id, string orderBy);
        Comment GetCommentById(long id);
        void InsertComment(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(long id);

    }
}
