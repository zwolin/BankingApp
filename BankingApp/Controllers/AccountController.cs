using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.DTOs;
using BankingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankingApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> logger;
    private readonly ITransactionService transactionService;


    public AccountController(ILogger<AccountController> logger,
        ITransactionService transactionService)
    {
        this.logger = logger;
        this.transactionService = transactionService;
    }

    [HttpGet(Name = "GetUserTransaction")]
    public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetTransactions(int userId, int accountId)
    {
        logger.LogInformation("Getting transactions for user: {User}", userId);
        try
        {
            var result = (await transactionService.GetUserTransactions(userId, accountId)).ToList();
            if (result.Any())
                return result;

            return NotFound();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured");
            throw;
        }
    }

    [HttpGet("Balance")]
    public async Task<ActionResult<decimal>> GetBalance(int userId, int accountId)
    {
        try
        {
            var balance = await transactionService.GetBalance(userId, accountId);
            return balance;
        }
        catch (AccountNotExistsException ex)
        {
            return NotFound(ex);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured");
            throw;
        }
    }

    [HttpPost("Deposit")]
    public async Task<ActionResult<TransactionResponse>> Deposit(TransactionRequest transaction)
    {
        try
        {
            var deposit = await transactionService.Deposit(transaction);
            return CreatedAtAction(nameof(Deposit), deposit);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured");
            throw;
        }
    }

    [HttpPost("Withdrawal")]
    public async Task<ActionResult<TransactionResponse>> Withdrawal(TransactionRequest transaction)
    {
        try
        {
            var withdrawal = await transactionService.Withdrawal(transaction);
            return CreatedAtAction(nameof(Deposit), withdrawal);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured");
            throw;
        }
    }

    [HttpPost("CreateAccount")]
    public async Task<ActionResult<int>> CreateAccount(int userId, string name)
    {
        try
        {
            return await transactionService.CreateAccount(userId, name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult<int>> CreateUser(string name, string lastName)
    {
        try
        {
            return await transactionService.CreateUser(name, lastName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}