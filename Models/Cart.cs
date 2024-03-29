namespace Assignment3.Models
{
    public class Cart
    {
        public int? Id { get; set; }
        public List<Product>? Products { get; set; }
        public List<int>? Quantities { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}