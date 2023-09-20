namespace YemekTarifiApp.Core.Models;

public class Base
{
    public string Id { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }


}