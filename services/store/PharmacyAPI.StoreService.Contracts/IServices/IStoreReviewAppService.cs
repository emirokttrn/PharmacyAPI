using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

public interface IStoreReviewAppService : IApplicationService
{
    Task<StoreReviewDto> CreateAsync(Guid storeId, CreateStoreReviewDto input);
    Task<StoreReviewDto> UpdateAsync(Guid id, UpdateStoreReviewDto input);
    Task<PagedResultDto<StoreReviewDto>> GetListAsync(Guid storeId, PagedResultRequestDto input);
}
