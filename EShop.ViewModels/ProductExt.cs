using EF_Core.Models;

namespace EShop.ViewModels
{
    public static class productExt
    {
        public static Product ToModel(this AddProductViewModel viewModel)
        {
            return new Product
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                Price = viewModel.Price,
                Quantity = viewModel.Quantity,
                VendorId = viewModel.VendorId,
                CreatedAt = viewModel.CreatedAt,
                IsDelated = viewModel.IsDelated,
                Attachments = viewModel.Paths.Select(path=> new ProductAttachment() { Image = path}).ToList()
            };
        }
        public static ProductDetailsViewModel ToDetailsVModel(this Product viewModel)
        {
            return new ProductDetailsViewModel
            {
                Id= viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                CategoryName = viewModel.Category.Name,
                Price = viewModel.Price,
                Quantity = viewModel.Quantity,
                VendorName = viewModel.Vendor.User.UserName ?? "Not Provided",
                CreatedAt = viewModel.CreatedAt,
                Images = viewModel.Attachments.Select(i=>i.Image).ToList()
                //Attachments
            };
        }
    }
}
