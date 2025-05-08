using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using ReservationSystem.Models.Entities;
using ReservationSystem.Repositories;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReservationSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBarberRepository _barberRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IBarberRepository barberRepository, IHttpContextAccessor httpContextAccessor)
        {
            _barberRepository = barberRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Barber> AuthenticateAsync(string username, string password)
        {
            var barber = await _barberRepository.GetByUsernameAsync(username);
            
            if (barber != null && barber.Password == password) // In a real application, use password hashing
            {
                return barber;
            }
            
            return null;
        }

        public async Task<bool> SignInAsync(Barber barber)
        {
            if (barber == null)
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, barber.Username),
                new Claim(ClaimTypes.NameIdentifier, barber.Id.ToString()),
                new Claim("FirstName", barber.FirstName),
                new Claim("LastName", barber.LastName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return true;
        }
        public async Task<bool> RegisterAsync(Barber barber)
        {
            var existing = await _barberRepository.GetByUsernameAsync(barber.Username);
            if (existing != null) return false;

            await _barberRepository.AddAsync(barber);

            return true;
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
