namespace MomoProducts.Server.Dtos.RemittanceDto
{
    using MomoProducts.Server.Dtos.CommonDto;

    public class TransferDto
    {
        public string Amount { get; set; }

        public string Currency { get; set; }

        public string ExternalId { get; set; }

        public PayeeDto Payee { get; set; }

        public string PayerMessage { get; set; }

        public string PayeeNote { get; set; }
    }

}