using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "User Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "User Name must be between 3 and 50 characters")]

        
        public string UserName { get; set; }



        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
        [Compare("PasswordConfirmed", ErrorMessage = "Password and Confirm Password must match")]
        public string Password { get; set; }    


        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]

        public string PasswordConfirmed { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid Phone Number")]
        [Phone]

        public string PhoneNumber { get; set; }
        
    }
}
