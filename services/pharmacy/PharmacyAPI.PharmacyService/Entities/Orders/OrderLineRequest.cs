using System;

namespace PharmacyAPI.Orders
{
    // [Claude Agent] - OrderManager.CreateAsync'e siparis satirlarini gecmek icin kullanilan
    // basit domain-seviyesi istek nesnesi (DTO degil, Application katmanindan bagimsiz)
    public record OrderLineRequest(Guid ProductId, int Quantity, Guid? PrescriptionId);
}
