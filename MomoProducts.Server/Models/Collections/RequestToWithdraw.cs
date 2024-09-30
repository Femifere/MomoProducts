namespace MomoProducts.Server.Models.Collections
{
    using MomoProducts.Server.Models.Common;

    public class RequestToWithdraw
    {
        public string ReferenceId { get; set; }

        public string PayeeNote { get; set; }

        public string ExternalId { get; set; }

        public string Amount { get; set; }

        public string Currency { get; set; }

        public Payer Payer { get; set; }

        public string PayerMessage { get; set; }
    }
}
