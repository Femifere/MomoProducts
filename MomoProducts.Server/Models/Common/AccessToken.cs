namespace MomoProducts.Server.Models.Common
{
    public class AccessToken
    {
        public string accessToken { get; set; }

        public string TokenType { get; set; }

        public DateTime ExpiresIn { get; set; }
    }
}
