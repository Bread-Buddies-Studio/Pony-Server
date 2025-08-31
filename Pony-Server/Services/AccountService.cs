namespace ServerSpace.Services;

public static class AccountService
{
    public static bool TryLogIn(string userName, string passWord)
    {
        string[] lines = File.ReadAllLines("Pony-Server/Data/AccountData");
        // Iterate through all accounts // //
        foreach (string line in lines)
        {
            // Data Source //
            string[] accountData = line.Split(' ');
            // Extract Data //
            string otherUserName = accountData[0];
            string otherPassWord = accountData[1];
            // Conditions //
            if (userName != otherUserName)
                continue;
            if (passWord != otherPassWord)
                return false;
            // SUCCESS //
            return true;
        } 
        // No such account //
        return false;
    }
}