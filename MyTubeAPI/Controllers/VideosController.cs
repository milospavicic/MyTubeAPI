using MyTube.DTO;
using MyTube.Repository;
using MyTubeAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestProject.Models;

namespace MyTubeAPI.Controllers
{
    public class VideosController : ApiController
    {
        private VideosRepository videosRepo;
        private readonly string BASIC_PICTURE = "https://blog.stylingandroid.com/wp-content/themes/lontano-pro/images/no-image-slide.png";

        public VideosController()
        {
            this.videosRepo = new VideosRepository(new MyDBContext());
        }

        [Route("api/videos/all")]
        [HttpGet]
        public HttpResponseMessage AllVideos()
        {
            var videos = videosRepo.GetVideosAll();
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/public")]
        [HttpGet]
        public HttpResponseMessage AllPublicVideos()
        {
            var videos = videosRepo.GetVideosPublic();
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/{id}")]
        [HttpGet]
        public HttpResponseMessage GetVideoById(long? id)
        {
            var video = videosRepo.GetVideoById(id);
            var videoDTO = VideoDTO.ConvertVideoToDTO(video);
            return Request.CreateResponse(HttpStatusCode.OK, videoDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/index")]
        [HttpGet]
        public HttpResponseMessage IndexSixRandomVideos()
        {
            var videos = videosRepo.GetNRandomVideos(6);
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/indexpublic")]
        [HttpGet]
        public HttpResponseMessage IndexSixPublicRandomVideos()
        {
            var videos = videosRepo.GetNRandomPublicVideos(6);
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/search/{parameter}/{orderBy?}")]
        [HttpGet]
        public HttpResponseMessage Search(string parameter, string orderBy = "")
        {
            if (parameter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videos = videosRepo.GetVideosAllSearchAndSort(parameter, orderBy);
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/search_public/{parameter}/{orderBy?}")]
        [HttpGet]
        public HttpResponseMessage SearchPublic(string parameter, string orderBy = "")
        {
            if (parameter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videos = videosRepo.GetVideosPublicSearchAndSort(parameter, orderBy);
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/by_owner_and_sort/{username}/{orderBy?}")]
        [HttpGet]
        public HttpResponseMessage AllVideosOwnedBy(string username, string orderBy = "")
        {
            if (username == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videos = videosRepo.GetVideosAllOwnedByUserAndSort(username, orderBy);
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/by_owner_public_and_sort/{username}/{orderBy?}")]
        [HttpGet]
        public HttpResponseMessage PublicVideosOwnedBy(string username, string orderBy = "")
        {
            if (username == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videos = videosRepo.GetVideosPublicOwnedByUserAndSort(username, orderBy);
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/liked_by_user_all/{username}")]
        [HttpGet]
        public HttpResponseMessage AllVideosLikedBy(string username)
        {
            if (username == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videos = videosRepo.GetVideosAllLikedByUser(username);
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/liked_by_user_public/{username}")]
        [HttpGet]
        public HttpResponseMessage PublicVideosLikedBy(string username)
        {
            if (username == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videos = videosRepo.GetVideosPublicLikedByUser(username);
            var videosDTO = VideoDTO.ConvertCollectionVideoToDTO(videos);
            return Request.CreateResponse(HttpStatusCode.OK, videosDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/create")]
        [HttpPost]
        public HttpResponseMessage CreateVideo(Video video)
        {
            if (video == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            video.DatePosted = DateTime.Now;
            if (video.ThumbnailUrl == null)
            {
                video.ThumbnailUrl = BASIC_PICTURE;
            }
            videosRepo.InsertVideo(video);
            using (var userRepo = new UsersRepository(new MyDBContext()))
            {
                User user = userRepo.GetUserByUsername(video.VideoOwner);
                user.VideosCount += 1;
                userRepo.UpdateUser(user);
            }
            var videoDTO = VideoDTO.ConvertVideoToDTO(video);
            return Request.CreateResponse(HttpStatusCode.Created, videoDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/update/{id}")]
        [HttpGet]
        public HttpResponseMessage GetVideoForUpdate(long? id)
        {
            var videoForEdit = videosRepo.GetVideoById(id);
            if (videoForEdit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            var editVideoModel = EditVideoModel.EditVideoModalFromVideo(videoForEdit);

            return Request.CreateResponse(HttpStatusCode.OK, editVideoModel, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/update/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateVideo(EditVideoModel video, long? id)
        {
            if (video == null || id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videoForEdit = videosRepo.GetVideoById(id);
            if (videoForEdit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            videoForEdit.UpdateVideoFromEVM(video);
            videosRepo.UpdateVideo(videoForEdit);

            var videoDTO = VideoDTO.ConvertVideoToDTO(videoForEdit);

            return Request.CreateResponse(HttpStatusCode.OK, videoDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/videos/unblock/{id}")]
        [HttpPut]
        public HttpResponseMessage UnblockVideo(long? id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videoForEdit = videosRepo.GetVideoById(id);
            if (videoForEdit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            videosRepo.UnblockVideo(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/videos/block/{id}")]
        [HttpPut]
        public HttpResponseMessage BlockVideo(long? id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videoForEdit = videosRepo.GetVideoById(id);
            if (videoForEdit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            videosRepo.BlockVideo(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/videos/delete/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteConfirmed(long? id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var videoForDel = videosRepo.GetVideoById(id);
            if (videoForDel == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            videosRepo.DeleteVideo(id);
            using (var userRepo = new UsersRepository(new MyDBContext()))
            {
                User user = userRepo.GetUserByUsername(videoForDel.VideoOwner);
                user.VideosCount -= 1;
                userRepo.UpdateUser(user);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
