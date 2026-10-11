using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.CategoryDtos
{
    public class CategoryDto : FullAuditedEntityDto<Guid>
    {
        public string CategoryName {get; set;}
    }
}