using System;
using SeafileCli.Argparse;

namespace SeafileCli.VerbHandler
{
    /// <summary>
    /// Retrieves the account information and prints it on the console.
    /// </summary>
    public class AccountInfoHandler : IVerbHandler
    {
        private readonly AuthorizationOptions _options;

        public AccountInfoHandler(AuthorizationOptions options)
        {
            _options = options;
        }

        public void Run()
        {
            var session = _options.GetSession().Result;
            var accountInfo = session.CheckAccountInfo().Result;
            var quota = accountInfo.HasUnlimitedSpace ? "unlimited" : accountInfo.Quota + " Bytes";
            Console.WriteLine($"Email: {accountInfo.Email}");
            Console.WriteLine($"Nickname: {accountInfo.Nickname}");
            Console.WriteLine($"Quota: {quota}");
            Console.WriteLine($"Usage: {accountInfo.Usage} Bytes");
        }
    }
}
