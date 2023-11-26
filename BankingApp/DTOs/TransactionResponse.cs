using System;

namespace BankingApp.DTOs;

public class TransactionResponse
{
    public decimal Amount { get; init; }
    public DateTime Date { get; set; }
    public TransactionDirection Direction { get; set; }
    public int AccountId { get; init; }
    public int UserId { get; init; }
}