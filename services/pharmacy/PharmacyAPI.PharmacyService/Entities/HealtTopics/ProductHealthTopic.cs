using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PharmacyAPI.HealtTopics
{
    public class ProductHealthTopic: Entity
    {
         public const int TopicMaxLength = 64;

        public virtual Guid ProductId { get; protected set; }
        public virtual string Topic { get; protected set; }

        protected ProductHealthTopic() { }

        public ProductHealthTopic(Guid productId, string topic)
        {
            ProductId = productId;
            SetTopic(topic);
        }

        public void SetTopic(string topic)
        {
            Topic = Check.NotNullOrWhiteSpace(topic, nameof(Topic), TopicMaxLength);
        }

        public override object[] GetKeys()
        {
            return new object[] { ProductId, Topic };
        }
    }
}