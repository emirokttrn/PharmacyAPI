

using PharmacyAPI.StoreService.Entities.Stores;
using Volo.Abp.Domain.Repositories;

public interface IStoreRepository : IRepository<Store,Guid>
{
    Task<List<Store>> GetActivateListBoundingBoxAsync(double minLatitude, double maxLatitude, double minLongitude, double maxLongitude, CancellationToken cancellationToken=default);
}