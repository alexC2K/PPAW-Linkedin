using Linkedin.DbModels.LoginRegister;
using Linkedin.Services.Authentication;
using Linkedin.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Linkedin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IUsersService usersService, IAuthenticationService authenticationService)
        {
            _usersService = usersService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult LoginRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginUserRequest request)
        {
            var user = _usersService.LoginUser(request.Email, request.Password);

            if (user != null)
            {
                _authenticationService.SignIn(HttpContext, user);
                return RedirectToAction("Home", "Users");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View("LoginRegister");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterUserRequest request)
        {
            var existingUser = _usersService.GetUserByEmail(request.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(RegisterUserRequest.Email), "Email is already in use.");
                return View("LoginRegister");
            }

            var newUser = _usersService.RegisterUser(request.Email, request.Password, request.Name, request.PhoneNo, request.Skills);

            _authenticationService.SignIn(HttpContext, newUser);
            return RedirectToAction("Home", "Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                _authenticationService.SignOut(HttpContext);
            }

            return RedirectToAction("LoginRegister", "Account");
        }
    }
}
