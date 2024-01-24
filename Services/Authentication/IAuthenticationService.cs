using Linkedin.Models;

namespace Linkedin.Services.Authentication
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Signs in the user by storing in cookies some details.
        /// </summary>
        void SignIn(HttpContext context, User user);

        /// <summary>
        /// Signs out the user by removing the details from cookies.
        /// </summary>
        void SignOut(HttpContext context);
    }
}
