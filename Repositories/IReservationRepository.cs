using ReservationSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationSystem.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetReservationsByBarberIdAsync(int barberId);
        Task<IEnumerable<Reservation>> GetReservationsByDateAsync(DateTime date);
        Task<IEnumerable<Reservation>> GetReservationsByBarberIdAndDateAsync(int barberId, DateTime date);
    }
}
