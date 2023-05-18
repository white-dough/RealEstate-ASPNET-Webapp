namespace RealEstate.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string PropertyId { get; set; }
        public int Status { get; set; }   
        public DateTime Date { get; set; }
    }
}
