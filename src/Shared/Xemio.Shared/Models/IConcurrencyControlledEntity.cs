namespace Xemio.Shared.Models.Notes
{
    public interface IConcurrencyControlledEntity
    {
        byte[] ETag { get; set; }
    }
}
