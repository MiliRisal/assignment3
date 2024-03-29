namespace Assignment3.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}


namespace Assignment3.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string pricing { get; set; }
        public string shippingCost { get; set; }
    }
}

namespace Assignment3.Models
{
    public class User
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string purchaseHistory { get; set; }
        public string shippingAddress { get; set; }
    }
}

namespace Assignment3.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public string product { get; set; }
        public string user { get; set; }
        public string rating { get; set; }
        public string images { get; set; }
        public string text { get; set; }
    }
}

namespace Assignment3.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string products { get; set; }
        public string quantities { get; set; }
        public string user { get; set; }
    }
}

namespace Assignment3.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
