using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels
{
    public class EditProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Provide valid Product Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must contain at least 3 letters and max 100 letters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Provide valid Product Description")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Product Description must contain at least 10 letters and max 1000 letters")]
        public string Description { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be at least 1 to 100")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please Provide valid Product Price Start from 5")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please Provide valid Product Category")]
        [DisplayName("Select Product Category")]
        public int CategoryId { get; set; }

        public string VendorId { get; set; } = "al123";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<string> ExistingImages { get; set; } = new List<string>(); // صور موجودة بالفعل
        public IFormFileCollection NewAttachments { get; set; } // صور جديدة تترفع

        public bool IsDelated { get; set; } = false;
    }
}
