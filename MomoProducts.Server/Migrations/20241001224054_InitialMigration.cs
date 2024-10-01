using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomoProducts.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessTokens",
                columns: table => new
                {
                    accessToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TokenType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiresIn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessTokens", x => x.accessToken);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    APIKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.APIKey);
                });

            migrationBuilder.CreateTable(
                name: "ApiUsers",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProviderCallbackHost = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsers", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "CashTransfers",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Payee_PartyIdType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payee_PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OriginatingCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OriginalAmount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OriginalCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PayerMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PayeeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PayerIdentificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayerIdentificationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayerIdentity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PayerFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PayerSurName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PayerLanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PayerEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PayerMsisdn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PayerGender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTransfers", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "Deposits",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Payee_PartyIdType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payee_PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayerMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PayeeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValidityDuration = table.Column<int>(type: "int", nullable: false),
                    IntendedPayer_PartyIdType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IntendedPayer_PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payee_PartyIdType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payee_PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "Oauth2Tokens",
                columns: table => new
                {
                    AccessToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TokenType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiresIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RefreshTokenExpiredIn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oauth2Tokens", x => x.AccessToken);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExternalTransactionId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Money_Amount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Money_Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CustomerReference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ServiceProviderUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "PreApprovals",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Payer_PartyIdType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payer_PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayerCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayerMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ValidityTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreApprovals", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PayerMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PayeeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "RequestsToPay",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Payer_PartyIdType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payer_PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayerMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PayeeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestsToPay", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "RequestsToWithdraw",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PayeeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Payer_PartyIdType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payer_PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayerMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestsToWithdraw", x => x.ReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    ReferenceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Payee_PartyIdType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payee_PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayerMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PayeeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.ReferenceId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessTokens");

            migrationBuilder.DropTable(
                name: "ApiKeys");

            migrationBuilder.DropTable(
                name: "ApiUsers");

            migrationBuilder.DropTable(
                name: "CashTransfers");

            migrationBuilder.DropTable(
                name: "Deposits");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Oauth2Tokens");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PreApprovals");

            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropTable(
                name: "RequestsToPay");

            migrationBuilder.DropTable(
                name: "RequestsToWithdraw");

            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
