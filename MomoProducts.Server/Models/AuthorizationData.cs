namespace MomoProducts.Server.Models
{
    public class AuthorizationData
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiry { get; set; }
        public string TokenType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
