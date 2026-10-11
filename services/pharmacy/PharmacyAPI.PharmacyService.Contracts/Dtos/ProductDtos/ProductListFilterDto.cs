using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.ProductDtos
{
    public class ProductListFilterDto : PagedAndSortedResultRequestDto
    {
        public Guid? CategoryId {get;set;}
        public Guid? BrandId{get;set;}

        public double? MinRating{get;set;}
        public List<string>? PriceRangeIds {get;set;}
        public List<string>? HealthTopics {get;set;}

        public List<string>? Gender {get;set;}

       public List<string>? AgeRanges { get; set; }
        public List<string>? ProductForms { get; set; }
        public List<string>? WeightRangeIds { get; set; }
        
         public ProductSortOption? Sort { get; set; }


    }
}