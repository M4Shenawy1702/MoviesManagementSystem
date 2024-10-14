namespace MoviesManagementSystem.Core.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }

    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        BankTransfer
    }
}
