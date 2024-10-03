﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MomoProducts.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MomoProducts.Server.Models.AuthData.ApiKey", b =>
                {
                    b.Property<string>("APIKey")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("APIKey");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.AuthData.ApiUser", b =>
                {
                    b.Property<string>("ReferenceId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProviderCallbackHost")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReferenceId");

                    b.ToTable("ApiUsers");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.Invoice", b =>
                {
                    b.Property<string>("ReferenceId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ValidityDuration")
                        .HasColumnType("int");

                    b.HasKey("ReferenceId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.Payment", b =>
                {
                    b.Property<string>("ReferenceId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("CustomerReference")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ExternalTransactionId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ServiceProviderUserName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ReferenceId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.PreApproval", b =>
                {
                    b.Property<string>("ReferenceId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PayerCurrency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PayerMessage")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ValidityTime")
                        .HasColumnType("int");

                    b.HasKey("ReferenceId");

                    b.ToTable("PreApprovals");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.RequestToWithdraw", b =>
                {
                    b.Property<string>("ReferenceId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PayeeNote")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PayerMessage")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("ReferenceId");

                    b.ToTable("RequestsToWithdraw");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.RequesttoPay", b =>
                {
                    b.Property<string>("ReferenceId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PayeeNote")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PayerMessage")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("ReferenceId");

                    b.ToTable("RequestsToPay");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Common.AccessToken", b =>
                {
                    b.Property<string>("accessToken")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("ExpiresIn")
                        .HasColumnType("datetime2");

                    b.Property<string>("TokenType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("accessToken");

                    b.ToTable("AccessTokens");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Common.Oauth2Token", b =>
                {
                    b.Property<string>("AccessToken")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresIn")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("RefreshTokenExpiredIn")
                        .HasColumnType("int");

                    b.Property<string>("Scope")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("TokenType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AccessToken");

                    b.ToTable("Oauth2Tokens");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Common.Transfer", b =>
                {
                    b.Property<string>("ExternalId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PayeeNote")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PayerMessage")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("ExternalId");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Disbursements.Deposit", b =>
                {
                    b.Property<string>("ExternalId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PayeeNote")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PayerMessage")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("ExternalId");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Disbursements.Refund", b =>
                {
                    b.Property<string>("ReferenceId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PayeeNote")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PayerMessage")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("ReferenceId");

                    b.ToTable("Refunds");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Remittance.CashTransfer", b =>
                {
                    b.Property<string>("ReferenceId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("OriginalAmount")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OriginalCurrency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("OriginatingCountry")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PayeeNote")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PayerEmail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PayerFirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PayerGender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PayerIdentificationNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PayerIdentificationType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PayerIdentity")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PayerLanguageCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PayerMessage")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PayerMsisdn")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PayerSurName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ReferenceId");

                    b.ToTable("CashTransfers");
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.Invoice", b =>
                {
                    b.OwnsOne("MomoProducts.Server.Models.Common.Payer", "IntendedPayer", b1 =>
                        {
                            b1.Property<string>("InvoiceReferenceId")
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("PartyId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("IntendedPayer_PartyId");

                            b1.Property<string>("PartyIdType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("IntendedPayer_PartyIdType");

                            b1.HasKey("InvoiceReferenceId");

                            b1.ToTable("Invoices");

                            b1.WithOwner()
                                .HasForeignKey("InvoiceReferenceId");
                        });

                    b.OwnsOne("MomoProducts.Server.Models.Common.Payee", "Payee", b1 =>
                        {
                            b1.Property<string>("InvoiceReferenceId")
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("PartyId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payee_PartyId");

                            b1.Property<string>("PartyIdType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payee_PartyIdType");

                            b1.HasKey("InvoiceReferenceId");

                            b1.ToTable("Invoices");

                            b1.WithOwner()
                                .HasForeignKey("InvoiceReferenceId");
                        });

                    b.Navigation("IntendedPayer")
                        .IsRequired();

                    b.Navigation("Payee")
                        .IsRequired();
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.Payment", b =>
                {
                    b.OwnsOne("MomoProducts.Server.Models.Common.Money", "Money", b1 =>
                        {
                            b1.Property<string>("PaymentReferenceId")
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("Amount")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Money_Amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("Money_Currency");

                            b1.HasKey("PaymentReferenceId");

                            b1.ToTable("Payments");

                            b1.WithOwner()
                                .HasForeignKey("PaymentReferenceId");
                        });

                    b.Navigation("Money")
                        .IsRequired();
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.PreApproval", b =>
                {
                    b.OwnsOne("MomoProducts.Server.Models.Common.Payer", "Payer", b1 =>
                        {
                            b1.Property<string>("PreApprovalReferenceId")
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("PartyId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payer_PartyId");

                            b1.Property<string>("PartyIdType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payer_PartyIdType");

                            b1.HasKey("PreApprovalReferenceId");

                            b1.ToTable("PreApprovals");

                            b1.WithOwner()
                                .HasForeignKey("PreApprovalReferenceId");
                        });

                    b.Navigation("Payer")
                        .IsRequired();
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.RequestToWithdraw", b =>
                {
                    b.OwnsOne("MomoProducts.Server.Models.Common.Payer", "Payer", b1 =>
                        {
                            b1.Property<string>("RequestToWithdrawReferenceId")
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("PartyId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payer_PartyId");

                            b1.Property<string>("PartyIdType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payer_PartyIdType");

                            b1.HasKey("RequestToWithdrawReferenceId");

                            b1.ToTable("RequestsToWithdraw");

                            b1.WithOwner()
                                .HasForeignKey("RequestToWithdrawReferenceId");
                        });

                    b.Navigation("Payer")
                        .IsRequired();
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Collections.RequesttoPay", b =>
                {
                    b.OwnsOne("MomoProducts.Server.Models.Common.Payer", "Payer", b1 =>
                        {
                            b1.Property<string>("RequesttoPayReferenceId")
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("PartyId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payer_PartyId");

                            b1.Property<string>("PartyIdType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payer_PartyIdType");

                            b1.HasKey("RequesttoPayReferenceId");

                            b1.ToTable("RequestsToPay");

                            b1.WithOwner()
                                .HasForeignKey("RequesttoPayReferenceId");
                        });

                    b.Navigation("Payer")
                        .IsRequired();
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Common.Transfer", b =>
                {
                    b.OwnsOne("MomoProducts.Server.Models.Common.Payee", "Payee", b1 =>
                        {
                            b1.Property<string>("TransferExternalId")
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("PartyId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payee_PartyId");

                            b1.Property<string>("PartyIdType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payee_PartyIdType");

                            b1.HasKey("TransferExternalId");

                            b1.ToTable("Transfers");

                            b1.WithOwner()
                                .HasForeignKey("TransferExternalId");
                        });

                    b.Navigation("Payee")
                        .IsRequired();
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Disbursements.Deposit", b =>
                {
                    b.OwnsOne("MomoProducts.Server.Models.Common.Payee", "Payee", b1 =>
                        {
                            b1.Property<string>("DepositExternalId")
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("PartyId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payee_PartyId");

                            b1.Property<string>("PartyIdType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payee_PartyIdType");

                            b1.HasKey("DepositExternalId");

                            b1.ToTable("Deposits");

                            b1.WithOwner()
                                .HasForeignKey("DepositExternalId");
                        });

                    b.Navigation("Payee")
                        .IsRequired();
                });

            modelBuilder.Entity("MomoProducts.Server.Models.Remittance.CashTransfer", b =>
                {
                    b.OwnsOne("MomoProducts.Server.Models.Common.Payee", "Payee", b1 =>
                        {
                            b1.Property<string>("CashTransferReferenceId")
                                .HasColumnType("nvarchar(255)");

                            b1.Property<string>("PartyId")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payee_PartyId");

                            b1.Property<string>("PartyIdType")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Payee_PartyIdType");

                            b1.HasKey("CashTransferReferenceId");

                            b1.ToTable("CashTransfers");

                            b1.WithOwner()
                                .HasForeignKey("CashTransferReferenceId");
                        });

                    b.Navigation("Payee")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
