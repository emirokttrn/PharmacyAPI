using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

public class CreateStoreReviewDto
{
    [Range(1,5)]
    public int Rating {get; set;}

    [StringLength(1000)]
    public string? Comment {get;set;}
}
public class UpdateStoreReviewDto
{
    [StringLength (1000)]
    public string? Comment {get; set;}
}
public class StoreReviewDto: FullAuditedEntityDto<Guid>
{
    public Guid StoreId {get; set;}
    public Guid UserId {get; set;}
    public int Raiting {get; set;}
    public string? Comment {get;set;}
}