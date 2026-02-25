namespace UserPaymentSystem.Models;

public class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;

    // Внешний ключ
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}