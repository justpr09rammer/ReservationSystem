using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Models.Entities
{
    public class Barber
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad daxil edilməlidir")]
        [StringLength(50, ErrorMessage = "Ad 50 simvoldan çox ola bilməz")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad daxil edilməlidir")]
        [StringLength(50, ErrorMessage = "Soyad 50 simvoldan çox ola bilməz")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir")]
        [StringLength(50, ErrorMessage = "İstifadəçi adı 50 simvoldan çox ola bilməz")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifrə daxil edilməlidir")]
        [StringLength(100, ErrorMessage = "Şifrə 100 simvoldan çox ola bilməz")]
        public string Password { get; set; }

        // Navigation property
        public virtual ICollection<Reservation> Reservations { get; set; }

        public Barber()
        {
            Reservations = new HashSet<Reservation>();
        }
    }
}
