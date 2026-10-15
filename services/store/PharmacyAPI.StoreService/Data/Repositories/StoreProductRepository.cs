


using Microsoft.EntityFrameworkCore;
using PharmacyAPI.StoreService.Data;
using PharmacyAPI.StoreService.Entities.StoreProducts;
using PharmacyAPI.StoreService.Entities.Stores;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

public class StoreProductRepository : EfCoreRepository<StoreServiceDbContext, StoreProduct, Guid>, IStoreProductRepository
{
    public StoreProductRepository(IDbContextProvider<StoreServiceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
   public async Task<StoreProduct> FindByStoreAndProductAsync(Guid storeId, Guid productId, CancellationToken cancellationToken)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.StoreId == storeId && x.ProductId == productId, GetCancellationToken(cancellationToken));
    }
}