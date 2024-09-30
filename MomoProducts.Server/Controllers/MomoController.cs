using Microsoft.AspNetCore.Mvc;
using MomoProducts.Server.Models;

namespace MomoProducts.Server.Controllers
{
    [ApiController]
    [Route("api/momo")]
    public class MomoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public MomoController(AppDbContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        // Method to get Momo Access Token
        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize()
        {
            var baseUrl = _configuration["MomoApi:BaseUrl"];
            var requestUrl = baseUrl + _configuration["MomoApi:Collections:CreateAccessToken"];

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            // Set headers and other necessary info
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var authData = await response.Content.ReadFromJsonAsync<AuthorizationData>();
                authData.CreatedAt = DateTime.UtcNow;
                authData.Expiry = DateTime.UtcNow.AddMinutes(60); // Assuming 1-hour token expiration

                _context.AuthorizationData.Add(authData);
                await _context.SaveChangesAsync();

                return Ok(authData);
            }

            return BadRequest("Authorization failed.");
        }

        // Method to record a transaction (Transfer, Deposit, Refund)
        [HttpPost("transaction")]
        public async Task<IActionResult> RecordTransaction([FromBody] Transaction transaction)
        {
            var baseUrl = _configuration["MomoApi:BaseUrl"];
            var requestUrl = string.Empty;

            switch (transaction.TransactionType)
            {
                case "Transfer":
                    requestUrl = baseUrl + _configuration["MomoApi:Disbursments:Transfer"];
                    break;
                case "Deposit":
                    requestUrl = baseUrl + _configuration["MomoApi:Disbursments:Deposit-V1"];
                    break;
                case "Refund":
                    requestUrl = baseUrl + _configuration["MomoApi:Disbursments:Refund-V1"];
                    break;
                default:
                    return BadRequest("Invalid transaction type.");
            }

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            // Set headers, token, and body (you can fetch stored tokens from AuthorizationData)
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                transaction.TransactionDate = DateTime.UtcNow;
                transaction.Status = "Success";

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return Ok(transaction);
            }

            return BadRequest("Transaction failed.");
        }
    }
}
