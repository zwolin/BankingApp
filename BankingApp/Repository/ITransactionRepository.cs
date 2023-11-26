using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingApp.Models;

namespace BankingApp.Repository;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetUserTransactions(int userId, int accountId);
    Task MakeTransaction(int userId, int accountId, decimal amount, DateTime dateTime, TransactionDirection direction);
    Task<decimal> GetBalance(int userId, int accountId);
    Task<int> CreateAccount(int userId, string name);
    Task<int> CreateUser(string name, string lastName);
}