
using PharmacyAPI.StoreService.Entities.StoreReviews;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace PharmacyAPI.StoreService.Services.DomainServices
{
    public class StoreReviewManager : DomainService
    {
        private readonly IStoreReviewRepository _storeReviewRepository;
        private readonly IStoreRepository _storeRepository;

        public StoreReviewManager(
            IStoreReviewRepository storeReviewRepository,
            IStoreRepository storeRepository)
        {
            _storeReviewRepository = storeReviewRepository;
            _storeRepository = storeRepository;
        }

        public async Task<StoreReview> CreateAsync(Guid storeId, Guid userId, int rating, string? comment)
        {
            var store = await _storeRepository.FindAsync(storeId, includeDetails: false);
            if (store == null)
            {
                throw new UserFriendlyException("Boyle bir eczane yok.");
            }

            if (!store.IsActive)
            {
                throw new UserFriendlyException("Pasif eczaneye puan verilemez.");
            }

            var existing = await _storeReviewRepository.FindByStoreAndUserAsync(storeId, userId);
            if (existing != null)
            {
                throw new UserFriendlyException("Bu eczaneye zaten puan verdiniz.");
            }

            var review = new StoreReview(GuidGenerator.Create(), storeId, userId, rating, comment);

            store.ApplyRating(rating);
            await _storeRepository.UpdateAsync(store);

            return review;
        }
    }
}