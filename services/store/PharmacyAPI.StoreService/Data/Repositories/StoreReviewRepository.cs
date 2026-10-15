



using Microsoft.EntityFrameworkCore;
using PharmacyAPI.StoreService.Data;
using PharmacyAPI.StoreService.Entities.StoreReviews;
using PharmacyAPI.StoreService.Entities.Stores;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

public class StoreReviewRepository : EfCoreRepository<StoreServiceDbContext, StoreReview, Guid>, IStoreReviewRepository
{
    public StoreReviewRepository(IDbContextProvider<StoreServiceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<StoreReview> FindByStoreAndUserAsync(Guid storeId, Guid userId, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(id=>id.StoreId==storeId && id.UserId==userId, GetCancellationToken(cancellationToken));
    }
}