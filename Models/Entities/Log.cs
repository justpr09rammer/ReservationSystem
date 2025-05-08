using System;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Models.Entities
{
    public class Log
    {
        public int Id { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        [Required]
        public string Level { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }
        
        public string Exception { get; set; }
        
        public string StackTrace { get; set; }
        
        public string Source { get; set; }
    }
}
