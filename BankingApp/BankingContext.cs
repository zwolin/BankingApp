using System;
using System.IO;
using BankingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApp;

public class BankingContext : DbContext
{
    private string DbPath { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Account> Accounts { get; set; }

    public BankingContext()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "banking.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
        optionsBuilder.LogTo(Console.WriteLine);
    }
}