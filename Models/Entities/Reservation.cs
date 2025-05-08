using System;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Models.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad daxil edilməlidir")]
        [StringLength(50, ErrorMessage = "Ad 50 simvoldan çox ola bilməz")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad daxil edilməlidir")]
        [StringLength(50, ErrorMessage = "Soyad 50 simvoldan çox ola bilməz")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Telefon nömrəsi daxil edilməlidir")]
        [StringLength(20, ErrorMessage = "Telefon nömrəsi 20 simvoldan çox ola bilməz")]
        [RegularExpression(@"^[0-9+\s-]+$", ErrorMessage = "Telefon nömrəsi düzgün formatda deyil")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Tarix daxil edilməlidir")]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Saat daxil edilməlidir")]
        public TimeSpan ReservationTime { get; set; }

        // Foreign key
        public int BarberId { get; set; }
        
        // Navigation property
        public virtual Barber Barber { get; set; }
    }
}
