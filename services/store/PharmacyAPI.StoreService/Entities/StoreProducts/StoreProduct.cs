using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.StoreService.Entities.StoreProducts
{
    public class StoreProduct : FullAuditedAggregateRoot<Guid>
    {

        public Guid StoreId {get; protected set;}
        public Guid ProductId {get; protected set;}

        public decimal Price {get; protected set;}

        public bool IsAvailable {get; protected set;}

        protected StoreProduct(){}
        public StoreProduct(Guid id , Guid storeId , Guid productId, decimal price):base(id)
        {
            StoreId =storeId;
            ProductId=productId;
            SetPrice(price);
            MarkAvailable();
        }

        public void SetPrice(decimal price)
        {
            if(price<=0)
            {
                throw new UserFriendlyException("fiyat 0 ve asagisi olamaz! ");

            }
            Price=price;
        }

        public void MarkAvailable()
        {
            IsAvailable=true;
        }
        public void UnMarkAvailable()
        {
            IsAvailable=false;
        }
    }
}
