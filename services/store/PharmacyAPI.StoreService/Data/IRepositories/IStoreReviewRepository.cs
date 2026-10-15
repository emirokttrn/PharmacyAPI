using PharmacyAPI.StoreService.Entities.StoreReviews;
using Volo.Abp.Domain.Repositories;

public interface IStoreReviewRepository : IRepository<StoreReview, Guid>
{
    Task<StoreReview> FindByStoreAndUserAsync(Guid storeId, Guid userId, CancellationToken cancellationToken=default);
}