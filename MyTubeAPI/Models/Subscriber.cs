using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    public class Subscriber
    {
        [Key]
        public long SubID { get; set; }
        [ForeignKey("ChannelSubscribed")]
        public string ChannelSubscribedUsername { get; set; }
        [ForeignKey("TheSubscriber")]
        public string SubscriberUsername { get; set; }
        public virtual User ChannelSubscribed { get; set; }
        public virtual User TheSubscriber { get; set; }
    }
}