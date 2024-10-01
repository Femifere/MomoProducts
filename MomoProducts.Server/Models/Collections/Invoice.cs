namespace MomoProducts.Server.Models.Collections
{
    using MomoProducts.Server.Models.Common;

    public class Invoice
    {
        public string ReferenceId { get; set; }

        public string ExternalId { get; set; }

        public decimal Amount { get; set; }

        public string  Currency { get; set; }
        public string Status { get; set; }

        public int  ValidityDuration { get; set; }

        public Payer IntendedPayer { get; set; }

        public Payee Payee { get; set; }

        public string Description { get; set; }
    }

}
