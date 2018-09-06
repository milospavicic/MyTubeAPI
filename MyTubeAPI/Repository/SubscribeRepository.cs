using MyTubeAPI.Models;
using System.Linq;
using TestProject.Models;

namespace MyTube.Repository
{
    public class SubscribeRepository : ISubscribeRepository
    {
        private MyDBContext db;

        public SubscribeRepository(MyDBContext db)
        {
            this.db = db;
        }
        public Subscriber GetSubscription(string channelSubscribedTo, string subscriber)
        {
            try
            {
                return db.Subscribers.Single(x => x.ChannelSubscribedUsername == channelSubscribedTo && x.SubscriberUsername == subscriber);
            }
            catch
            {
                return null;
            }
        }
        public bool SubscriptionExists(string channelSubscribedTo, string subscriber)
        {
            return db.Subscribers.Any(u => u.ChannelSubscribedUsername == channelSubscribedTo && u.SubscriberUsername == subscriber);
        }

        public void NewSubscription(string channelSubscribedTo, string subscriber)
        {
            Subscriber newSub = new Subscriber
            {
                ChannelSubscribedUsername = channelSubscribedTo,
                SubscriberUsername = subscriber
            };
            db.Subscribers.Add(newSub);

            db.SaveChanges();
        }

        public void DeleteSubscription(string channelSubscribedTo, string subscriber)
        {
            Subscriber sub = GetSubscription(channelSubscribedTo, subscriber);
            if (sub != null)
            {
                db.Subscribers.Remove(sub);
                db.SaveChanges();
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}