﻿namespace MomoProducts.Server.Dtos.CommonDto
{
    public class AccessTokenDto
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public DateTime ExpiresIn { get; set; }
    }
}
