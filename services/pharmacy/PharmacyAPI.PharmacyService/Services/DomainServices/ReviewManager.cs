using System;
using System.Threading.Tasks;
using PharmacyAPI.IRepositories;
using PharmacyAPI.Reviews;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PharmacyAPI.DomainServices
{
    // [Claude Agent] - Review olusturma kurallari + Product.Rating/ReviewCount yeniden hesaplama tetikleyicisi
    public class ReviewManager : DomainService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ProductManager _productManager;
        private readonly IGuidGenerator _guidGenerator;

        public ReviewManager(
            IReviewRepository reviewRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            ProductManager productManager,
            IGuidGenerator guidGenerator)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _productManager = productManager;
            _guidGenerator = guidGenerator;
        }

        public async Task<Review> CreateAsync(Guid productId, Guid customerId, int rating, string? comment)
        {
            var product = await _productRepository.FindAsync(productId);
            if (product == null)
            {
                throw new UserFriendlyException("boyle bir urun yok!");
            }

            var customer = await _customerRepository.FindAsync(customerId);
            if (customer == null)
            {
                throw new UserFriendlyException("boyle bir musteri yok!");
            }

            var existingReview = await _reviewRepository.FindByProductAndCustomerAsync(productId, customerId);
            if (existingReview != null)
            {
                throw new UserFriendlyException("bu urune zaten bir yorum yaptiniz!");
            }

            return new Review(_guidGenerator.Create(), productId, customerId, rating, comment);
        }

        public async Task AfterReviewChangedAsync(Guid productId)
        {
            await _productManager.RecalculateRatingAsync(productId);
        }
    }
}
