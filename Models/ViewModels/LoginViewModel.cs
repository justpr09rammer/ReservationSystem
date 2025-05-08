using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir")]
        [Display(Name = "İstifadəçi adı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifrə daxil edilməlidir")]
        [Display(Name = "Şifrə")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Məni xatırla")]
        public bool RememberMe { get; set; }
    }
}
