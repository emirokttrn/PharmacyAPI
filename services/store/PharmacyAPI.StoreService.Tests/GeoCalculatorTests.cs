using PharmacyAPI.StoreService.Services.DomainServices;

namespace PharmacyAPI.StoreService.Tests;

public class GeoCalculatorTests
{
    [Fact]
    public void DistanceKm_Istanbul_To_Ankara_Is_About_351_Km()
    {
        var distance = GeoCalculator.DistanceKm(41.0082, 28.9784, 39.9334, 32.8597);

        Assert.InRange(distance, 346, 356);
    }

    [Fact]
    public void DistanceKm_Same_Point_Is_Zero()
    {
        var distance = GeoCalculator.DistanceKm(41.0082, 28.9784, 41.0082, 28.9784);

        Assert.Equal(0, distance, precision: 6);
    }

    [Fact]
    public void BoundingBox_Contains_Center()
    {
        const double lat = 41.0082;
        const double lng = 28.9784;

        var box = GeoCalculator.BoundingBox(lat, lng, 5);

        Assert.InRange(lat, box.MinLat, box.MaxLat);
        Assert.InRange(lng, box.MinLng, box.MaxLng);
    }
}
