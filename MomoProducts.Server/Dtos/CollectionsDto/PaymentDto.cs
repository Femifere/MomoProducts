namespace MomoProducts.Server.Dtos.CollectionsDto
{
    using MomoProducts.Server.Dtos.CommonDto;

    public class PaymentDto
    {
        public string ReferenceId { get; set; }

        public string ExternalTransactionId { get; set; }

        public MoneyDto Money { get; set; }

        public string CustomerReference { get; set; }

        public string ServiceProviderUserName { get; set; }
    }
}
