using Microsoft.AspNetCore.Mvc.Rendering;

namespace EShop.ViewModels
{
    public class AddOrderViewModel
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ClientId { get; set; }

        // List of products + quantity
        //public List<OrderItemInput> Items { get; set; } = new List<OrderItemInput>();

        // Dropdowns:
        public List<SelectListItem> Clients { get; set; }
        public List<SelectListItem> Products { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }
    }

    public class OrderItemInput
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
