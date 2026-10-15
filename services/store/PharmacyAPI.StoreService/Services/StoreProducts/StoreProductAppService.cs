using PharmacyAPI.StoreService.Contracts.Enums;
using PharmacyAPI.StoreService.Entities.StoreProducts;
using PharmacyAPI.StoreService.Services.DomainServices;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.StoreService.Services.StoreProducts;

public class StoreProductAppService : ApplicationService, IStoreProductAppService
{
    private readonly IStoreProductRepository _storeProductRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly StoreProductManager _storeProductManager;

    public StoreProductAppService(
        IStoreProductRepository storeProductRepository,
        IStoreRepository storeRepository,
        StoreProductManager storeProductManager)
    {
        _storeProductRepository = storeProductRepository;
        _storeRepository = storeRepository;
        _storeProductManager = storeProductManager;
    }

    public async Task<StoreProductDto> CreateAsync(Guid storeId, CreateStoreProductDto input)
    {
        var storeProduct = await _storeProductManager.CreateAsync(storeId, input.ProductId, input.Price);
        await _storeProductRepository.InsertAsync(storeProduct, autoSave: true);
        return ObjectMapper.Map<StoreProduct, StoreProductDto>(storeProduct);
    }

    public async Task<StoreProductDto> UpdateAsync(Guid id, UpdateStoreProductDto input)
    {
        var storeProduct = await _storeProductRepository.GetAsync(id);

        storeProduct.SetPrice(input.Price);
        if (input.IsAvailable)
        {
            storeProduct.MarkAvailable();
        }
        else
        {
            storeProduct.UnMarkAvailable();
        }

        await _storeProductRepository.UpdateAsync(storeProduct, autoSave: true);
        return ObjectMapper.Map<StoreProduct, StoreProductDto>(storeProduct);
    }

    public async Task<List<StoreProductDto>> GetListByStoreAsync(Guid storeId)
    {
        var storeProducts = await _storeProductRepository.GetListAsync(x => x.StoreId == storeId);
        return storeProducts.Select(ObjectMapper.Map<StoreProduct, StoreProductDto>).ToList();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _storeProductRepository.DeleteAsync(id);
    }

    public async Task<List<StoreOfferDto>> GetOffersAsync(Guid productId, GetStoreOffersInput input)
    {
        var storeProducts = await _storeProductRepository.GetListAsync(x => x.ProductId == productId);
        if (storeProducts.Count == 0)
        {
            return new List<StoreOfferDto>();
        }

        // Ilgili eczaneler tek sorguda; pasif olanlar teklif listesinden cikar.
        var storeIds = storeProducts.Select(x => x.StoreId).Distinct().ToList();
        var stores = (await _storeRepository.GetListAsync(s => storeIds.Contains(s.Id), includeDetails: true))
            .Where(s => s.IsActive)
            .ToDictionary(s => s.Id);

        var hasLocation = input.Latitude.HasValue && input.Longitude.HasValue;
        var now = IstanbulClock.Now;

        var offers = storeProducts
            .Where(sp => stores.ContainsKey(sp.StoreId))
            .Select(sp =>
            {
                var store = stores[sp.StoreId];
                return new StoreOfferDto
                {
                    StoreId = store.Id,
                    StoreName = store.StoreName,
                    Price = sp.Price,
                    IsAvailable = sp.IsAvailable,
                    DistanceKm = hasLocation
                        ? Math.Round(GeoCalculator.DistanceKm(input.Latitude!.Value, input.Longitude!.Value, store.Latitude, store.Longitude), 2)
                        : null,
                    IsOpenNow = store.IsOpenAt(now),
                    AverageRating = store.AverageRating
                };
            });

        offers = input.SortBy switch
        {
            OfferSortType.Distance when hasLocation => offers.OrderBy(x => x.DistanceKm).ThenBy(x => x.Price),
            OfferSortType.Rating => offers.OrderByDescending(x => x.AverageRating).ThenBy(x => x.Price),
            _ => offers.OrderBy(x => x.Price)
        };

        return offers.ToList();
    }
}
