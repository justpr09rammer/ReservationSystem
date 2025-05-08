using ReservationSystem.Models.Entities;
using ReservationSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogService _logService;

        public ReservationService(IReservationRepository reservationRepository, ILogService logService)
        {
            _reservationRepository = reservationRepository;
            _logService = logService;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            try
            {
                return await _reservationRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(GetAllReservationsAsync));
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByBarberIdAsync(int barberId)
        {
            try
            {
                return await _reservationRepository.GetReservationsByBarberIdAsync(barberId);
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(GetReservationsByBarberIdAsync));
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByDateAsync(DateTime date)
        {
            try
            {
                return await _reservationRepository.GetReservationsByDateAsync(date);
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(GetReservationsByDateAsync));
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByBarberIdAndDateAsync(int barberId, DateTime date)
        {
            try
            {
                return await _reservationRepository.GetReservationsByBarberIdAndDateAsync(barberId, date);
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(GetReservationsByBarberIdAndDateAsync));
                throw;
            }
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            try
            {
                return await _reservationRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(GetReservationByIdAsync));
                throw;
            }
        }

        public async Task CreateReservationAsync(Reservation reservation)
        {
            try
            {
                await _reservationRepository.AddAsync(reservation);
                await _reservationRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(CreateReservationAsync));
                throw;
            }
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            try
            {
                await _reservationRepository.UpdateAsync(reservation);
                await _reservationRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(UpdateReservationAsync));
                throw;
            }
        }

        public async Task DeleteReservationAsync(int id)
        {
            try
            {
                var reservation = await _reservationRepository.GetByIdAsync(id);
                if (reservation != null)
                {
                    await _reservationRepository.DeleteAsync(reservation);
                    await _reservationRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(DeleteReservationAsync));
                throw;
            }
        }
    }
}
