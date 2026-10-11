using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.CategoryDtos
{
    public class CategoryFilterDto : PagedAndSortedResultRequestDto
    {
        public string? FilterByName{get;set;}
        public bool? OnlyRoot {get; set;}
    }
}