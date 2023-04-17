namespace Sat.Recruitment.Infrastructure.Helpers
{
    /// <summary>
    /// Manage commons errors in the application
    /// </summary>
    public static class Messages
    {
        public const string FileNotFoundError = "File not found with path";
        public const string UserTypeError = "Valid UserType should be defined";
        public const string DuplicatedUser = "User is duplicated with name";
        public const string NameRequired = "The name is required";
        public const string AddressRequired = "The address is required";
        public const string PhoneRequired = "The phone is required";
        public const string InvalidEmail = "Invalid email";
        public const string UserShouldNotBeNull = "User should not be null";

    }
}