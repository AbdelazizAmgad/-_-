using CinemaProject.Models;
using CinemaProject.Repositories;
using CinemaProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaProject.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Repository<ApplicationUserOtp> _applicationUserOtpRepository;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            Repository<ApplicationUserOtp> applicationUserOtpRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _applicationUserOtpRepository = applicationUserOtpRepository;
        }

        // ================= 1. Register =================
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            ApplicationUser user = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                UserName = registerVM.Email,
                Email = registerVM.Email,
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                return View(registerVM);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // اللينك بدون تشفير زي كود الانستراكتور
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", id = user.Id, token }, Request.Scheme);

            await _emailSender.SendEmailAsync(
                registerVM.Email,
                "Cinema Confirmation Email",
                $"<h1> please click <a href='{link}'> here </a>to confirm Your Mail</h1>"
            );

            TempData["Successful_Notification"] = "Registration Successfully";
            return RedirectToAction(nameof(Login));
        }

        // ================= 2. Confirm Email =================
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                TempData["Error_Notification"] = "Invalid Confirmation Email";
                return RedirectToAction(nameof(Login));
            }

            TempData["Successful_Notification"] = "Confirm email Successfully";
            return RedirectToAction(nameof(Login));
        }

        // ================= 3. Login =================
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail) ??
                       await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Email/Username or Password");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut) ModelState.AddModelError("", "You are Locked, please try again later");
                else if (result.IsNotAllowed) ModelState.AddModelError("", "Please confirm Your Email First");
                else ModelState.AddModelError("", "Invalid Email/Username or Password");

                return View(loginVM);
            }

            TempData["Successful_Notification"] = "Login Successfully";
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        // ================= 4. Logout =================
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        // ================= 5. Resend Confirmation =================
        [HttpGet]
        public IActionResult ResendEmailConfirmation() => View();

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail) ??
                       await _userManager.FindByNameAsync(model.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Email/Username");
                return View(model);
            }
            if (user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Email is already confirmed");
                return View(model);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", id = user.Id, token }, Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Cinema Confirmation Email",
                $"<h1> please click <a href='{link}'> here </a>to confirm Your Mail</h1>"
            );

            TempData["Successful_Notification"] = "Confirmation Email Resent";
            return RedirectToAction(nameof(Login));
        }

        // ================= 6. Forget Password =================
        [HttpGet]
        public IActionResult ForgetPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail) ??
                       await _userManager.FindByNameAsync(model.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Email/Username");
                return View(model);
            }

            var otp = new Random().Next(1000, 9999).ToString(); 
            var applicationUserOtp = new ApplicationUserOtp(otp, user.Id);

            await _applicationUserOtpRepository.AddAsync(applicationUserOtp);
            await _applicationUserOtpRepository.SaveChangesAsync();

            await _emailSender.SendEmailAsync(
                user.Email,
                "Cinema Reset Password",
                $"<h1> use this {otp} to Reset Your Password</h1>"
            );

            TempData["Successful_Notification"] = "OTP Sent to your email";
            return RedirectToAction(nameof(VerifyOtp), new { userId = user.Id });
        }

        // ================= 7. Verify OTP =================
        [HttpGet]
        public IActionResult VerifyOtp(string userId)
        {
            return View(new VerifyOtpVM() { UserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtpVM verifyOtpVM)
        {
            var user = await _userManager.FindByIdAsync(verifyOtpVM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid user");
                return View(verifyOtpVM);
            }

            var applicationUserOtps = await _applicationUserOtpRepository.FindAsync(a =>
                a.ApplicationUserId == user.Id &&
                a.IsValid &&
                a.ValidTo >= DateTime.UtcNow
            );

            var applicationUserOtp = applicationUserOtps.OrderBy(a => a.CreatedAt).LastOrDefault();

            if (applicationUserOtp == null || applicationUserOtp.OTP != verifyOtpVM.OTP)
            {
                ModelState.AddModelError("", "Invalid or Expired OTP");
                return View(verifyOtpVM);
            }

            applicationUserOtp.IsValid = false;
            await _applicationUserOtpRepository.SaveChangesAsync();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return RedirectToAction(nameof(ResetPassword), new { userId = user.Id, token });
        }

        // ================= 8. Reset Password =================
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPasswordVM() { UserId = userId, Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid) return View(resetPasswordVM);

            var user = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid user");
                return View(resetPasswordVM);
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                return View(resetPasswordVM);
            }

            TempData["Successful_Notification"] = "Password Reset Successfully";
            return RedirectToAction(nameof(Login));
        }
    }
}