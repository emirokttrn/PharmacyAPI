using PharmacyAPI.StoreService.Entities.StoreProducts;
using Volo.Abp.Domain.Repositories;

public interface IStoreProductRepository :IRepository<StoreProduct , Guid>
{
    Task<StoreProduct> FindByStoreAndProductAsync(Guid storeId, Guid productId, CancellationToken cancellationToken=default);
}