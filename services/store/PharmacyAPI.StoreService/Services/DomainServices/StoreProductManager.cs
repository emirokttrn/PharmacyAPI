

using PharmacyAPI.StoreService.Entities.StoreProducts;
using Volo.Abp;
using Volo.Abp.Domain.Services;

public class StoreProductManager : DomainService
{
    private readonly IStoreProductRepository _storeProductRepository;
    private readonly IStoreRepository _storeRepository;

    public StoreProductManager(
       IStoreProductRepository storeProductRepository,
       IStoreRepository storeRepository)
    {
        _storeProductRepository = storeProductRepository;
        _storeRepository = storeRepository;
    }

    public async Task<StoreProduct> CreateAsync(Guid storeId, Guid productId, decimal price)
    {
        if (productId == Guid.Empty)
        {
            throw new UserFriendlyException("gecerli bir urun secilmeli");
        }
        var store = await _storeRepository.FindAsync(storeId, includeDetails: false);
        if (store == null)
        {
            throw new UserFriendlyException("Boyle bir eczane yok.");
        }

        if (!store.IsActive)
        {
            throw new UserFriendlyException("Pasif eczaneye urun eklenemez.");
        }

        var existing = await _storeProductRepository.FindByStoreAndProductAsync(storeId, productId);
        if (existing != null)
        {
            throw new UserFriendlyException("Bu urun bu eczanede zaten listelenmis.");
        }

        return new StoreProduct(GuidGenerator.Create(), storeId, productId, price);
    }
}


