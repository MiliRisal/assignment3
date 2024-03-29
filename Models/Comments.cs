namespace Assignment3.Models
{
    public class Comments
    {
        public int? Id { get; set; }
        public Product? Product { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? Rating { get; set; }
        public string? Images { get; set; }
        public string? Text { get; set; }
    }
}