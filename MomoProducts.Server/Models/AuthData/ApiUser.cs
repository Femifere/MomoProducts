namespace MomoProducts.Server.Models.AuthData
{
    public class ApiUser
    {
        public string ReferenceId { get; set; }
        public string ProviderCallbackHost { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
