using MyTube.Repository;
using MyTubeAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestProject.Models;

namespace MyTubeAPI.Controllers
{
    public class VideoRatingsController : ApiController
    {
        private readonly VideoRatingRepository videoRatingRepo;
        private readonly VideosRepository videosRepo;

        public VideoRatingsController()
        {
            this.videoRatingRepo = new VideoRatingRepository(new MyDBContext());
            this.videosRepo = new VideosRepository(new MyDBContext());
        }
        [Route("api/video_rating/{videoId}/{username}")]
        [HttpGet]
        public HttpResponseMessage GetVideoRating(long videoId, string username)
        {
            string returnMessage = "none";
            var vr = videoRatingRepo.GetVideoRating(videoId, username);
            if (vr != null)
            {
                returnMessage = vr.IsLike.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, returnMessage, Configuration.Formatters.JsonFormatter);
        }
        [Route("api/video_rating/create")]
        [HttpPost]
        public HttpResponseMessage CreateVideoRating(VideoRating newVR)
        {
            VideoRating vr = videoRatingRepo.GetVideoRating(newVR.VideoID, newVR.LikeOwner);
            if (vr != null)
            {
                return AlterExistingVideoRating(vr, newVR.IsLike);
            }
            else
            {
                return CreateNewVideoRating(newVR);
            }
        }
        public HttpResponseMessage AlterExistingVideoRating(VideoRating vr, bool newRating)
        {
            string returnMessage = "";
            Video video = videosRepo.GetVideoById(vr.VideoID);
            //true = like, false = dislike
            if (vr.IsLike)
            {
                if (newRating)
                {
                    video.LikesCount -= 1;
                    returnMessage = "neutral";
                }
                else
                {
                    video.LikesCount -= 1;
                    video.DislikesCount += 1;
                    returnMessage = "dislike";
                }
            }
            else
            {
                if (newRating)
                {
                    video.LikesCount += 1;
                    video.DislikesCount -= 1;
                    returnMessage = "like";
                }
                else
                {
                    video.DislikesCount -= 1;
                    returnMessage = "neutral";
                }
            }

            videosRepo.UpdateVideo(video);

            vr.IsLike = newRating;
            if (returnMessage == "neutral")
            {
                videoRatingRepo.DeleteVideoRating(vr.LikeID);
            }
            else
            {
                videoRatingRepo.UpdateVideoRating(vr);
            }
            var returnData = new { returnMessage, video.LikesCount, video.DislikesCount };
            return Request.CreateResponse(HttpStatusCode.OK, returnData, Configuration.Formatters.JsonFormatter);

        }

        public HttpResponseMessage CreateNewVideoRating(VideoRating vr)
        {
            vr.LikeDate = DateTime.Now;
            videoRatingRepo.CreateVideoRating(vr);
            Video video = videosRepo.GetVideoById(vr.VideoID);
            if (vr.IsLike)
            {
                video.LikesCount += 1;
            }
            else
            {
                video.DislikesCount += 1;
            }
            videosRepo.UpdateVideo(video);

            string returnMessage = (vr.IsLike == true) ? "like" : "dislike";

            var returnData = new { returnMessage, video.LikesCount, video.DislikesCount };
            return Request.CreateResponse(HttpStatusCode.OK, returnMessage, Configuration.Formatters.JsonFormatter);
        }
    }
}