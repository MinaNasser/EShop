using EShop.ViewModels;
using EF_Core.Models; // أو المسار الخاص بـ Order و Product

public static class OrderExtensions
{
    public static OrderDetailsViewModel ToDetailsVModel(this Order order)
    {
        return new OrderDetailsViewModel
        {
            Id = order.Id,
            Date = order.Date,
            TotalPrice = order.TotalPrice,
            TotalQuantity = order.TotalQuantity,
            Status = order.Status.ToString(),
            PaymentMethod = order.PaymentMethod.ToString(),
            City = order.City,
            Address = order.Address,
            ClientName = order.Client != null ? order.Client.User.UserName : "Unknown",
            Items = order.Items.Select(i => new OrderItemViewModel
            {
                ProductName = i.Product != null ? i.Product.Name : "Unknown Product",
                Quantity = i.Quantity
            }).ToList()
        };
    }
}
