using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem.Models.Entities;
using ReservationSystem.Models.ViewModels;
using ReservationSystem.Repositories;
using ReservationSystem.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReservationSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IBarberRepository _barberRepository;
        private readonly ILogService _logService;
        private const string CustomerInfoCookieKey = "CustomerInfo";

        public ReservationController(
            IReservationService reservationService,
            IBarberRepository barberRepository,
            ILogService logService)
        {
            _reservationService = reservationService;
            _barberRepository = barberRepository;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                // Debug info
                System.Diagnostics.Debug.WriteLine("GET Create action called");
                
                var viewModel = new ReservationViewModel
                {
                    ReservationDate = DateTime.Today,
                    ReservationTime = DateTime.Now.TimeOfDay
                };

                // Populate barbers dropdown
                await PopulateBarbersList();
                
                // Debug info - check if barbers exist
                var barbers = await _barberRepository.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"Number of barbers in database: {barbers.Count()}");
                foreach (var barber in barbers)
                {
                    System.Diagnostics.Debug.WriteLine($"Barber: {barber.FirstName} {barber.LastName}, ID: {barber.Id}");
                }

                // Check if customer info cookie exists
                if (Request.Cookies.TryGetValue(CustomerInfoCookieKey, out string customerInfo))
                {
                    var parts = customerInfo.Split('|');
                    if (parts.Length == 3)
                    {
                        viewModel.FirstName = parts[0];
                        viewModel.LastName = parts[1];
                        viewModel.PhoneNumber = parts[2];
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in GET Create: {ex.Message}");
                await _logService.LogExceptionAsync(ex, nameof(Create));
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationViewModel model)
        {
            try
            {

                System.Diagnostics.Debug.WriteLine("POST Create action called");
                System.Diagnostics.Debug.WriteLine($"Form submitted with values: FirstName={Request.Form["FirstName"]}, LastName={Request.Form["LastName"]}, PhoneNumber={Request.Form["PhoneNumber"]}, BarberId={Request.Form["BarberId"]}");
                System.Diagnostics.Debug.WriteLine($"Model binding result: FirstName={model.FirstName}, LastName={model.LastName}, PhoneNumber={model.PhoneNumber}, BarberId={model.BarberId}");
                

                await PopulateBarbersList();
                

                System.Diagnostics.Debug.WriteLine($"Model state valid: {ModelState.IsValid}");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {key}: {state.Errors[0].ErrorMessage}");
                    }
                }
                
                if (Request.Form.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine("Attempting manual binding from form data");
                    
                    if (string.IsNullOrEmpty(model.FirstName) && Request.Form.TryGetValue("FirstName", out var firstName) && !string.IsNullOrEmpty(firstName))
                        model.FirstName = firstName.ToString();
                        
                    if (string.IsNullOrEmpty(model.LastName) && Request.Form.TryGetValue("LastName", out var lastName) && !string.IsNullOrEmpty(lastName))
                        model.LastName = lastName.ToString();
                        
                    if (string.IsNullOrEmpty(model.PhoneNumber) && Request.Form.TryGetValue("PhoneNumber", out var phoneNumber) && !string.IsNullOrEmpty(phoneNumber))
                        model.PhoneNumber = phoneNumber.ToString();
                    
                    if (model.BarberId == 0 && Request.Form.TryGetValue("BarberId", out var barberIdValue) && !string.IsNullOrEmpty(barberIdValue) && int.TryParse(barberIdValue, out int barberId))
                        model.BarberId = barberId;
                    
                    // Handle date conversion
                    if (Request.Form.TryGetValue("ReservationDate", out var dateValue) && !string.IsNullOrEmpty(dateValue))
                    {
                        System.Diagnostics.Debug.WriteLine($"Parsing date from: {dateValue}");
                        if (DateTime.TryParse(dateValue, out DateTime date))
                        {
                            model.ReservationDate = date;
                            System.Diagnostics.Debug.WriteLine($"Successfully parsed date: {model.ReservationDate}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Failed to parse date from: {dateValue}");
                        }
                    }
                    
                    if (Request.Form.TryGetValue("ReservationTime", out var timeValue) && !string.IsNullOrEmpty(timeValue))
                    {
                        string timeStr = timeValue.ToString();
                        System.Diagnostics.Debug.WriteLine($"Parsing time from: {timeStr}");
                        if (TimeSpan.TryParse(timeStr, out TimeSpan time))
                        {
                            model.ReservationTime = time;
                            System.Diagnostics.Debug.WriteLine($"Successfully parsed time: {model.ReservationTime}");
                        }
                        else
                        {
                            try
                            {
                                if (timeStr.Contains(":"))
                                {
                                    var parts = timeStr.Split(':');
                                    if (parts.Length >= 2 && int.TryParse(parts[0], out int hours) && int.TryParse(parts[1], out int minutes))
                                    {
                                        model.ReservationTime = new TimeSpan(hours, minutes, 0);
                                        System.Diagnostics.Debug.WriteLine($"Successfully parsed time using alternative method: {model.ReservationTime}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Alternative time parsing failed: {ex.Message}");
                            }
                        }
                    }
                    
                    ModelState.Clear();
                    TryValidateModel(model);
                }
                
                bool isValid = !string.IsNullOrEmpty(model.FirstName) && 
                               !string.IsNullOrEmpty(model.LastName) && 
                               !string.IsNullOrEmpty(model.PhoneNumber) && 
                               model.BarberId > 0 && 
                               model.ReservationDate != default && 
                               model.ReservationTime != default;
                
                System.Diagnostics.Debug.WriteLine($"Manual validation result: {isValid}");
                System.Diagnostics.Debug.WriteLine($"Final model values: FirstName={model.FirstName}, LastName={model.LastName}, PhoneNumber={model.PhoneNumber}, BarberId={model.BarberId}, Date={model.ReservationDate}, Time={model.ReservationTime}");
                
                if (isValid)
                {
                    System.Diagnostics.Debug.WriteLine("Model is valid, creating reservation");
                    
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"Creating reservation with: FirstName={model.FirstName}, LastName={model.LastName}, PhoneNumber={model.PhoneNumber}, Date={model.ReservationDate}, Time={model.ReservationTime}, BarberId={model.BarberId}");
                        
                        var reservation = new Reservation
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            PhoneNumber = model.PhoneNumber,
                            ReservationDate = model.ReservationDate,
                            ReservationTime = model.ReservationTime,
                            BarberId = model.BarberId
                        };

                        await _reservationService.CreateReservationAsync(reservation);
                        System.Diagnostics.Debug.WriteLine("Reservation created successfully");
                        
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            System.Diagnostics.Debug.WriteLine("Returning JSON success response");
                            return Json(new { success = true, message = "Rezervasiya uğurla yaradıldı" });
                        }
                        
                        return RedirectToAction("Success");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error creating reservation: {ex.Message}");
                        await _logService.LogExceptionAsync(ex, nameof(Create));
                        
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            System.Diagnostics.Debug.WriteLine("Returning JSON error response");
                            return Json(new { success = false, message = "Rezervasiya yaradılarkən xəta baş verdi. Zəhmət olmasa bir az sonra yenidən cəhd edin." });
                        }
                        
                        ModelState.AddModelError("", "Rezervasiya yaradılarkən xəta baş verdi. Zəhmət olmasa bir az sonra yenidən cəhd edin.");
                    }
                }
                
                System.Diagnostics.Debug.WriteLine("Model validation failed");
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Zəhmət olmasa bütün sahələri düzgün doldurun" });
                }
                
                return View(model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in POST Create: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                await _logService.LogExceptionAsync(ex, nameof(Create));
                
                // Check if it's an AJAX request
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Rezervasiya yaradılarkən xəta baş verdi. Zəhmət olmasa bir az sonra yenidən cəhd edin." });
                }
                
                ModelState.AddModelError("", "Rezervasiya yaradılarkən xəta baş verdi");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                int barberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var reservations = await _reservationService.GetReservationsByBarberIdAsync(barberId);
                return View(reservations);
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(Index));
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var reservation = await _reservationService.GetReservationByIdAsync(id);
                if (reservation == null)
                {
                    return NotFound();
                }

                int barberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (reservation.BarberId != barberId)
                {
                    return Forbid();
                }

                return View(reservation);
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(Details));
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var reservation = await _reservationService.GetReservationByIdAsync(id);
                if (reservation == null)
                {
                    return NotFound();
                }

                int barberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (reservation.BarberId != barberId)
                {
                    return Forbid();
                }

                return View(reservation);
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(Delete));
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var reservation = await _reservationService.GetReservationByIdAsync(id);
                if (reservation == null)
                {
                    return NotFound();
                }

                int barberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (reservation.BarberId != barberId)
                {
                    return Forbid();
                }

                await _reservationService.DeleteReservationAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _logService.LogExceptionAsync(ex, nameof(DeleteConfirmed));
                return RedirectToAction("Error", "Home");
            }
        }

        private async Task PopulateBarbersList()
        {
            var barbers = await _barberRepository.GetAllAsync();
            ViewBag.Barbers = barbers.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = $"{b.FirstName} {b.LastName}"
            }).ToList();
        }
    }
}
