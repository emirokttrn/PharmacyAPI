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
            //artik return tipi IQueryable<Product> olan her nesneninyanina include details methodunu cagirabilirsin 
            public static IQueryable<Product> IncludeDetails(this IQueryable<Product> queryable, bool include=true)
            {
                if(!include) return queryable; // eger include false ise hic join ekletmiyoruz burda direk querable veriyor
                return queryable.Include(p=>p.Brand).Include(c=>c.Category);
            }
        }

  }
