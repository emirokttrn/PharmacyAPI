namespace PharmacyAPI.ConsentRecords
{
    // [Claude Agent] - CustomerManager.CreateAsync'e rizalari gecmek icin domain-seviyesi istek nesnesi
    public record ConsentInput(ConsentType ConsentType, bool ConsentGiven, string ConsentTextVersion, string? IpAddress);
}
