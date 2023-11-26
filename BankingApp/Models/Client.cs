using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Models;

public class Client
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }

    public List<Account> Accounts { get; set; } = new();
}