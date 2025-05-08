using ReservationSystem.Models.Entities;
using System.Threading.Tasks;

namespace ReservationSystem.Repositories
{
    public interface IBarberRepository : IRepository<Barber>
    {
        Task<Barber> GetByUsernameAsync(string username);
    }
}
