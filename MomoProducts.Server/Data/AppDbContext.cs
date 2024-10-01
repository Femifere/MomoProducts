using Microsoft.EntityFrameworkCore;
using MomoProducts.Server.Models;
using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Models.Disbursements;
using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Models.AuthData;
using MomoProducts.Server.Models.Remittance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<ApiUser> ApiUsers { get; set; }

    // Collections
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PreApproval> PreApprovals { get; set; }
    public DbSet<RequesttoPay> RequestsToPay { get; set; }
    public DbSet<RequestToWithdraw> RequestsToWithdraw { get; set; }

    // Disbursements
    public DbSet<Deposit> Deposits { get; set; }
    public DbSet<Refund> Refunds { get; set; }
    

    // Common
    public DbSet<AccessToken> AccessTokens { get; set; }
    public DbSet<Money> Money { get; set; }
    public DbSet<Oauth2Token> Oauth2Tokens { get; set; }
    public DbSet<Payee> Payees { get; set; }
    public DbSet<Payer> Payers { get; set; }

    public DbSet<Transfer> Transfers { get; set; }

    // Remittance
    public DbSet<CashTransfer> CashTransfers { get; set; }
}
