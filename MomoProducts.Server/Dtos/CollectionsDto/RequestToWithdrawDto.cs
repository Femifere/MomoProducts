namespace MomoProducts.Server.Dtos.CollectionsDto
{
    using MomoProducts.Server.Dtos.CommonDto;

    public class RequestToWithdrawDto
    {
        public string ReferenceId { get; set; }

        public string PayeeNote { get; set; }

        public string ExternalId { get; set; }

        public string Amount { get; set; }

        public string Currency { get; set; }

        public PayerDto Payer { get; set; }

        public string PayerMessage { get; set; }
    }
}
