using ReservationSystem.Models.Entities;
using System.Threading.Tasks;

namespace ReservationSystem.Services
{
    public interface IAuthService
    {
        Task<Barber> AuthenticateAsync(string username, string password);
        Task<bool> SignInAsync(Barber barber);
        Task<bool> RegisterAsync(Barber barber);
        Task SignOutAsync();
    }
}
