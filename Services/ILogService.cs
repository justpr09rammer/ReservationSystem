using System;
using System.Threading.Tasks;

namespace ReservationSystem.Services
{
    public interface ILogService
    {
        Task LogExceptionAsync(Exception ex, string source = null);
    }
}
