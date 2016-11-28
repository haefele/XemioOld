namespace Xemio.Server.Infrastructure.Entites
{
    public interface IConcurrencyControlledEntity
    {
        byte[] ETag { get; set; }
    }
}
