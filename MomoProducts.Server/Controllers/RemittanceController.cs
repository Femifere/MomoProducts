using Microsoft.AspNetCore.Mvc;
using MomoProducts.Server.Interfaces.Remittance;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Models.Remittance;
using MomoProducts.Server.Models.Common;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

[Route("api/[controller]")]
[ApiController]
public class RemittanceController : ControllerBase
{
    private readonly ICashTransferRepository _cashTransferRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public RemittanceController(ICashTransferRepository cashTransferRepository, ITransferRepository transferRepository, HttpClient httpClient, IConfiguration config)
    {
        _cashTransferRepository = cashTransferRepository;
        _transferRepository = transferRepository;
        _httpClient = httpClient;
        _config = config;
    }

    // BC Authorize
    [HttpPost("bc-authorize")]
    public async Task<IActionResult> AuthorizeBusinessClient([FromBody] AuthorizeRequest authorizeRequest)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:bc-authorize"]}";
        var response = await _httpClient.PostAsJsonAsync(url, authorizeRequest);
        if (response.IsSuccessStatusCode)
        {
            return Ok(await response.Content.ReadAsStringAsync());
        }
        return BadRequest("Authorization failed.");
    }

    // Cash Transfer
    [HttpPost("cash-transfer")]
    public async Task<IActionResult> CreateCashTransfer([FromBody] CashTransfer cashTransfer)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:CashTransfer"]}";
        var response = await _httpClient.PostAsJsonAsync(url, cashTransfer);

        if (response.IsSuccessStatusCode)
        {
            await _cashTransferRepository.CreateCashTransferAsync(cashTransfer);
            return Ok("Cash transfer created successfully.");
        }
        return BadRequest("Failed to create cash transfer.");
    }

    // Get Cash Transfer Status
    [HttpGet("cash-transfer-status/{referenceId}")]
    public async Task<IActionResult> GetCashTransferStatus(string referenceId)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:GetCashTransferStatus"].Replace("{referenceId}", referenceId)}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var status = await response.Content.ReadAsStringAsync();
            return Ok(status);
        }
        return NotFound("Cash transfer status not found.");
    }

    // Create Access Token
    [HttpPost("create-access-token")]
    public async Task<IActionResult> CreateAccessToken([FromBody] TokenRequest tokenRequest)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:CreateAccessToken"]}";
        var response = await _httpClient.PostAsJsonAsync(url, tokenRequest);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            return Ok(token);
        }
        return BadRequest("Failed to create access token.");
    }

    // Create Oauth2 Token
    [HttpPost("create-oauth2-token")]
    public async Task<IActionResult> CreateOauth2Token([FromBody] TokenRequest tokenRequest)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:CreateOauth2Token"]}";
        var response = await _httpClient.PostAsJsonAsync(url, tokenRequest);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            return Ok(token);
        }
        return BadRequest("Failed to create Oauth2 token.");
    }

    // Get Account Balance
    [HttpGet("account-balance")]
    public async Task<IActionResult> GetAccountBalance()
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:GetAccountBalance"]}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var balance = await response.Content.ReadAsStringAsync();
            return Ok(balance);
        }
        return NotFound("Account balance not found.");
    }

    // Get Account Balance in Specific Currency
    [HttpGet("account-balance/{currency}")]
    public async Task<IActionResult> GetAccountBalanceInSpecificCurrency(string currency)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:GetAccountBalanceInSpecificCurrency"].Replace("{Currency}", currency)}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var balance = await response.Content.ReadAsStringAsync();
            return Ok(balance);
        }
        return NotFound("Account balance not found for the specified currency.");
    }

    // Get Basic User Info
    [HttpGet("basic-userinfo/{accountHolderMSISDN}")]
    public async Task<IActionResult> GetBasicUserInfo(string accountHolderMSISDN)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:GetBasicUserinfo"].Replace("{accountHolderMSISDN}", accountHolderMSISDN)}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var userInfo = await response.Content.ReadAsStringAsync();
            return Ok(userInfo);
        }
        return NotFound("User info not found.");
    }

    // Validate Account Holder Status
    [HttpGet("validate-account/{accountHolderIdType}/{accountHolderId}")]
    public async Task<IActionResult> ValidateAccountHolderStatus(string accountHolderIdType, string accountHolderId)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:ValidateAccountHolderStatus"].Replace("{accountHolderIdType}", accountHolderIdType).Replace("{accountHolderId}", accountHolderId)}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var status = await response.Content.ReadAsStringAsync();
            return Ok(status);
        }
        return NotFound("Account holder status not found.");
    }

    // Transfer
    [HttpPost("transfer")]
    public async Task<IActionResult> CreateTransfer([FromBody] Transfer transfer)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:Transfer"]}";
        var response = await _httpClient.PostAsJsonAsync(url, transfer);

        if (response.IsSuccessStatusCode)
        {
            await _transferRepository.CreateTransferAsync(transfer);
            return Ok("Transfer created successfully.");
        }
        return BadRequest("Failed to create transfer.");
    }

    // Get Transfer Status
    [HttpGet("transfer-status/{referenceId}")]
    public async Task<IActionResult> GetTransferStatus(string referenceId)
    {
        var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:GetTransferStatus"].Replace("{referenceId}", referenceId)}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var status = await response.Content.ReadAsStringAsync();
            return Ok(status);
        }
        return NotFound("Transfer status not found.");
    }
}
