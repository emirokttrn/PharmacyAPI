using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.StoreService.Entities.StoreReviews
{
    public static class StoreReviewConsts
    {
        public const int MinRating = 1;
        public const int MaxRating = 5;
        public const int MaxCommentLength = 1000;
    }

    public class StoreReview : FullAuditedAggregateRoot<Guid>
    {
        public Guid StoreId { get; protected set; }
        public Guid UserId { get; protected set; }
        public int Rating { get; protected set; }
        public string? Comment { get; protected set; }

        protected StoreReview() { }

        public StoreReview(Guid id, Guid storeId, Guid userId, int rating, string? comment) : base(id)
        {
            StoreId = storeId;
            UserId = userId;
            SetRating(rating);
            SetComment(comment);
        }

        public void SetRating(int rating)
        {
            if (rating < StoreReviewConsts.MinRating || rating > StoreReviewConsts.MaxRating)
            {
                throw new UserFriendlyException("Puan 1 ile 5 arasinda olmali.");
            }

            Rating = rating;
        }

        public void SetComment(string? comment)
        {
            Comment = Check.Length(comment, nameof(Comment), StoreReviewConsts.MaxCommentLength);
        }
    }
}
