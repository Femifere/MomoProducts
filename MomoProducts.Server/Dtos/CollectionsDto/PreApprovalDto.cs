namespace MomoProducts.Server.Dtos.CollectionsDto
{
    using MomoProducts.Server.Dtos.CommonDto;

    public class PreApprovalDto
    {
        public string ReferenceId { get; set; }

        public PayerDto Payer { get; set; }

        public string PayerCurrency { get; set; }

        public string PayerMessage { get; set; }

        public int ValidityTime { get; set; }
    }
}
