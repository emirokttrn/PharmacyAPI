using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.Customers;
using PharmacyAPI.IRepositories;
using PharmacyAPI.Orders;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PharmacyAPI.DomainServices
{
    // [Claude Agent] - Order/Cart olusturma is kurallari: stok yeterliligi, recete gerektiren
    // urunler icin onayli recete zorunlulugu, 18+ urunler icin yas kontrolu, siparis anindaki
    // fiyat/isim snapshot'i ve stok dususu burada yapilir.
    public class OrderManager : DomainService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IGuidGenerator _guidGenerator;

        public OrderManager(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            ICustomerAddressRepository customerAddressRepository,
            IPrescriptionRepository prescriptionRepository,
            IGuidGenerator guidGenerator)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _customerAddressRepository = customerAddressRepository;
            _prescriptionRepository = prescriptionRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<Order> CreateAsync(Guid customerId, Guid shippingAddressId, List<OrderLineRequest> lines)
        {
            if (lines == null || lines.Count == 0)
            {
                throw new UserFriendlyException("siparişte en az bir urun olmali!");
            }

            var customer = await _customerRepository.FindAsync(customerId);
            if (customer == null)
            {
                throw new UserFriendlyException("boyle bir musteri yok!");
            }

            var shippingAddress = await _customerAddressRepository.FindAsync(shippingAddressId);
            if (shippingAddress == null || shippingAddress.CustomerId != customerId)
            {
                throw new UserFriendlyException("bu teslimat adresi bu musteriye ait degil!");
            }

            var order = new Order(_guidGenerator.Create(), customerId, shippingAddressId);

            foreach (var line in lines)
            {
                var product = await _productRepository.FindAsync(line.ProductId);
                if (product == null)
                {
                    throw new UserFriendlyException($"{line.ProductId} id'li urun bulunamadi!");
                }

                if (product.StockQuantity < line.Quantity)
                {
                    throw new UserFriendlyException($"{product.ProductName} icin yeterli stok yok!");
                }

                if (product.RequiresPrescription)
                {
                    if (line.PrescriptionId == null)
                    {
                        throw new UserFriendlyException($"{product.ProductName} recete gerektiriyor, PrescriptionId zorunlu!");
                    }

                    var prescription = await _prescriptionRepository.FindAsync(line.PrescriptionId.Value);
                    if (prescription == null || prescription.CustomerId != customerId || prescription.Status != PrescriptionStatus.Approved)
                    {
                        throw new UserFriendlyException($"{product.ProductName} icin gecerli, onaylanmis bir recete gerekli!");
                    }
                }

                if (!string.IsNullOrWhiteSpace(product.AgeRange) && product.AgeRange.Contains("18+"))
                {
                    if (customer.DateOfBirth == null || Customer.CalculateAge(customer.DateOfBirth.Value) < 18)
                    {
                        throw new UserFriendlyException($"{product.ProductName} icin musteri 18 yasindan buyuk olmali!");
                    }
                }

                var unitPrice = product.DiscountedPrice ?? product.Price;
                var orderLine = new OrderLine(_guidGenerator.Create(), order.Id, product.Id, product.ProductName, line.Quantity, unitPrice);
                order.AddLine(orderLine);

                product.DecreaseStock(line.Quantity);
                await _productRepository.UpdateAsync(product);
            }

            return order;
        }
    }
}
