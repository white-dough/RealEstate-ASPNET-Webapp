namespace RealEstate.Models
{
    public class OrderListModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PropertyId { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
    }
}
