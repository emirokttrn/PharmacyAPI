using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.Reviews
{
    // [Claude Agent] - Urun yorumu/degerlendirmesi. Bir musteri bir urune birden fazla yorum
    // yapamaz kurali ReviewManager'da uygulanir; Product.Rating/ReviewCount buradan yeniden hesaplanir.
    public static class ReviewConsts
    {
        public const int CommentMaxLength = 1000;
    }

    public class Review : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProductId { get; protected set; }
        public Guid CustomerId { get; protected set; }
        public int Rating { get; protected set; }
        public string? Comment { get; protected set; }

        protected Review() { }

        internal Review(Guid id, Guid productId, Guid customerId, int rating, string? comment) : base(id)
        {
            ProductId = productId;
            CustomerId = customerId;
            SetRating(rating);
            SetComment(comment);
        }

        public void SetRating(int rating)
        {
            if (rating < 1 || rating > 5)
            {
                throw new UserFriendlyException("puan 1 ile 5 arasinda olmali!");
            }
            Rating = rating;
        }

        public void SetComment(string? comment)
        {
            Comment = Check.Length(comment, nameof(Comment), ReviewConsts.CommentMaxLength);
        }
    }
}
