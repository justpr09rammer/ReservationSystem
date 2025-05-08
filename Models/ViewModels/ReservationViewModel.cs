using System;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Models.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad daxil edilməlidir")]
        [Display(Name = "Ad")]
        [StringLength(50, ErrorMessage = "Ad 50 simvoldan çox ola bilməz")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad daxil edilməlidir")]
        [Display(Name = "Soyad")]
        [StringLength(50, ErrorMessage = "Soyad 50 simvoldan çox ola bilməz")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Telefon nömrəsi daxil edilməlidir")]
        [Display(Name = "Telefon nömrəsi")]
        [StringLength(20, ErrorMessage = "Telefon nömrəsi 20 simvoldan çox ola bilməz")]
        [RegularExpression(@"^[0-9+\s-]+$", ErrorMessage = "Telefon nömrəsi düzgün formatda deyil")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Tarix daxil edilməlidir")]
        [Display(Name = "Tarix")]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Saat daxil edilməlidir")]
        [Display(Name = "Saat")]
        [DataType(DataType.Time)]
        public TimeSpan ReservationTime { get; set; }

        [Required(ErrorMessage = "Bərbər seçilməlidir")]
        [Display(Name = "Bərbər")]
        public int BarberId { get; set; }

        public string BarberName { get; set; }
    }
}
