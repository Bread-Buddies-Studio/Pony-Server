using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using Network.Enums;
using Storage.Exceptions;
using Storage.Services;

namespace Network.Services;
// Exceptions //
[Serializable]
public class UsernameInvalidFormat(string message = "Username invalid format") : Exception(message) { }
[Serializable]
public class PasswordInvalidFormat(string message = "Password invalid format") : Exception(message) { }
[Serializable]
public class AccountExistsException(string message = "Account already exists") : Exception(message) { }
[Serializable]
public class AccountNotFoundException(string message = "Account not found") : Exception(message) { }
[Serializable]
public class InvalidTaglineException(string message = "Invalid tagline") : Exception(message) { }
[Serializable]
public class IncorrectPasswordException(string message = "Incorrect Password") : Exception(message) { }
// Service //
public static class AccountService
{
    // Base Methods //
    public static void SignUp(string accountData)
    {
        // Conditions //
        if (!byte.TryParse(accountData[0..3], out _))
            throw new InvalidTaglineException();
        // Try //
        try
        {
            StorageService.Store(accountData);
        }

        catch(AlreadyStoredException)
        {
            throw new AccountExistsException();
        }
    }
    public static void LogIn(string accountData)
    {
        // Data Source //
        ReadOnlySpan<string> data = accountData.Split('|').AsSpan();
        // Conditions //
        if (!byte.TryParse(data[0], out _))
            throw new InvalidTaglineException();

        if (!StorageService.Exists(accountData))
            throw new AccountNotFoundException();
    }
}