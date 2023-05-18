
namespace RealEstate.Helper
{
	public class CustomPasswordHasher
	{
		public string HashPassword(string password) {
			return Encrypt.getMD5Hash(password);
		}

		public bool VerifyHashedPassword(string password, string providedPassword)
		{
			if (password == HashPassword(providedPassword)) {
				return true;
			}

			return false;
		}
	}
}
