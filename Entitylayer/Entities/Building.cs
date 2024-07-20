namespace EntityLayer.Entities;

public class Building : BaseEntity
{
    public string Metro { get; set; }
    public string City { get; set; }
    public string Village { get; set; }
    public string District { get; set; }
    public decimal Price { get; set; }
    public int RoomCount { get; set; }
    public decimal Area { get; set; }
    public string AdType { get; set; }
    public string SellerType { get; set; }
}
