using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearn.Models;
using Elearn.ViewModels.Account;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace Elearn.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            AppUser newUser = new AppUser
            {
                UserName = model.Username,
                FullName = model.FullName,
                Email = model.Email
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            //await _signInManager.SignInAsync(newUser, false);

            //return RedirectToAction("Index", "Home");

            TempData["Email"] = newUser.Email;

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            string link = Url.Action("ConfirmEmail", "Account", new {userId = newUser.Id, token}, Request.Scheme, Request.Host.ToString());

            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("aliit@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(newUser.Email));
            email.Subject = "Testing";
            email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Hi, Ali</h1>" };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("aliit@code.edu.az", "kymxjpejiumjxeyc");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction(nameof(VerifyEmail));
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            AppUser user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);

            if (user is null)
            {
                user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
            }

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Email or password is wrong.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Please, verify your email.");
                return View(model);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or password is wrong.");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}