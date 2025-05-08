using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data.Context;
using ReservationSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystem.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByBarberIdAsync(int barberId)
        {
            return await _dbSet.Where(r => r.BarberId == barberId)
                              .OrderBy(r => r.ReservationDate)
                              .ThenBy(r => r.ReservationTime)
                              .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByDateAsync(DateTime date)
        {
            return await _dbSet.Where(r => r.ReservationDate.Date == date.Date)
                              .OrderBy(r => r.ReservationTime)
                              .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByBarberIdAndDateAsync(int barberId, DateTime date)
        {
            return await _dbSet.Where(r => r.BarberId == barberId && r.ReservationDate.Date == date.Date)
                              .OrderBy(r => r.ReservationTime)
                              .ToListAsync();
        }
    }
}
