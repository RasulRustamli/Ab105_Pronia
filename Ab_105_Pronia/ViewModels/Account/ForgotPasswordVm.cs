using System.ComponentModel.DataAnnotations;

namespace Ab_105_Pronia.ViewModels.Account
{
    public class ForgotPasswordVm
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
