

using System.ComponentModel.DataAnnotations;
using PharmacyAPI.StoreService.Contracts.Enums;
using Volo.Abp.Application.Dtos;

public class CreateStoreProductDto
{
    public Guid ProductId {get; set;}

    [Range(typeof(decimal),"0.01","100000", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal Price {get;set;}
}
public class UpdateStoreProductDto
{
    [Range(typeof(decimal),"0.01","100000", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal Price {get;set;}
    public bool IsAvailable {get;set;}
}
public class StoreProductDto :FullAuditedEntityDto<Guid>
{
    public Guid StoreId{get;set;}
    public Guid ProductId {get; set;}
    public decimal Price{get;set;}
    public bool IsAvailable {get; set;}
}
public class GetStoreOffersInput
{
    [Range(-90,90)]
    public double? Latitude {get;set;}
    [Range(-180,180)]
    public double? Longitude {get; set;}

    public OfferSortType SortBy {get; set;}= OfferSortType.Price;

}
public class StoreOfferDto
    {
        public Guid StoreId { get; set; }
        public string StoreName { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public double? DistanceKm { get; set; }
        public bool IsOpenNow { get; set; }
        public double AverageRating { get; set; }
    }