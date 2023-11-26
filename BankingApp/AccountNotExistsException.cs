using System;

namespace BankingApp;

public class AccountNotExistsException : Exception
{
    public AccountNotExistsException(string message) : base(message)
    {
        
    }
}