using System;

namespace BankingApp;

public class UserNotExistsException : Exception
{
    public UserNotExistsException(string message) : base(message)
    {
        
    }
}