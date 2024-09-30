namespace MomoProducts.Server.Dtos.CommonDto
{
    public class Oauth2TokenDto
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public int ExpiresIn { get; set; }

        public string Scope { get; set; }

        public string RefreshToken { get; set; }

        public int RefreshTokenExpiredIn { get; set; }
    }
}
