namespace BankingApp.DTOs;

public class TransactionRequest
{
    public int UserId { get; init; }
    public int AccountId { get; init; }
    public decimal Amount { get; init; }
}