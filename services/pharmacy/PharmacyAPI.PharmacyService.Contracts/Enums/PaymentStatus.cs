namespace PharmacyAPI
{
    public enum PaymentStatus
    {
        Pending, // islemde
        Succesful, // basarili
        Failed, // basarisiz

        rejected, // iptal edilme
        Refunded // iade edilme
    }
}