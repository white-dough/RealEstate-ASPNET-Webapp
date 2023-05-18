using System.Security.Cryptography;

namespace RealEstate
{
	public class Encrypt
	{
		public static string getMD5Hash(string input)
		{
			MD5 md5 = MD5.Create();
			byte[] b = System.Text.Encoding.UTF8.GetBytes(input);
			b = md5.ComputeHash(b);
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (byte x in b)
			{
				sb.Append(x.ToString("x2"));
			}
			return sb.ToString();
		}
	}
}
