// File: src/Healthcare.MVC/Controllers/PatientPortalController.cs
// Purpose: Provides normal User functionality: dashboard, patient profile, appointment booking, and appointment history.
// Security: Restricted to Role=User, uses anti-forgery tokens, server-side validation, and does not expose PatientId in booking forms.

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
using Healthcare.Core.Exceptions;
using Healthcare.Core.Interfaces;
using Healthcare.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Healthcare.MVC.Controllers
{
    [Authorize(Roles = "User")]
    public class PatientPortalController : Controller
    {
        private readonly IUserPortalService _userPortalService;
        private readonly IUnitOfWork _unitOfWork;

        public PatientPortalController(IUserPortalService userPortalService, IUnitOfWork unitOfWork)
        {
            _userPortalService = userPortalService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Dashboard()
        {
            // Dashboard checks whether the user has already created a patient profile.
            var patient = await _userPortalService.GetPatientProfileAsync(GetCurrentUserId());
            ViewBag.HasProfile = patient != null;
            ViewBag.PatientName = patient == null ? User.Identity.Name : patient.FullName;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var patient = await _userPortalService.GetPatientProfileAsync(GetCurrentUserId());

            // If no profile exists, prefill safe default values from login claims.
            var model = patient == null
                ? new UserPatientProfileViewModel
                {
                    FullName = User.Identity.Name,
                    Email = User.FindFirstValue(ClaimTypes.Email),
                    DateOfBirth = DateTime.Today.AddYears(-18)
                }
                : new UserPatientProfileViewModel
                {
                    Id = patient.Id,
                    FullName = patient.FullName,
                    Email = patient.Email,
                    PhoneNumber = patient.PhoneNumber,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    Address = patient.Address,
                    BloodGroup = patient.BloodGroup
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserPatientProfileViewModel model)
        {
            // Server-side validation protects the database even if browser validation is bypassed.
            if (!ModelState.IsValid) return View(model);

            var patient = new Patient
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = model.Address,
                BloodGroup = model.BloodGroup
            };

            await _userPortalService.CreateOrUpdatePatientProfileAsync(GetCurrentUserId(), patient, User.Identity.Name);
            TempData["Success"] = "Patient profile saved successfully.";
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> BookAppointment()
        {
            var patient = await _userPortalService.GetPatientProfileAsync(GetCurrentUserId());
            if (patient == null)
            {
                TempData["Success"] = "Please create your patient profile before booking an appointment.";
                return RedirectToAction(nameof(Profile));
            }

            return View(await BuildBookingModel(new UserBookAppointmentViewModel
            {
                AppointmentDateTime = DateTime.Now.AddDays(1),
                DurationMinutes = 30
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(UserBookAppointmentViewModel model)
        {
            if (!ModelState.IsValid) return View(await BuildBookingModel(model));

            try
            {
                await _userPortalService.BookAppointmentForUserAsync(
                    GetCurrentUserId(),
                    model.DoctorId,
                    model.AppointmentDateTime,
                    model.DurationMinutes,
                    model.Reason,
                    User.Identity.Name);

                TempData["Success"] = "Appointment booked successfully.";
                return RedirectToAction(nameof(MyAppointments));
            }
            catch (BusinessRuleException ex)
            {
                // Shows business-rule errors such as duplicate slot or missing profile without exposing stack traces.
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(await BuildBookingModel(model));
            }
        }

        public async Task<IActionResult> MyAppointments()
        {
            // The service filters by the logged-in user's linked PatientId.
            var appointments = await _userPortalService.GetMyAppointmentsAsync(GetCurrentUserId());
            return View(appointments);
        }

        private int GetCurrentUserId()
        {
            // The login action stores user.Id in ClaimTypes.NameIdentifier.
            // This is safer than accepting user id from a hidden form field or query string.
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        private async Task<UserBookAppointmentViewModel> BuildBookingModel(UserBookAppointmentViewModel model)
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync();
            model.Doctors = doctors
                .Where(x => x.IsActive)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName + " - " + x.Specialization
                });

            return model;
        }
    }
}
