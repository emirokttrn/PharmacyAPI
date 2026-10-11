using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PharmacyAPI.HealtTopics;
using PharmacyAPI.ProductDtos;
using PharmacyAPI.Products;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI;

 public class PharmacyAPIApplicationAutoMapperProfile : Profile
    {
        public PharmacyAPIApplicationAutoMapperProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.HealthTopics,
                    opt => opt.MapFrom(src => src.healthTopics.Select(h => h.Topic).ToList()));
        }
    }