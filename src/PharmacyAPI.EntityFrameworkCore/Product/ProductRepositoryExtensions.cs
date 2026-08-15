using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Products;

namespace PharmacyAPI.Products
{
    
        public static class ProductRepositoryExtensions
        {
            public static IQueryable<Product> IncludeDetails(this IQueryable<Product> queryable, bool include=true)
            {
                if(!include) return queryable;
                return queryable.Include(p=>p.brand).Include(c=>c.category);
            }
        }

  }
