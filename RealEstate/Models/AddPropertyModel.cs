using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models
{
	public class AddPropertyModel
	{
		public string Title { get; set; }
		public string PContent { get; set; }
		public string Type { get; set; }
		public string Bhk { get; set; }
		public string Stype { get; set; }
		public int Bedroom { get; set; }
		public int Bathroom { get; set; }
		public int Balcony { get; set; }
		public int Kitchen { get; set; }
		public int Hall { get; set; }
		public string Floor { get; set; }
		public int Size { get; set; }
		public int Price { get; set; }
		public string Location { get; set; }
		public string City { get; set; }
		public string Barangay { get; set; }
		public string Feature { get; set; }
		public string PImage { get; set; }
		public string PImageOne { get; set; }
		public string PImageTwo { get; set; }
		public string PImageThree { get; set; }
		public string PImageFour { get; set; }
		public IFormFile PImageFile { get; set; }
		public IFormFile PImageOneFile { get; set; }
		public IFormFile PImageTwoFile { get; set; }
		public IFormFile PImageThreeFile { get; set; }
		public IFormFile PImageFourFile { get; set; }
		public string Uid { get; set; }
		public string Status { get; set; }
		public string MapImage { get; set; }
		public string TopMapImage { get; set; }
		public string GroundMapImage { get; set; }
		public IFormFile MapImageFile { get; set; }
		public IFormFile TopMapImageFile { get; set; }
		public IFormFile GroundMapImageFile { get; set; }
		public string TotalFloor { get; set; }
		public int IsFeatured { get; set; }
		public DateTime Date { get; set; }
	}
}
