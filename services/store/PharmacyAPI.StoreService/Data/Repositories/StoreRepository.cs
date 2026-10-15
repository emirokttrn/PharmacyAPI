


using Microsoft.EntityFrameworkCore;
using PharmacyAPI.StoreService.Data;
using PharmacyAPI.StoreService.Entities.Stores;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

public class StoreRepository : EfCoreRepository<StoreServiceDbContext, Store, Guid>, IStoreRepository
{
    public StoreRepository(IDbContextProvider<StoreServiceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<Store>> GetActivateListBoundingBoxAsync(double minLatitude, double maxLatitude, double minLongitude, double maxLongitude, CancellationToken cancellationToken = default)
    {
        var dbSet= await WithDetailsAsync();
        return await dbSet
        .Where(s=>s.IsActive)
        .Where(s=>s.Latitude>=minLatitude && s.Latitude<=maxLatitude)
        .Where(s=>s.Longitude>= minLongitude && s.Longitude<=maxLongitude)
        .ToListAsync(GetCancellationToken(cancellationToken));

    }
}