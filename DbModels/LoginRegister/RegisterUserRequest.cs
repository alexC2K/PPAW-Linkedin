namespace Linkedin.DbModels.LoginRegister
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Skills { get; set; }
    }
}
