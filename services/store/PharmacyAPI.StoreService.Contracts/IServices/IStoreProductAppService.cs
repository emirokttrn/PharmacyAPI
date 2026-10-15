using Volo.Abp.Application.Services;

public interface IStoreProductAppService : IApplicationService
{
    Task<StoreProductDto> CreateAsync(Guid storeId, CreateStoreProductDto input);
    Task<StoreProductDto> UpdateAsync(Guid id, UpdateStoreProductDto input);
    Task<List<StoreProductDto>> GetListByStoreAsync(Guid storeId);
    Task DeleteAsync(Guid id);
    Task<List<StoreOfferDto>> GetOffersAsync(Guid productId, GetStoreOffersInput input);
}
