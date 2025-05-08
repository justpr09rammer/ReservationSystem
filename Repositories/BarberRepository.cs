using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data.Context;
using ReservationSystem.Models.Entities;
using System.Threading.Tasks;

namespace ReservationSystem.Repositories
{
    public class BarberRepository : Repository<Barber>, IBarberRepository
    {
        public BarberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Barber> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.Username == username);
        }
    }
}
