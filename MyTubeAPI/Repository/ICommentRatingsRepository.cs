﻿using System;
using System.Collections.Generic;
using TestProject.Models;

namespace MyTube.Repository
{
    interface ICommentRatingsRepository : IDisposable
    {
        bool RatingExistsForComment(long commentId, string username);

        CommentRating GetCommentRatingById(long id);

        CommentRating GetCommentRating(long commentId, string username);

        IEnumerable<CommentRating> GetCommentRatingsForVideo(long videoId, string username);

        void UpdateCommentRating(CommentRating cr);

        void CreateCommentRating(CommentRating cr);

        void DeleteCommentRating(long ratingId);
    }
}
