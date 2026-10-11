using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.BranDtos
{
    public class BrandDto :FullAuditedEntityDto<Guid>
    {
        public string BrandName { get; set; }
    }
}