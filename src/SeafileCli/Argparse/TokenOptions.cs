using CommandLine;

namespace SeafileCli.Argparse
{
    [Verb("token", HelpText = "Retrieves the authorization token with the provided username and password.")]
    public class TokenOptions : AuthorizationOptions
    {
    }
}