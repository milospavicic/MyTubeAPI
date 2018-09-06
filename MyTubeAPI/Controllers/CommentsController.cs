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
    public class CommentsController : ApiController
    {
        private readonly CommentsRepository commentsRepo;

        public CommentsController()
        {
            this.commentsRepo = new CommentsRepository(new MyDBContext());
        }
        [Route("api/comments/{commentId}")]
        [HttpGet]
        public HttpResponseMessage GetCommentById(long? commentId)
        {
            if (commentId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var comment = commentsRepo.GetCommentById((long)commentId);
            var commentDTO = CommentDTO.ConvertCommentToDTO(comment);
            return Request.CreateResponse(HttpStatusCode.OK, commentDTO, Configuration.Formatters.JsonFormatter);
        }
        [Route("api/comments/for_video/{videoId}/{orderBy?}")]
        [HttpGet]
        public HttpResponseMessage GetCommentsForVideo(long? videoId, string orderBy = "")
        {
            if (videoId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var comments = commentsRepo.GetAllCommentsForVideo((long)videoId, orderBy);
            var commentsDTO = CommentDTO.ConvertCollectionCommentToDTO(comments);
            return Request.CreateResponse(HttpStatusCode.OK, commentsDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/comments/create")]
        [HttpPost]
        public HttpResponseMessage CreateComment(Comment comment)
        {
            if (comment == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            comment.DatePosted = DateTime.Now;
            commentsRepo.InsertComment(comment);
            var commentDTO = CommentDTO.ConvertCommentToDTO(comment);
            return Request.CreateResponse(HttpStatusCode.OK, commentDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/comments/update/{commentId}")]
        [HttpPut]
        public HttpResponseMessage UpdateComment(Comment comment, long? commentId)
        {
            if (comment == null || commentId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var commentForEdit = commentsRepo.GetCommentById((long)commentId);
            if (commentForEdit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            commentForEdit.CommentText = comment.CommentText;
            commentsRepo.UpdateComment(commentForEdit);

            var commentDTO = CommentDTO.ConvertCommentToDTO(comment);
            return Request.CreateResponse(HttpStatusCode.OK, commentDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/comments/delete/{commentId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteComment(long? commentId)
        {
            if (commentId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var commentForDel = commentsRepo.GetCommentById((long)commentId);
            if (commentForDel == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            commentsRepo.DeleteComment((long)commentId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}