using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.HealtTopics;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.Products
{
    public class Product : FullAuditedAggregateRoot<Guid>
    {
        public string ProductName { get; set; }
        public const int ProductNameMaxLength = 128;
        public const int ProductNameMinLength = 2;
        public string Country { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public int SoldLast30Days { get; set; }


        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }

        public string ActiveIngredient { get; set; }
        public string Description { get; set; }

        public ProductBadge? Badge { get; set; }
        public string BadgeLabel { get; set; }
        public bool IsNew { get; set; }

        public ProductGender? Gender { get; set; }
        public string AgeRange { get; set; }
        public string ProductForm { get; set; }
        public string Weight { get; set; }


        public ICollection<ProductHealthTopic> healthTopics { get; set; } = new List<ProductHealthTopic>();
        public bool InStock { get; set; }

        protected Product() { }

        public Product(Guid id, string name, Guid brandId, Guid categoryId, decimal price) : base(id)
        {
            ProductName = name;
            BrandId = brandId;
            CategoryId = categoryId;
            Price = price;
        }



        public void SetName(string Name)
        {
            Check.NotNullOrWhiteSpace(Name, nameof(ProductName), ProductNameMinLength, ProductNameMaxLength);
            ProductName = Name;
        }
        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new UserFriendlyException("fiyat sifirdan kucuk olamaz dikkat et! ");
            }
            Price = price;
        }
        public void SetDiscountedPrice(decimal? discountedPrice)
        {
            //idirimli fiyatin bi degeri varsa ve o deger sifirdan kucukse exception atiyor
            if (discountedPrice.HasValue && discountedPrice.Value < 0)
            {
                throw new UserFriendlyException("idirimli fiyat sifirdan kucuk olamaz");
            }
            //eger idirimli fiyat normal degferden fazla ise exception atar
            if (discountedPrice.HasValue && discountedPrice.Value > Price)
            {
                throw new UserFriendlyException("indirimli fiyat normal fiayattan fazla olmaz");
            }
            DiscountedPrice = discountedPrice;
        }
        public void SetRating(double rating)
        {
            if (rating < 0 || rating > 5)
            {
                throw new UserFriendlyException("rating 0 ile 5 arasinda olmali!");
            }
            Rating = rating;
        }

        public void SetReviewCount(int reviewCount)
        {
            if (reviewCount < 0)
            {
                throw new UserFriendlyException("review count negatif olamaz!");
            }
            ReviewCount = reviewCount;
        }

        public void SetSoldLast30Days(int soldLast30Days)
        {
            if (soldLast30Days < 0)
            {
                throw new UserFriendlyException("satis adedi negatif olamaz!");
            }
            SoldLast30Days = soldLast30Days;
        }

        public void SetCountry(string country)
        {
            Country = Check.NotNullOrWhiteSpace(country, nameof(Country), 64);
        }

        public void SetImage(string image)
        {
            Image = Check.NotNullOrWhiteSpace(image, nameof(Image), 512);
        }

        public void SetActiveIngredient(string activeIngredient)
        {
            ActiveIngredient = Check.Length(activeIngredient, nameof(ActiveIngredient), 256);
        }

        public void SetDescription(string description)
        {
            Description = Check.Length(description, nameof(Description), 2000);
        }

        public void SetBadgeLabel(string badgeLabel)
        {
            BadgeLabel = Check.Length(badgeLabel, nameof(BadgeLabel), 64);
        }

        public void SetAgeRange(string ageRange)
        {
            AgeRange = Check.Length(ageRange, nameof(AgeRange), 64);
        }

        public void SetProductForm(string productForm)
        {
            ProductForm = Check.Length(productForm, nameof(ProductForm), 64);
        }

        public void SetWeight(string weight)
        {
            Weight = Check.Length(weight, nameof(Weight), 32);
        }

        public void SetBrand(Guid brandId)
        {
            BrandId = brandId;
        }

        public void SetCategory(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public void SetBadge(ProductBadge? badge)
        {
            Badge = badge;
        }

        public void SetGender(ProductGender? gender)
        {
            Gender = gender;
        }

        public void MarkAsNew(bool isNew = true)
        {
            IsNew = isNew;
        }

        public void SetStock(bool inStock)
        {
            InStock = inStock;
        }



    }




}