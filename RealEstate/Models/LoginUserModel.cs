using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
	public class LoginUserModel
	{
		[Required(ErrorMessage = "Required.")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Required.")]
		public string Password { get; set; }
	}
}
