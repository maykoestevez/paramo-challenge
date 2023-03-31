namespace Sat.Recruitment.Api.Models
{
    /// <summary>
    /// Manage results from the application
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result <T>
    {
        public string Errors { get; set; }
        public bool IsSuccessful { get; set; }
        public T DataResult { get; set; }
    }
}