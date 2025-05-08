using ReservationSystem.Models.Entities;
using ReservationSystem.Repositories;
using System;
using System.Threading.Tasks;

namespace ReservationSystem.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task LogExceptionAsync(Exception ex, string source = null)
        {
            var log = new Log
            {
                Message = ex.Message,
                Level = "Error",
                Timestamp = DateTime.Now,
                Exception = ex.GetType().Name,
                StackTrace = ex.StackTrace,
                Source = source ?? ex.Source
            };

            await _logRepository.AddAsync(log);
            await _logRepository.SaveChangesAsync();
        }
    }
}
