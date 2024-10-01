namespace MomoProducts.Server.Dtos.CollectionsDto
{
    using MomoProducts.Server.Dtos.CommonDto;

    public class InvoiceDto
    {
        public string ReferenceId { get; set; }

        public string ExternalId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
        public string Status { get; set; }

        public int ValidityDuration { get; set; }

        public PayerDto IntendedPayer { get; set; }

        public PayeeDto Payee { get; set; }

        public string Description { get; set; }
    }
}
