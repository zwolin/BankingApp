using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingApp.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly ILogger<TransactionRepository> logger;

    public TransactionRepository(ILogger<TransactionRepository> logger)
    {
        this.logger = logger;
    }
    
    public async Task<IEnumerable<Transaction>> GetUserTransactions(int userId, int accountId)
    {
        logger.LogTrace("Getting transaction history for user: {User} in account {Account}", userId, accountId);
        await using var db = new BankingContext();
        
        var found = db.Transactions.Where(x => x.UserId == userId && x.Id == accountId);
        logger.LogDebug("Found {Count} records for user: {User} in account {Account}", found.Count(), userId, accountId);
        return found.ToList();
    }

    public async Task MakeTransaction(int userId, int accountId, decimal amount, DateTime dateTime, TransactionDirection direction)
    {
        logger.LogTrace("Making transaction for user: {User} on account {Account}, amount: {Amount}", userId, accountId, amount);
        await using var db = new BankingContext();

        await using var dbTransaction = await db.Database.BeginTransactionAsync();
        var account = db.Accounts.FirstOrDefault(x => x.Id == accountId);
        if (account == null)
        {
            await dbTransaction.RollbackAsync();
            logger.LogError("User's {UserId} account {AccountId} not exists", userId, accountId);
            throw new AccountNotExistsException($"User's {userId} account {accountId} not exists");
        }
            
        db.Entry(account).State = EntityState.Modified;
            
        account.Balance += direction == TransactionDirection.In ? amount : amount * -1;
        account.Transactions.Add(new Transaction()
            {Date = dateTime, Direction = direction, UserId = userId, Amount = amount, CurrentBalance = account.Balance});
        
        try
        {
            await db.SaveChangesAsync();
            await dbTransaction.CommitAsync();
            logger.LogTrace("Transaction for user: {User} on account {Account}, amount: {Amount} successful", userId, accountId, amount);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Cannot save transaction");
            await dbTransaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<decimal> GetBalance(int userId, int accountId)
    {
        logger.LogTrace("Getting balance for user: {User} in account {Account}", userId, accountId);
        await using var db = new BankingContext();
        var account = db.Accounts.FirstOrDefault(x => x.UserId == userId && x.Id == accountId);
        if (account == null)
        {
            throw new AccountNotExistsException($"User's {userId} account {accountId} not exists");
        }
        return account.Balance;
    }
    
    public async Task<int> CreateAccount(int userId, string name)
    {
        await using var db = new BankingContext();
        var client = db.Clients.FirstOrDefault(x => x.Id == userId);
        if (client == null)
        {
            throw new UserNotExistsException($"User with id: {userId} not exists");
        }

        var newAccount = new Account() {UserId = client.Id, Name = name}; 
        client.Accounts.Add(newAccount);
        
        await db.SaveChangesAsync();
        return newAccount.Id;
    }

    public async Task<int> CreateUser(string name, string lastName)
    {
        await using var db = new BankingContext();
        var x = db.Clients.Add(new Client() {Name = name, LastName = lastName});
        await db.SaveChangesAsync();
        return x.Entity.Id;
    }
}