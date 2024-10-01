using MomoProducts.Server.Models.Common;
namespace MomoProducts.Server.Models.Remittance
{
    public class CashTransfer
    {
        public string ReferenceId { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public Payee Payee { get; set; }
        public string ExternalId { get; set; }
        public string OriginatingCountry { get; set; }
        public string OriginalAmount { get; set; }
        public string OriginalCurrency { get; set; }
        public string PayerMessage { get; set; }
        public string PayeeNote { get; set; }
        public string PayerIdentificationType { get; set; }
        public string PayerIdentificationNumber { get; set; }
        public string PayerIdentity { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerSurName { get; set; }
        public string PayerLanguageCode { get; set; }
        public string PayerEmail { get; set; }
        public string PayerMsisdn { get; set; }
        public string PayerGender { get; set; }
    }
}