using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using PharmacyAPI;

namespace PharmacyAPI.ProductDtos
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(maximumLength: 128, MinimumLength = 2)]
        public string ProductName { get; set; }

        [Required]
        public Guid BrandId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "fiyat 0 veya buyuk olmali ")]
        public decimal Price { get; set; }

        public decimal? DiscountedPrice { get; set; } // null olabilir cunku surekli indirim mi olacak! 

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(512)]
        public string Image { get; set; }

        [StringLength(256)]
        public string ActiveIngredient { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public ProductBadge? Badge { get; set; }

        [StringLength(64)]
        public string BadgeLabel { get; set; }

        public ProductGender? Gender { get; set; }

        [StringLength(64)]
        public string AgeRange { get; set; }

        [StringLength(64)]
        public string ProductForm { get; set; }

        [StringLength(32)]
        public string Weight { get; set; }

        public List<string> HealthToPICS { get; set; } = new List<string>();

        [Required]
        public bool InStock { get; set; }




    }
}