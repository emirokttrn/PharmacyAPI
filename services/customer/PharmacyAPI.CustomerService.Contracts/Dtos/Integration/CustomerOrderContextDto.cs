namespace PharmacyAPI.CustomerService.Contracts.Dtos.Integration
{
    public class CustomerOrderContextDto
    {
        public bool IsAdult { get; set; }

        public string CustomerName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public string AddressTitle { get; set; } = null!;
        public string FullAddress { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string? PostalCode { get; set; }
    }
}