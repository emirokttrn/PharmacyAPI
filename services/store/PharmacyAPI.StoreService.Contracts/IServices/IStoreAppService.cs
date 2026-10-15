using Volo.Abp.Application.Services;

public interface IStoreAppService : IApplicationService
{
    Task<StoreDto> GetAsync(Guid id);
    Task<StoreDto> CreateAsync(CreateStoreDto input);
    Task<StoreDto> UpdateAsync(Guid id, UpdateStoreDto input);
    Task DeleteAsync(Guid id);
    Task<StoreDto> SetWorkingHourAsync(Guid id, SetStoreWorkingHoursDto input);
    Task<StoreDto> RemoveWorkingHourAsync(Guid id, DayOfWeek day);
    Task<StoreDto> SetOnDutyAsync(Guid id, bool isOnDuty);
    Task<StoreDto> SetActiveAsync(Guid id, bool isActive);
    Task<List<NearbyStoreDto>> GetNearbyAsync(GetNearbyStoresInput input);
}
