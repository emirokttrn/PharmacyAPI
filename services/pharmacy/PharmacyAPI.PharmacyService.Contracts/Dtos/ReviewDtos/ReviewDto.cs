using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.ReviewDtos
{
    public class CreateReviewDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }
    }

    public class ReviewDto : FullAuditedEntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
