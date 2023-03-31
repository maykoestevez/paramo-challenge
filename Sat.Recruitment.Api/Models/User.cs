namespace Sat.Recruitment.Api.Models
{
    /// <summary>
    /// Hold user data
    /// </summary>
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public UserType UserType { get; set; }
        public decimal Money { get; set; }
    }

    public enum UserType
    {
       SuperUser,
       Premium,
       Normal
    }
}