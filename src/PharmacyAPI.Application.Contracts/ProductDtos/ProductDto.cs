using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.ProductDtos
{
    public class ProductDto : FullAuditedEntityDto<Guid>
    {

        public string ProductName { get; set; }
        public const int ProductNameMaxLength = 128;
        public const int ProductNameMinLength = 2;
        public string Country { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public int SoldLast30Days { get; set; }


           public Guid BrandId { get; set; }
        public string BrandName { get; set; }   // join edilmiş, frontend BrandId ile ayrıca sorgu atmasın diye

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } // aynı mantık



        public string ActiveIngredient { get; set; }
        public string Description { get; set; }

        public ProductBadge? Badge { get; set; }
        public string BadgeLabel { get; set; }
        public bool IsNew { get; set; }

        public ProductGender? Gender { get; set; }
        public string AgeRange { get; set; }
        public string ProductForm { get; set; }
        public string Weight { get; set; }

        public List<string> HealthTopics { get; set; } = new List<string>();
        public bool InStock { get; set; }
        

    }
}