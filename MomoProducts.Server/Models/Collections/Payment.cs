namespace MomoProducts.Server.Models.Collections
{
    using MomoProducts.Server.Models.Common;

    public class Payment
    {
        public string ReferenceId { get; set; }

        public string ExternalTransactionId { get; set; }

        public Money Money { get; set; }

        public string CustomerReference { get; set; }

        public string ServiceProviderUserName { get; set; }
    }

}
