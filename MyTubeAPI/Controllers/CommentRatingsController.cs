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
    public class CommentRatingsController : ApiController
    {
        private readonly CommentRatingsRepository commentRatingsRepository;
        private readonly CommentsRepository commentsRepo;

        public CommentRatingsController()
        {
            this.commentRatingsRepository = new CommentRatingsRepository(new MyDBContext());
            this.commentsRepo = new CommentsRepository(new MyDBContext());
        }

        [Route("api/comment_rating/{commentId}/{username}")]
        [HttpGet]
        public HttpResponseMessage RatingForOneComment(long? commentId, string username)
        {
            string returnMessage = "none";
            var vr = commentRatingsRepository.GetCommentRating((long)commentId, username);
            if (vr != null)
            {
                returnMessage = vr.IsLike.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, returnMessage, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/comment_rating/for_video/{videoId}/{username}")]
        [HttpGet]
        public HttpResponseMessage CommentRatingsForVideo(long? videoId, string username)
        {
            if (username != null && videoId != null)
            {
                var crs = commentRatingsRepository.GetCommentRatingsForVideo((long)videoId, username);
                var crsDTO = CommentRatingsDTO.ConvertCollectionCommentToDTO(crs);
                return Request.CreateResponse(HttpStatusCode.OK, crsDTO, Configuration.Formatters.JsonFormatter);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Route("api/comment_rating/create")]
        [HttpPost]
        public HttpResponseMessage CreateCommentRating(CommentRating newCR)
        {
            CommentRating cr = commentRatingsRepository.GetCommentRating(newCR.CommentId, newCR.LikeOwner);
            if (cr != null)
            {
                return AlterExistingCommentRating(cr, newCR.IsLike);
            }
            else
            {
                return CreateNewCommentRating(newCR);
            }
        }
        public HttpResponseMessage AlterExistingCommentRating(CommentRating cr, bool newRating)
        {
            string returnMessage = "";
            Comment comment = commentsRepo.GetCommentById((long)cr.CommentId);
            //true = like, false = dislike
            if (cr.IsLike)
            {
                if (newRating)
                {
                    comment.LikesCount -= 1;
                    returnMessage = "neutral";
                }
                else
                {
                    comment.LikesCount -= 1;
                    comment.DislikesCount += 1;
                    returnMessage = "dislike";
                }
            }
            else
            {
                if (newRating)
                {
                    comment.LikesCount += 1;
                    comment.DislikesCount -= 1;
                    returnMessage = "like";
                }
                else
                {
                    comment.DislikesCount -= 1;
                    returnMessage = "neutral";
                }
            }

            commentsRepo.UpdateComment(comment);

            cr.IsLike = newRating;
            if (returnMessage == "neutral")
            {
                commentRatingsRepository.DeleteCommentRating(cr.LikeID);
            }
            else
            {
                commentRatingsRepository.UpdateCommentRating(cr);
            }
            var returnData = new { returnMessage, comment.LikesCount, comment.DislikesCount };
            return Request.CreateResponse(HttpStatusCode.OK, returnData, Configuration.Formatters.JsonFormatter);

        }

        public HttpResponseMessage CreateNewCommentRating(CommentRating cr)
        {
            cr.LikeDate = DateTime.Now;
            commentRatingsRepository.CreateCommentRating(cr);
            Comment comment = commentsRepo.GetCommentById((long)cr.CommentId);
            if (cr.IsLike)
            {
                comment.LikesCount += 1;
            }
            else
            {
                comment.DislikesCount += 1;
            }
            commentsRepo.UpdateComment(comment);

            string returnMessage = (cr.IsLike == true) ? "like" : "dislike";
            var returnData = new { returnMessage, comment.LikesCount, comment.DislikesCount };
            return Request.CreateResponse(HttpStatusCode.OK, returnData, Configuration.Formatters.JsonFormatter);
        }
    }
}