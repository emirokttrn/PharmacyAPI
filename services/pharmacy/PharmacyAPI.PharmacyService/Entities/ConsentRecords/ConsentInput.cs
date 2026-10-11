namespace PharmacyAPI.ConsentRecords
{
    public record ConsentInput(ConsentType ConsentType, bool ConsentGiven, string ConsentTextVersion, string? IpAddress);
}
