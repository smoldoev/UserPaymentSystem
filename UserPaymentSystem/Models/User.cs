namespace UserPaymentSystem.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Связь с платежами (один пользователь - много платежей)
    public List<Payment> Payments { get; set; } = new();

    // Добавь это свойство для отображения полного имени в ComboBox
    public string FullName => $"{FirstName} {LastName}";
}