using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.Brands
{
    public class Brand :FullAuditedAggregateRoot<Guid>
    {
        // aqsin burda brand icin biraz daha prop. eklemiz gerek sadece name ile olmaz
        // bide clean code'a dikkat et, abp ayni aspnet gibi isimizi cok kolaylastiracak 
        //temel crudlar icin veya sade methodlar icin controller yazmiyacaz otamtik endpointini olusturyor
        public const int BrandNameMaxLength=128;
        public const int BrandNameMinLength=2;
        public virtual string BrandName {get; set;} = null!;

        protected Brand(){}
        // burda id yazmamiza gerek yok guid bizim icin id olusturyor int id yapma yani olusturdugu id su sekil 
        //anskdocanc123e31-123123csncacnlnzcxclksc-2314 
        public Brand(Guid id, string Name) :base(id)
        {BrandName=Name;}

        //direk engel icin 
        public virtual void SetName(string name)
        {
            Check.NotNullOrWhiteSpace(name,nameof(BrandName), BrandNameMaxLength,BrandNameMinLength);
            BrandName=name;
        }
    }
}