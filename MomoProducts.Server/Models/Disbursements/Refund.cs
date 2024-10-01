namespace MomoProducts.Server.Models.Disbursements
{
    public class Refund
    {
 
        public string Amount { get; set; }

        public string Currency { get; set; }

        public string ExternalId { get; set; }

        public string PayerMessage { get; set; }

        public string PayeeNote { get; set; }

        public string ReferenceId { get; set; }
    }
}
