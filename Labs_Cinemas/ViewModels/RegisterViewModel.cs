using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Labs_Cinemas.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        [Display(Name = "Repeate Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
