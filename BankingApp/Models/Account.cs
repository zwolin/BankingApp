using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Models;

public class Account
{
    [Key] public int Id { get; set; }
    public int UserId { get; set; } = default;
    public string Name { get; set; }
    public int Number { get; set; }
    public decimal Balance { get; set; } = default;
    [Timestamp] public byte[]? RowVersion { get; set; } = Array.Empty<byte>();
    public List<Transaction> Transactions { get; set; } = new();
}