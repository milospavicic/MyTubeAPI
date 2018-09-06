using System;
using TestProject.Models;

namespace MyTube.Repository
{
    interface IVideoRatingRepository : IDisposable
    {
        bool RatingExistsForVideo(long videoId, string username);

        VideoRating GetVideoRating(long videoId, string username);

        void UpdateVideoRating(VideoRating vr);

        void CreateVideoRating(VideoRating vr);

        void DeleteVideoRating(long ratingId);
    }
}
