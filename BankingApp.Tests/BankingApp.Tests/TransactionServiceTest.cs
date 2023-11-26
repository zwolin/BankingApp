using System;
using System.Threading.Tasks;
using BankingApp.DTOs;
using BankingApp.Repository;
using BankingApp.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankingApp.Tests;

public class TransactionServiceTest
{
    private readonly Mock<ILogger<TransactionService>> loggerMock = new();
    private readonly Mock<ITransactionRepository> transactionRepositoryMock = new();
    private readonly ITransactionService target;
    
    private const int ExistingUserId = 12;
    private const int ExistingAccountId = 3;
    private const decimal Amount1 = 300;
    private readonly TransactionRequest transaction;

    public TransactionServiceTest()
    {
        transaction = new TransactionRequest {UserId = ExistingUserId, AccountId = ExistingAccountId, Amount = Amount1};
        
        target = new TransactionService(loggerMock.Object, transactionRepositoryMock.Object);
    }

    [Fact]
    public async Task Deposit_MakesDepositWithCorrectValues()
    {
        var result = await target.Deposit(transaction);
        transactionRepositoryMock.Verify(x=>x.MakeTransaction(ExistingUserId, ExistingAccountId, Amount1, It.IsAny<DateTime>(), TransactionDirection.In));
        
        Assert.Equal(Amount1, result.Amount);
        Assert.Equal(ExistingAccountId, result.AccountId);
        Assert.Equal(ExistingUserId, result.UserId);
    }
    
    [Fact]
    public async Task Withdrawal_MakesDepositWithCorrectValues()
    {
        var result = await target.Withdrawal(transaction);
        
        transactionRepositoryMock.Verify(x=>x.MakeTransaction(ExistingUserId, ExistingAccountId, Amount1, It.IsAny<DateTime>(), TransactionDirection.Out));
        
        Assert.Equal(Amount1, result.Amount);
        Assert.Equal(ExistingAccountId, result.AccountId);
        Assert.Equal(ExistingUserId, result.UserId);
    }

    [Fact]
    public async Task GetBalance_CallsRepositoryWithCorrectDataAndReturnsCorrectValue()
    {
        transactionRepositoryMock.Setup(x => x.GetBalance(ExistingUserId, ExistingAccountId)).ReturnsAsync(Amount1);
        
        var balance = await target.GetBalance(ExistingUserId, ExistingAccountId);
        
        transactionRepositoryMock.Verify(x=>x.GetBalance(ExistingUserId, ExistingAccountId));
        Assert.Equal(Amount1, balance);
    }
}