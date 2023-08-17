namespace BartenderIntegration.Infrastructure.Models
{
    public class JwtHandler
    {
        public string Key { get; set; }
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
        public string Url { get; set; }
    }

    public class TokenRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthorizedModel
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
