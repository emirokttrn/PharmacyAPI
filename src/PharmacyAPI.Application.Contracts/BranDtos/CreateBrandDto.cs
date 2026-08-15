using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.BranDtos
{
    public class CreateBrandDto : FullAuditedEntityDto<Guid>
    {
        [Required]
        [StringLength(128, MinimumLength = 2)]
        public string BrandName { get; set; }
    }
}