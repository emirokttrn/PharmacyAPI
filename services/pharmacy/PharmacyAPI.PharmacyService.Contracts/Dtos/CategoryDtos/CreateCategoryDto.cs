using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.CategoryDtos
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(128, MinimumLength = 2)]
        public string CategorName { get; set; }
        public Guid? guid { get; set; }
    }
}