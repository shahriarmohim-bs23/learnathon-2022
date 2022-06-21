namespace backend_task3.Models
{
    public class AuthenticatedResponse
    {
        public string? RefreshTokenExpires;
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
       
    }
}
