using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data.Context;
using ReservationSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystem.Repositories
{
    public class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate)
                              .OrderByDescending(l => l.Timestamp)
                              .ToListAsync();
        }
    }
}
