using PharmacyAPI.StoreService.Entities.Stores;
using PharmacyAPI.StoreService.Services.DomainServices;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.StoreService.Services.Stores;

public class StoreAppService : ApplicationService, IStoreAppService
{
    private readonly IStoreRepository _storeRepository;

    public StoreAppService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<StoreDto> GetAsync(Guid id)
    {
        var store = await _storeRepository.GetAsync(id, includeDetails: true);
        return ObjectMapper.Map<Store, StoreDto>(store);
    }

    public async Task<StoreDto> CreateAsync(CreateStoreDto input)
    {
        var store = new Store(GuidGenerator.Create(), input.StoreName, input.PhoneNumber, input.Address,
            input.City, input.District, input.Latitude, input.Longitude);

        await _storeRepository.InsertAsync(store, autoSave: true);
        return ObjectMapper.Map<Store, StoreDto>(store);
    }

    public async Task<StoreDto> UpdateAsync(Guid id, UpdateStoreDto input)
    {
        var store = await _storeRepository.GetAsync(id, includeDetails: true);

        store.SetStoreName(input.StoreName);
        store.SetPhoneNumber(input.PhoneNumber);
        store.SetAddress(input.Address, input.City, input.District);
        store.SetLocation(input.Latitude, input.Longitude);

        await _storeRepository.UpdateAsync(store, autoSave: true);
        return ObjectMapper.Map<Store, StoreDto>(store);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _storeRepository.DeleteAsync(id);
    }

    public async Task<StoreDto> SetWorkingHourAsync(Guid id, SetStoreWorkingHoursDto input)
    {
        var store = await _storeRepository.GetAsync(id, includeDetails: true);
        store.SetWorkingHour(input.DayOfWeek, input.OpenTime, input.CloseTime);

        await _storeRepository.UpdateAsync(store, autoSave: true);
        return ObjectMapper.Map<Store, StoreDto>(store);
    }

    public async Task<StoreDto> RemoveWorkingHourAsync(Guid id, DayOfWeek day)
    {
        var store = await _storeRepository.GetAsync(id, includeDetails: true);
        store.RemoveWorkingHour(day);

        await _storeRepository.UpdateAsync(store, autoSave: true);
        return ObjectMapper.Map<Store, StoreDto>(store);
    }

    public async Task<StoreDto> SetOnDutyAsync(Guid id, bool isOnDuty)
    {
        var store = await _storeRepository.GetAsync(id, includeDetails: true);
        if (isOnDuty)
        {
            store.MarkOnDuty();
        }
        else
        {
            store.UnmarkOnDuty();
        }

        await _storeRepository.UpdateAsync(store, autoSave: true);
        return ObjectMapper.Map<Store, StoreDto>(store);
    }

    public async Task<StoreDto> SetActiveAsync(Guid id, bool isActive)
    {
        var store = await _storeRepository.GetAsync(id, includeDetails: true);
        if (isActive)
        {
            store.Activate();
        }
        else
        {
            store.Deactivate();
        }

        await _storeRepository.UpdateAsync(store, autoSave: true);
        return ObjectMapper.Map<Store, StoreDto>(store);
    }

    public async Task<List<NearbyStoreDto>> GetNearbyAsync(GetNearbyStoresInput input)
    {
        // Once DB'de kaba bir kutu ile daralt, sonra gercek mesafeyi bellekte hesapla.
        var box = GeoCalculator.BoundingBox(input.Latitude, input.Longitude, input.RadiusKm);
        var stores = await _storeRepository.GetActivateListBoundingBoxAsync(box.MinLat, box.MaxLat, box.MinLng, box.MaxLng);

        var now = IstanbulClock.Now;

        return stores
            .Select(store => new
            {
                Store = store,
                DistanceKm = GeoCalculator.DistanceKm(input.Latitude, input.Longitude, store.Latitude, store.Longitude),
                IsOpenNow = store.IsOpenAt(now)
            })
            .Where(x => x.DistanceKm <= input.RadiusKm)
            .Where(x => !input.OnlyOpen || x.IsOpenNow)
            .OrderBy(x => x.DistanceKm)
            .Select(x => new NearbyStoreDto
            {
                Id = x.Store.Id,
                StoreName = x.Store.StoreName,
                FullAddress = x.Store.FullAddress,
                District = x.Store.District,
                Latitude = x.Store.Latitude,
                Longitude = x.Store.Longitude,
                DistanceKm = Math.Round(x.DistanceKm, 2),
                IsOpenNow = x.IsOpenNow,
                IsOnDuty = x.Store.IsOnDuty,
                AverageRating = x.Store.AverageRating,
                RatingCount = x.Store.RatingCount
            })
            .ToList();
    }
}
