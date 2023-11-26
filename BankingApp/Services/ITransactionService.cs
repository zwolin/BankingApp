using System.Collections.Generic;
using System.Threading.Tasks;
using BankingApp.DTOs;

namespace BankingApp.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionResponse>> GetUserTransactions(int userId, int accountId);
    Task<TransactionResponse> Deposit(TransactionRequest transaction);
    Task<TransactionResponse> Withdrawal(TransactionRequest transaction);
    Task<int> CreateAccount(int userId, string name);
    Task<decimal> GetBalance(int userId, int accountId);
    Task<int> CreateUser(string name, string lastName);
}