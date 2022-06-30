namespace backend_task3.Models
{
    public class Refreshtoken
    {

        public string Token { get; set; } = string.Empty;

        public DateTime? Expires { get; set; } = null;
    }
}
