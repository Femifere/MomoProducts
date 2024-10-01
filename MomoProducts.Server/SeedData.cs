using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Models.Disbursements;
using MomoProducts.Server.Models.Remittance;
using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Models.AuthData;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        // Seed ApiKeys
        if (!context.ApiKeys.Any())
        {
            var apiKeys = new List<ApiKey>
            {
                new ApiKey { APIKey = "YOUR_API_KEY_1" },
                new ApiKey { APIKey = "YOUR_API_KEY_2" }
            };
            context.ApiKeys.AddRange(apiKeys);
        }

        // Seed ApiUsers
        if (!context.ApiUsers.Any())
        {
            var apiUsers = new List<ApiUser>
            {
                new ApiUser { ReferenceId = "USER_1" },
                new ApiUser { ReferenceId = "USER_2" }
            };
            context.ApiUsers.AddRange(apiUsers);
        }

        // Seed Invoices
        if (!context.Invoices.Any())
        {
            var invoices = new List<Invoice>
            {
                new Invoice
                {
                    ReferenceId = "INV001",
                    ExternalId = "EXT001",
                    Amount = 100.00m,
                    Currency = "USD",
                    Status = "Pending",
                    ValidityDuration = 30,
                    IntendedPayer = new Payer { PartyIdType = "MSISDN", PartyId = "123456789" },
                    Payee = new Payee { PartyIdType = "MSISDN", PartyId = "987654321" },
                    Description = "Invoice for payment"
                }
            };
            context.Invoices.AddRange(invoices);
        }

        // Add more seeding for other entities as needed

        context.SaveChanges();
    }
}
