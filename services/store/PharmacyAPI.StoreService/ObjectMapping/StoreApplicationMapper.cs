using PharmacyAPI.StoreService.Entities.Stores;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.StoreService.ObjectMapping;

// Sadece DTO tarafindaki eslenmeyen alanlar uyari verir; entity'deki ConcurrencyStamp
// ve ExtraProperties DTO'da bilerek yok.
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class StoreApplicationMapper : MapperBase<Store, StoreDto>
{
    public override partial StoreDto Map(Store source);
    public override partial void Map(Store source, StoreDto destination);

    // StoreWorkingHours listesindeki elemanlar icin Mapperly bu metodu kullanir.
    [MapProperty(nameof(StoreWorkingHour.OpenTime), nameof(StoreWorkingHourDto.TimeOnly))]
    private partial StoreWorkingHourDto MapWorkingHour(StoreWorkingHour source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class StoreWorkingHourApplicationMapper : MapperBase<StoreWorkingHour, StoreWorkingHourDto>
{
    // Entity'de "OpenTime", DTO'da "TimeOnly"; adlar farkli oldugu icin acikca eslestiriyoruz.
    [MapProperty(nameof(StoreWorkingHour.OpenTime), nameof(StoreWorkingHourDto.TimeOnly))]
    public override partial StoreWorkingHourDto Map(StoreWorkingHour source);
    [MapProperty(nameof(StoreWorkingHour.OpenTime), nameof(StoreWorkingHourDto.TimeOnly))]
    public override partial void Map(StoreWorkingHour source, StoreWorkingHourDto destination);
}
