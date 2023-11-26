using System.Threading.Tasks;
using BankingApp.Controllers;
using BankingApp.DTOs;
using BankingApp.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankingApp.Tests;

public class AccountControllerTest
{
    private readonly Mock<ITransactionService> transactionServiceMock = new();
    private readonly Mock<ILogger<AccountController>> loggerMock = new();
    
    private readonly AccountController target;

    private const int ExistingUserId = 12;
    private const int ExistingAccountId = 3;
    private const decimal Amount = 123;
    
    
    public AccountControllerTest()
    {
        target = new AccountController(loggerMock.Object, transactionServiceMock.Object);
    }
    
    [Fact]
    public async Task GetBalance_CallsServiceWithCorrectValues()
    {
        const decimal balance = 300;
        transactionServiceMock
            .Setup(x => x.GetBalance(ExistingUserId, ExistingAccountId))
            .ReturnsAsync(balance);

        var result = await target.GetBalance(ExistingUserId, ExistingAccountId);
        transactionServiceMock.Verify(x=>x.GetBalance(ExistingUserId, ExistingAccountId));
        
        Assert.Equal(balance, result.Value);
    }

    [Fact]
    public async Task Deposit_CallsServiceWithCorrectValues()
    {
        var transactionRequest = new TransactionRequest()
            {AccountId = ExistingAccountId, UserId = ExistingUserId, Amount = Amount};
        var result = await target.Deposit(transactionRequest);
        
        transactionServiceMock.Verify(x=>x.Deposit(transactionRequest));
        if (result.Value != null) Assert.Equal(Amount, result.Value.Amount);
    }
}