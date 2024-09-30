namespace MomoProducts.Server.Dtos.DisbursementsDto
{
    using MomoProducts.Server.Dtos.CommonDto;

    public class DepositDto
    {
        public string ReferenceId { get; set; }

        public string Amount { get; set; }

        public string Currency { get; set; }

        public string ExternalId { get; set; }

        public PayeeDto Payee { get; set; }

        public string PayerMessage { get; set; }

        public string PayeeNote { get; set; }
    }
}
