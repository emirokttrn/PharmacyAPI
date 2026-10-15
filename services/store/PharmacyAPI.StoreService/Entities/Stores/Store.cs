using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.StoreService.Entities.Stores
{
    public static class StoreConsts
    {
        public const int MaxStoreNameLength = 128;
        public const int MaxPhoneNumberLength = 16;
        public const int MaxFullAddressLength = 256;
        public const int MaxCityLength = 64;
        public const int MaxDistrictLength = 64;
    }

    public class Store : FullAuditedAggregateRoot<Guid>
    {
        public string StoreName { get; protected set; } = null!;
        public string PhoneNumber { get; protected set; } = null!;
        public string FullAddress { get; protected set; } = null!;
        public string City { get; protected set; } = null!;
        public string District { get; protected set; } = null!;
        public double Latitude { get; protected set; }
        public double Longitude { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool IsOnDuty { get; protected set; }
        public double AverageRating { get; protected set; }
        public int RatingCount { get; protected set; }
        public ICollection<StoreWorkingHour> StoreWorkingHours { get; protected set; } = new List<StoreWorkingHour>();

        protected Store() { }

        public Store(Guid id, string storeName, string phoneNumber, string fullAddress,
                     string city, string district, double latitude, double longitude) : base(id)
        {
            SetStoreName(storeName);
            SetPhoneNumber(phoneNumber);
            SetAddress(fullAddress, city, district);
            SetLocation(latitude, longitude);

            IsActive = true;
            IsOnDuty = false;
            AverageRating = 0;
            RatingCount = 0;
        }

        public void SetStoreName(string storeName)
        {
            StoreName = Check.NotNullOrWhiteSpace(storeName, nameof(StoreName), StoreConsts.MaxStoreNameLength);
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(PhoneNumber), StoreConsts.MaxPhoneNumberLength);

            var digits = phoneNumber.StartsWith('+') ? phoneNumber[1..] : phoneNumber;
            if (digits.Length == 0 || !IsAllDigits(digits))
            {
                throw new UserFriendlyException("Telefon numarasi sadece rakam icermeli.");
            }

            PhoneNumber = phoneNumber;
        }

        public void SetAddress(string fullAddress, string city, string district)
        {
            FullAddress = Check.NotNullOrWhiteSpace(fullAddress, nameof(FullAddress), StoreConsts.MaxFullAddressLength);
            City = Check.NotNullOrWhiteSpace(city, nameof(City), StoreConsts.MaxCityLength);
            District = Check.NotNullOrWhiteSpace(district, nameof(District), StoreConsts.MaxDistrictLength);
        }

        public void SetLocation(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new UserFriendlyException("Enlem -90 ile 90 arasinda olmali.");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new UserFriendlyException("Boylam -180 ile 180 arasinda olmali.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        public void MarkOnDuty() => IsOnDuty = true;
        public void UnmarkOnDuty() => IsOnDuty = false;
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void SetWorkingHour(DayOfWeek day, TimeOnly openTime, TimeOnly closeTime)
        {
            if (openTime >= closeTime)
            {
                throw new UserFriendlyException("Acilis saati kapanis saatinden once olmali.");
            }

            var existing = StoreWorkingHours.FirstOrDefault(w => w.DayOfWeek == day);
            if (existing != null)
            {
                existing.SetHours(openTime, closeTime);
            }
            else
            {
                StoreWorkingHours.Add(new StoreWorkingHour(Id, day, openTime, closeTime));
            }
        }

        public void RemoveWorkingHour(DayOfWeek day)
        {
            var existing = StoreWorkingHours.FirstOrDefault(w => w.DayOfWeek == day);
            if (existing != null)
            {
                StoreWorkingHours.Remove(existing);
            }
        }

        public bool IsOpenAt(DateTime localTime)
        {
            if (!IsActive)
            {
                return false;
            }

            if (IsOnDuty)
            {
                return true;
            }

            var workingHour = StoreWorkingHours.FirstOrDefault(w => w.DayOfWeek == localTime.DayOfWeek);
            if (workingHour == null)
            {
                return false;
            }

            var time = TimeOnly.FromDateTime(localTime);
            return time >= workingHour.OpenTime && time < workingHour.CloseTime;
        }

        internal void ApplyRating(int rating)
        {
            if (rating < 1 || rating > 5)
            {
                throw new UserFriendlyException("Puan 1 ile 5 arasinda olmali.");
            }

            AverageRating = (AverageRating * RatingCount + rating) / (RatingCount + 1);
            RatingCount++;
        }

        private static bool IsAllDigits(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}