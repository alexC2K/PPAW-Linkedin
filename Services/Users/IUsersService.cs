using Linkedin.Models;

namespace Linkedin.Services.Users
{
    public interface IUsersService
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        List<User> GetAllUsers();

        /// <summary>
        /// Returns a user based on its email.
        /// </summary>
        User GetUserByEmail(string email);

        /// <summary>
        /// Returns a user based on its id.
        /// </summary>
        User GetUserById(int userId);

        /// <summary>
        /// Registers a user.
        /// </summary>
        User RegisterUser(string email, string password, string name, string phoneNo, string skills);
        
        /// <summary>
        /// Logs in a user based on email and password.
        /// </summary>
        User LoginUser(string email, string password);

        /// <summary>
        /// Gets all companies owned by a recruiter.
        /// </summary>
        List<CompanyViewModel> GetRecruiterCompanies(int recruiterId);

        /// <summary>
        /// Gets all jobs listed by a recruiter.
        /// </summary>
        List<Job> GetRecruiterJobs(int recruiterId);
    }
}
