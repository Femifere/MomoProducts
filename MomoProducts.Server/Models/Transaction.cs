namespace MomoProducts.Server.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionType { get; set; } // Transfer, Deposit, Refund
        public string ReferenceId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    
}
