using System;
using TestProject.Models;

namespace MyTube.Repository
{
    interface ISubscribeRepository : IDisposable
    {
        Subscriber GetSubscription(string channelSubscribedTo, string subscriber);
        bool SubscriptionExists(string channelSubscribedTo, string subscriber);

        void NewSubscription(string channelSubscribedTo, string subscriber);

        void DeleteSubscription(string channelSubscribedTo, string subscriber);
    }
}
