namespace RealEstate.Models
{
    public class PropertyListUserModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Stype { get; set; }
        public int Size { get; set; }
        public int Price { get; set; }
        public string Location { get; set; }
        public int IsFeatured { get; set; }
        public string PImage { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
    }
}
