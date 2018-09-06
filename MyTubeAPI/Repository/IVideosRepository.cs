using System;
using System.Collections.Generic;
using TestProject.Models;

namespace MyTube.Repository
{
    interface IVideosRepository : IDisposable
    {
        IEnumerable<Video> GetNRandomVideos(int n);
        IEnumerable<Video> GetNRandomPublicVideos(int n);
        IEnumerable<Video> GetVideosAll();
        IEnumerable<Video> GetVideosPublic();
        IEnumerable<Video> GetVideosAllOwnedByUserAndSort(string username, string orderBy);
        IEnumerable<Video> GetVideosPublicOwnedByUserAndSort(string username, string orderBy);
        IEnumerable<Video> GetVideosAllLikedByUser(string username);
        IEnumerable<Video> GetVideosPublicLikedByUser(string username);
        IEnumerable<Video> GetVideosAllSearchAndSort(string searchString, string orderBy);
        IEnumerable<Video> GetVideosPublicSearchAndSort(string searchString, string orderBy);
        Video GetVideoById(long? id);
        void InsertVideo(Video video);
        void UpdateVideo(Video video);
        void UnblockVideo(long? id);
        void BlockVideo(long? id);
        void DeleteVideo(long? id);

    }
}
