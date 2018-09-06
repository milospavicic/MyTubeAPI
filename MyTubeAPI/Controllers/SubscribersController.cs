using MyTube.Repository;
using MyTubeAPI.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyTubeAPI.Controllers
{
    public class SubscribersController : ApiController
    {
        private readonly SubscribeRepository subsRepo;

        public SubscribersController()
        {
            this.subsRepo = new SubscribeRepository(new MyDBContext());
        }
        [Route("api/subscribers/check_if_sub/{subscriber}/{channelSubscribed}")]
        [HttpGet]
        public HttpResponseMessage ChechIfSubbed(string subscriber, string channelSubscribed)
        {
            if (subscriber == null || channelSubscribed == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            bool exists = subsRepo.SubscriptionExists(channelSubscribed, subscriber);
            return Request.CreateResponse(HttpStatusCode.OK, exists, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/subscribers/sub/{subscriber}/{channelSubscribed}")]
        [HttpGet]
        public HttpResponseMessage Subscribe(string subscriber, string channelSubscribed)
        {
            if (subscriber == null || channelSubscribed == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            bool exists = subsRepo.SubscriptionExists(channelSubscribed, subscriber);
            if (exists)
            {
                subsRepo.DeleteSubscription(channelSubscribed, subscriber);

            }
            else
            {
                subsRepo.NewSubscription(channelSubscribed, subscriber);

            }
            bool finishStatus = subsRepo.SubscriptionExists(channelSubscribed, subscriber);
            return Request.CreateResponse(HttpStatusCode.OK, finishStatus, Configuration.Formatters.JsonFormatter);
        }
    }
}
