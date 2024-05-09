namespace Ab_105_Pronia.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string MainPhotoUrl { get; set; }
        public string PhotoUrl { get; set; }
    }
}
