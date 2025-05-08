using ReservationSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationSystem.Repositories
{
    public interface ILogRepository : IRepository<Log>
    {
        Task<IEnumerable<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
