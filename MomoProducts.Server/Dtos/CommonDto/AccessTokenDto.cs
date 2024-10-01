namespace MomoProducts.Server.Dtos.CommonDto
{
    public class AccessTokenDto
    {
        public string accessToken { get; set; }

        public string TokenType { get; set; }

        public DateTime ExpiresIn { get; set; }
    }
}
