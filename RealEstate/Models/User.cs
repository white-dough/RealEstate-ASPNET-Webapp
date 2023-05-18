using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Username { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public string RoleName { get; set; }
	}
}
