namespace RealEstate.Models
{
    public class AddOrderModel
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
        public int Status { get; set; }
    }
}
