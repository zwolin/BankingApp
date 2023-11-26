using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Models;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    public TransactionDirection Direction { get; init; }
    public decimal Amount { get; init; }
    public decimal CurrentBalance { get; set; }
    public DateTime Date { get; init; }
    public int UserId { get; init; }
}