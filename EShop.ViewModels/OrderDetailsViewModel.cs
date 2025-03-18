namespace EShop.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string ClientName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public List<OrderItemViewModel> Items { get; set; }
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
