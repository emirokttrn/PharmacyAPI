namespace PharmacyAPI.StoreService.Services.DomainServices
{
    public static class GeoCalculator
    {
        private const double EarthRadiusKm = 6371;
        private const double KmPerDegreeLatitude = 111.0;

        public static double DistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                  + Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2))
                  * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        public static (double MinLat, double MaxLat, double MinLng, double MaxLng) BoundingBox(
            double latitude, double longitude, double radiusKm)
        {
            var latDelta = radiusKm / KmPerDegreeLatitude;
            var lngDelta = radiusKm / (KmPerDegreeLatitude * Math.Cos(ToRadians(latitude)));

            return (latitude - latDelta, latitude + latDelta, longitude - lngDelta, longitude + lngDelta);
        }

        private static double ToRadians(double degrees) => degrees * Math.PI / 180;
    }
}