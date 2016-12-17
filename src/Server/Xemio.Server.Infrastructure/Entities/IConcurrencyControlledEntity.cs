namespace Xemio.Server.Infrastructure.Entities
{
    public interface IConcurrencyControlledEntity
    {
        byte[] ETag { get; set; }
    }
}
