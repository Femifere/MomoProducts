namespace MomoProducts.Server.Models.Disbursements
{
    using MomoProducts.Server.Models.Common;

    public class Deposit
    {
        

        public string Amount { get; set; }

        public string Currency { get; set; }

        public string ExternalId { get; set; }

        public Payee Payee { get; set; }

        public string PayerMessage { get; set; }

        public string PayeeNote { get; set; }
    }
}
