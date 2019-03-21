using CommandLine;

namespace SeafileCli.Argparse
{
    [Verb("account-info", HelpText = "Retrieves the user account information.")]
    public class AccountInfoOptions : AuthorizationOptions
    {
    }
}