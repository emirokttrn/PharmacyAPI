using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.DomainServices;
using PharmacyAPI.IRepositories;
using PharmacyAPI.IServices.Review;
using PharmacyAPI.ReviewDtos;
using PharmacyAPI.Reviews;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.ReviewServices
{
    public class ReviewService : ApplicationService, IReviewService
    {
        private readonly IReviewRepository _repository;
        private readonly ReviewManager _reviewManager;

        public ReviewService(IReviewRepository repository, ReviewManager reviewManager)
        {
            _repository = repository;
            _reviewManager = reviewManager;
        }

        public async Task<ReviewDto> CreateAsync(CreateReviewDto request)
        {
            var review = await _reviewManager.CreateAsync(request.ProductId, request.CustomerId, request.Rating, request.Comment);

            await _repository.InsertAsync(review);

            // [Claude Agent] - Review olusturulduktan sonra Product.Rating/ReviewCount yeniden hesaplanir
            await _reviewManager.AfterReviewChangedAsync(review.ProductId);

            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        public async Task<List<ReviewDto>> GetListByProductAsync(Guid productId)
        {
            var reviews = await _repository.GetListByProductAsync(productId);
            return ObjectMapper.Map<List<Review>, List<ReviewDto>>(reviews);
        }

        public async Task DeleteAsync(Guid id)
        {
            var review = await _repository.FindAsync(id);
            if (review == null)
            {
                throw new UserFriendlyException("bu id ile bir yorum bulunamadi");
            }

            await _repository.DeleteAsync(id);

            // [Claude Agent] - Review silindikten sonra Product.Rating/ReviewCount yeniden hesaplanir
            await _reviewManager.AfterReviewChangedAsync(review.ProductId);
        }
    }
}
