

using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

public class CreateStoreDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string StoreName { get; set; } = null!;

    [Required]
    [StringLength(16)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [StringLength(256)]
    public string Address { get; set; } = null!;

    [Required]
    [StringLength(64)]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(64)]
    public string District { get; set; } = null!;

    [Required]
    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Required]
    [Range(-180, 180)]
    public double Longitude { get; set; }
}

public class UpdateStoreDto : CreateStoreDto
{
}
public class SetStoreWorkingHoursDto
{
    [Required]
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }

}
public class StoreWorkingHourDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly TimeOnly { get; set; }
    public TimeOnly CloseTime { get; set; }
}
public class StoreDto : FullAuditedEntityDto<Guid>
{
    public string StoreName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string FullAddress { get; set; } = null!;
    public string City { get; set; } = null!;
    public string District { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsActive { get; set; }
    public bool IsOnDuty { get; set; }
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
    public List<StoreWorkingHourDto> StoreWorkingHours { get; set; } = new();

}
public class GetNearbyStoresInput
{
    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    [Range(0.1, 50)]
    public double RadiusKm { get; set; } = 5;

    public bool OnlyOpen { get; set; }
}

public class NearbyStoreDto
{
    public Guid Id { get; set; }
    public string StoreName { get; set; } = null!;
    public string FullAddress { get; set; } = null!;
    public string District { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double DistanceKm { get; set; }
    public bool IsOpenNow { get; set; }
    public bool IsOnDuty { get; set; }
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
}
