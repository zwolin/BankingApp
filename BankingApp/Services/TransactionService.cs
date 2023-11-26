using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.DTOs;
using BankingApp.Repository;
using Microsoft.Extensions.Logging;

namespace BankingApp.Services;

public class TransactionService : ITransactionService
{
    private readonly ILogger<TransactionService> logger;
    private readonly ITransactionRepository repository;

    public TransactionService(ILogger<TransactionService> logger, ITransactionRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task<IEnumerable<TransactionResponse>> GetUserTransactions(int userId, int accountId)
    {
        var transactions = (await repository.GetUserTransactions(userId, accountId)).ToList();
        

        var transactionResponses = transactions.Select(x =>
            new TransactionResponse {Amount = x.Amount, Date = x.Date, Direction = x.Direction});
        
        return transactionResponses;
    }

    public async Task<TransactionResponse> Deposit(TransactionRequest transaction)
    {
        var dateTime = DateTime.UtcNow;
        await repository.MakeTransaction(transaction.UserId, transaction.AccountId, transaction.Amount, dateTime,
            TransactionDirection.In);
        
        return new TransactionResponse
        {
            Amount = transaction.Amount, Direction = TransactionDirection.In, Date = dateTime,
            AccountId = transaction.AccountId, UserId = transaction.UserId
        };
    }

    public async Task<TransactionResponse> Withdrawal(TransactionRequest transaction)
    {
        var dateTime = DateTime.UtcNow;
        await repository.MakeTransaction(transaction.UserId, transaction.AccountId, transaction.Amount, dateTime,
            TransactionDirection.Out);

        return new TransactionResponse
        {
            Amount = transaction.Amount, Direction = TransactionDirection.Out, Date = dateTime,
            AccountId = transaction.AccountId, UserId = transaction.UserId
        };
    }

    public async Task<int> CreateAccount(int userId, string name)
    {
        return await repository.CreateAccount(userId, name);
    }
    
    public async Task<int> CreateUser(string name, string lastName)
    {
        return await repository.CreateUser(name, lastName);
    }

    public async Task<decimal> GetBalance(int userId, int accountId)
    {
        try
        {
            return await repository.GetBalance(userId, accountId);
        }
        catch (AccountNotExistsException)
        {
            logger.LogError("User: {User} has no account with id: {Account}", userId, accountId);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError("Error occured");
            throw;
        }
    }
}