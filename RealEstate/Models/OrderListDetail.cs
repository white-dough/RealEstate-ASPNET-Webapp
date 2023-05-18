namespace RealEstate.Models
{
	public class OrderListDetail
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string UserName { get; set; }
		public string UserId { get; set; }	
		public string PropertyId { get; set; }
		public int Status { get; set; }
		public DateTime Date { get; set; }
	}
}
