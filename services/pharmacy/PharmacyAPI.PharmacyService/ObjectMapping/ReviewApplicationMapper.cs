using PharmacyAPI.ReviewDtos;
using PharmacyAPI.Reviews;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.ReviewServices
{
    [Mapper]
    public partial class ReviewApplicationMapper : MapperBase<Review, ReviewDto>
    {
        public override partial ReviewDto Map(Review source);
        public override partial void Map(Review source, ReviewDto destination);
    }
}
