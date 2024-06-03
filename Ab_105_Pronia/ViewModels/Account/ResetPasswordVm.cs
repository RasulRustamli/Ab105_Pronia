using System.ComponentModel.DataAnnotations;

namespace Ab_105_Pronia.ViewModels.Account
{
    public class ResetPasswordVm
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password))]

        public string ConfirmPassword { get; set; }
    }
}
