using Microsoft.EntityFrameworkCore;
using MomoProducts.Server.Models.AuthData;
using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Models.Disbursements;
using MomoProducts.Server.Models.Remittance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

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

    public DbSet<Oauth2Token> Oauth2Tokens { get; set; }

    public DbSet<Transfer> Transfers { get; set; }

    // Remittance
    public DbSet<CashTransfer> CashTransfers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ApiKey configuration
        modelBuilder.Entity<ApiKey>(entity =>
        {
            entity.HasKey(e => e.APIKey);
            entity.Property(e => e.APIKey)
                  .IsRequired()
                  .HasMaxLength(255);
        });

        // ApiUser configuration
        modelBuilder.Entity<ApiUser>(entity =>
        {
            entity.HasKey(e => e.ReferenceId);
            entity.Property(e => e.ReferenceId)
                  .IsRequired()
                  .HasMaxLength(255);
        });

        // Invoice configuration
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(i => i.ReferenceId);

            entity.Property(i => i.ReferenceId)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(i => i.ExternalId)
                  .HasMaxLength(255);

            entity.Property(i => i.Amount)
                  .HasColumnType("decimal(18,2)");

            entity.Property(i => i.Currency)
                  .HasMaxLength(10);

            entity.Property(i => i.Status)
                  .HasMaxLength(50);

            entity.Property(i => i.ValidityDuration);

            entity.Property(i => i.Description)
                  .HasMaxLength(500);

            // Value objects configuration
            entity.OwnsOne(i => i.IntendedPayer, payer =>
            {
                payer.Property(p => p.PartyId)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("IntendedPayer_PartyId");

                payer.Property(p => p.PartyIdType)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("IntendedPayer_PartyIdType");
            });

            entity.OwnsOne(i => i.Payee, payee =>
            {
                payee.Property(p => p.PartyId)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payee_PartyId");

                payee.Property(p => p.PartyIdType)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payee_PartyIdType");
            });
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.ReferenceId);

            entity.Property(p => p.ReferenceId)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(p => p.ExternalTransactionId)
                  .HasMaxLength(255);

            entity.Property(p => p.CustomerReference)
                  .HasMaxLength(255);

            entity.Property(p => p.ServiceProviderUserName)
                  .HasMaxLength(255);

            // Value object configuration
            entity.OwnsOne(p => p.Money, money =>
            {
                money.Property(m => m.Amount)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Money_Amount");

                money.Property(m => m.Currency)
                     .IsRequired()
                     .HasMaxLength(10)
                     .HasColumnName("Money_Currency");
            });
        });

        // PreApproval configuration
        modelBuilder.Entity<PreApproval>(entity =>
        {
            entity.HasKey(p => p.ReferenceId);

            entity.Property(p => p.ReferenceId)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(p => p.Amount)
                  .HasColumnType("decimal(18,2)");

            entity.Property(p => p.PayerCurrency)
                  .HasMaxLength(10);

            entity.Property(p => p.Status)
                  .HasMaxLength(50);

            entity.Property(p => p.PayerMessage)
                  .HasMaxLength(500);

            entity.Property(p => p.ValidityTime);

            // Value object configuration
            entity.OwnsOne(p => p.Payer, payer =>
            {
                payer.Property(p => p.PartyId)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payer_PartyId");

                payer.Property(p => p.PartyIdType)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payer_PartyIdType");
            });
        });

        // RequesttoPay configuration
        modelBuilder.Entity<RequesttoPay>(entity =>
        {
            entity.HasKey(r => r.ExternalId);

            

            entity.Property(r => r.Amount)
                  .HasMaxLength(50);

            entity.Property(r => r.Currency)
                  .HasMaxLength(10);

            entity.Property(r => r.ExternalId)
                  .HasMaxLength(255);

            entity.Property(r => r.PayerMessage)
                  .HasMaxLength(500);

            entity.Property(r => r.PayeeNote)
                  .HasMaxLength(500);

            // Value object configuration
            entity.OwnsOne(r => r.Payer, payer =>
            {
                payer.Property(p => p.PartyId)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payer_PartyId");

                payer.Property(p => p.PartyIdType)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payer_PartyIdType");
            });
        });

        // RequestToWithdraw configuration
        modelBuilder.Entity<RequestToWithdraw>(entity =>
        {
            entity.HasKey(r => r.ReferenceId);

            entity.Property(r => r.ReferenceId)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(r => r.PayeeNote)
                  .HasMaxLength(500);

            entity.Property(r => r.ExternalId)
                  .HasMaxLength(255);

            entity.Property(r => r.Amount)
                  .HasMaxLength(50);

            entity.Property(r => r.Currency)
                  .HasMaxLength(10);

            entity.Property(r => r.PayerMessage)
                  .HasMaxLength(500);

            // Value object configuration
            entity.OwnsOne(r => r.Payer, payer =>
            {
                payer.Property(p => p.PartyId)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payer_PartyId");

                payer.Property(p => p.PartyIdType)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payer_PartyIdType");
            });
        });

        // Deposit configuration
        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(d => d.ExternalId);

            entity.Property(d => d.Amount)
                  .HasMaxLength(50);

            entity.Property(d => d.Currency)
                  .HasMaxLength(10);

            entity.Property(d => d.ExternalId)
                  .HasMaxLength(255);

            entity.Property(d => d.PayerMessage)
                  .HasMaxLength(500);

            entity.Property(d => d.PayeeNote)
                  .HasMaxLength(500);

            // Value object configuration
            entity.OwnsOne(d => d.Payee, payee =>
            {
                payee.Property(p => p.PartyId)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payee_PartyId");

                payee.Property(p => p.PartyIdType)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payee_PartyIdType");
            });
        });

        // Refund configuration
        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(r => r.ReferenceId);

            entity.Property(r => r.ReferenceId)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(r => r.Amount)
                  .HasMaxLength(50);

            entity.Property(r => r.Currency)
                  .HasMaxLength(10);

            entity.Property(r => r.ExternalId)
                  .HasMaxLength(255);

            entity.Property(r => r.PayerMessage)
                  .HasMaxLength(500);

            entity.Property(r => r.PayeeNote)
                  .HasMaxLength(500);
        });

        // Transfer configuration
        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(t => t.ExternalId);

            entity.Property(t => t.Amount)
                  .HasMaxLength(50);

            entity.Property(t => t.Currency)
                  .HasMaxLength(10);

            entity.Property(t => t.ExternalId)
                  .HasMaxLength(255);

            entity.Property(t => t.PayerMessage)
                  .HasMaxLength(500);

            entity.Property(t => t.PayeeNote)
                  .HasMaxLength(500);

            // Value object configuration
            entity.OwnsOne(t => t.Payee, payee =>
            {
                payee.Property(p => p.PartyId)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payee_PartyId");

                payee.Property(p => p.PartyIdType)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payee_PartyIdType");
            });
        });

        // CashTransfer configuration
        modelBuilder.Entity<CashTransfer>(entity =>
        {
            entity.HasKey(c => c.ReferenceId);

            entity.Property(c => c.ReferenceId)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(c => c.Amount)
                  .HasMaxLength(50);

            entity.Property(c => c.Currency)
                  .HasMaxLength(10);

            entity.Property(c => c.ExternalId)
                  .HasMaxLength(255);

            entity.Property(c => c.OriginatingCountry)
                  .HasMaxLength(50);

            entity.Property(c => c.OriginalAmount)
                  .HasMaxLength(50);

            entity.Property(c => c.OriginalCurrency)
                  .HasMaxLength(10);

            entity.Property(c => c.PayerMessage)
                  .HasMaxLength(500);

            entity.Property(c => c.PayeeNote)
                  .HasMaxLength(500);

            entity.Property(c => c.PayerIdentificationType)
                  .HasMaxLength(50);

            entity.Property(c => c.PayerIdentificationNumber)
                  .HasMaxLength(50);

            entity.Property(c => c.PayerIdentity)
                  .HasMaxLength(100);

            entity.Property(c => c.PayerFirstName)
                  .HasMaxLength(100);

            entity.Property(c => c.PayerSurName)
                  .HasMaxLength(100);

            entity.Property(c => c.PayerLanguageCode)
                  .HasMaxLength(10);

            entity.Property(c => c.PayerEmail)
                  .HasMaxLength(255);

            entity.Property(c => c.PayerMsisdn)
                  .HasMaxLength(20);

            entity.Property(c => c.PayerGender)
                  .HasMaxLength(10);

            // Value object configuration
            entity.OwnsOne(c => c.Payee, payee =>
            {
                payee.Property(p => p.PartyId)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payee_PartyId");

                payee.Property(p => p.PartyIdType)
                     .IsRequired()
                     .HasMaxLength(50)
                     .HasColumnName("Payee_PartyIdType");
            });
        });

        // AccessToken configuration
        modelBuilder.Entity<AccessToken>(entity =>
        {
            // Define the primary key
            entity.HasKey(a => a.accessToken);

            // Configure properties
            entity.Property(a => a.accessToken)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(a => a.TokenType)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(a => a.ExpiresIn)
                  .IsRequired(); // If ExpiresIn is DateTime, this is fine.
        });

        // Oauth2Token configuration
        modelBuilder.Entity<Oauth2Token>(entity =>
        {
            entity.HasKey(o => o.AccessToken);

            entity.Property(o => o.AccessToken)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(o => o.TokenType)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(o => o.ExpiresIn)
                  .IsRequired();

            entity.Property(o => o.Scope)
                  .HasMaxLength(500);

            entity.Property(o => o.RefreshToken)
                  .HasMaxLength(500);

            entity.Property(o => o.RefreshTokenExpiredIn)
                  .IsRequired();
        });
    }
}
