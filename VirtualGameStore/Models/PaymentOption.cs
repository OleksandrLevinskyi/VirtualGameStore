namespace VirtualGameStore.Models;

public class PaymentOption
{
    public int Id { get; set; }
    public User User { get; }
    public string CardNumber { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string HolderFirstName { get; set; }
    public string HolderLastName { get; set; }
    public string BillingAddress { get; set; }
}