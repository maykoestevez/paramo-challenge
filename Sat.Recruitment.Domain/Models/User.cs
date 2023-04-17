using System;

namespace Sat.Recruitment.Domain.Models
{
    /// <summary>
    /// Hold user structure and business logic
    /// </summary>
    public class User
    {
        public User(string name, string email, string address, string phone)
        {
            if (IsUserInvalid(name, email, address, phone)) throw new ArgumentException("Required fields are null or empty");

            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
            
            CalculateUserGif();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public UserType UserType { get; set; }
        public decimal Money { get; set; }


        public bool IsUserInvalid(string name, string email, string address, string phone) =>
            (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) ||
             string.IsNullOrEmpty(address));

        public void CalculateUserGif()
        {
            var gif = 0.0M;
            switch (UserType)
            {
                case UserType.Normal:
                    if (Money > 100) gif = Money * 0.12M;
                    if (Money <= 100 && Money > 10) gif = Money * 0.8M;
                    break;
                case UserType.SuperUser:
                    if (Money > 100) gif = Money * 0.20M;
                    break;
                case UserType.Premium:
                    if (Money > 100) gif = Money * 2M;
                    break;
            }

            Money += gif;
        }
    }
}