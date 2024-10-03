namespace MomoProducts.Server.Models.Common
{
    public class Oauth2Token
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public DateTime ExpiresIn { get; set; }

        public string Scope { get; set; }

        public string RefreshToken { get; set; }

        public int RefreshTokenExpiredIn { get; set; }

        public DateTime CreatedAt { get; internal set; }
    }
}
