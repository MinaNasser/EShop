using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels
{
    public class MultiLineDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var description = value as string;
            if (string.IsNullOrWhiteSpace(description))
                return new ValidationResult("Description is required");

            if (!description.Contains("\n")&&description.Split(Environment.NewLine).Length<2 )
                return new ValidationResult("Description must be multi-line (contain line breaks)");

            return ValidationResult.Success;
        }
    }

}
