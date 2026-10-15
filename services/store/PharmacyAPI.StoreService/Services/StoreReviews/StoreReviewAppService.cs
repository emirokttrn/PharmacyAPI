using PharmacyAPI.StoreService.Entities.StoreReviews;
using PharmacyAPI.StoreService.Services.DomainServices;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Users;

namespace PharmacyAPI.StoreService.Services.StoreReviews;

public class StoreReviewAppService : ApplicationService, IStoreReviewAppService
{
    private readonly IStoreReviewRepository _storeReviewRepository;
    private readonly StoreReviewManager _storeReviewManager;

    public StoreReviewAppService(IStoreReviewRepository storeReviewRepository, StoreReviewManager storeReviewManager)
    {
        _storeReviewRepository = storeReviewRepository;
        _storeReviewManager = storeReviewManager;
    }

    public async Task<StoreReviewDto> CreateAsync(Guid storeId, CreateStoreReviewDto input)
    {
        var review = await _storeReviewManager.CreateAsync(storeId, CurrentUser.GetId(), input.Rating, input.Comment);
        await _storeReviewRepository.InsertAsync(review, autoSave: true);
        return ObjectMapper.Map<StoreReview, StoreReviewDto>(review);
    }

    public async Task<StoreReviewDto> UpdateAsync(Guid id, UpdateStoreReviewDto input)
    {
        var review = await _storeReviewRepository.GetAsync(id);
        if (review.UserId != CurrentUser.GetId())
        {
            throw new AbpAuthorizationException("Sadece kendi yorumunuzu duzenleyebilirsiniz.");
        }

        review.SetComment(input.Comment);

        await _storeReviewRepository.UpdateAsync(review, autoSave: true);
        return ObjectMapper.Map<StoreReview, StoreReviewDto>(review);
    }

    public async Task<PagedResultDto<StoreReviewDto>> GetListAsync(Guid storeId, PagedResultRequestDto input)
    {
        var query = (await _storeReviewRepository.GetQueryableAsync()).Where(x => x.StoreId == storeId);

        var totalCount = await AsyncExecuter.CountAsync(query);
        var reviews = await AsyncExecuter.ToListAsync(
            query.OrderByDescending(x => x.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount));

        return new PagedResultDto<StoreReviewDto>(
            totalCount,
            reviews.Select(ObjectMapper.Map<StoreReview, StoreReviewDto>).ToList());
    }
}
