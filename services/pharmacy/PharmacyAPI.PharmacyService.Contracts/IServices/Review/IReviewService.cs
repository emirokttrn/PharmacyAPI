using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.ReviewDtos;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices.Review
{
    public interface IReviewService : IApplicationService
    {
        Task<ReviewDto> CreateAsync(CreateReviewDto request);
        Task<List<ReviewDto>> GetListByProductAsync(Guid productId);
        Task DeleteAsync(Guid id);
    }
}
