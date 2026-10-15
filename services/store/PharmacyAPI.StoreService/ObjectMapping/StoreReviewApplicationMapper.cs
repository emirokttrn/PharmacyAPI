using PharmacyAPI.StoreService.Entities.StoreReviews;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.StoreService.ObjectMapping;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class StoreReviewApplicationMapper : MapperBase<StoreReview, StoreReviewDto>
{
    // DTO'daki alan adi "Raiting" (yazim hatasi); acikca eslestiriyoruz.
    [MapProperty(nameof(StoreReview.Rating), nameof(StoreReviewDto.Raiting))]
    public override partial StoreReviewDto Map(StoreReview source);
    [MapProperty(nameof(StoreReview.Rating), nameof(StoreReviewDto.Raiting))]
    public override partial void Map(StoreReview source, StoreReviewDto destination);
}
