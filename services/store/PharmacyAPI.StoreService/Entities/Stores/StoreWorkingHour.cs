using Volo.Abp.Domain.Entities;

namespace PharmacyAPI.StoreService.Entities.Stores
{
    public class StoreWorkingHour : Entity
    {
        public Guid StoreId { get; protected set; }
        public DayOfWeek DayOfWeek { get; protected set; }

        public TimeOnly OpenTime { get; protected set; }
        public TimeOnly CloseTime { get; protected set; }

        protected StoreWorkingHour()
        {

        }
        internal StoreWorkingHour(Guid storeId, DayOfWeek dayOfWeek, TimeOnly openTime, TimeOnly closeTime)
        {
            StoreId= storeId;
            DayOfWeek=dayOfWeek;
            OpenTime=openTime;
            CloseTime=closeTime;

        }
        internal void SetHours(TimeOnly openTime, TimeOnly closeTime)
        {
            OpenTime=openTime;
            CloseTime=closeTime;
        }

        public override object?[] GetKeys()=> new object[]{StoreId, DayOfWeek};
    }
}
