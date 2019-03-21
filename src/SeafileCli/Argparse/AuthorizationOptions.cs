using System.Threading.Tasks;
using CommandLine;
using SeafClient;

namespace SeafileCli.Argparse
{
    public abstract class AuthorizationOptions : CommonOptions
    {
        [Option('u', "username", HelpText = "The username used for authorization.", SetName = "PasswordAuth", Required = true)]
        public string Username { get; set; }

        [Option('p', "password", HelpText = "The password used for authorization.", SetName = "PasswordAuth", Required = true)]
        public string Password { get; set; }

        [Option('t', "token",
            HelpText = "The API Token used for authorization. If the token is used, username and password are not required.",
            SetName = "TokenAuth", Required = true)]
        public string Token { get; set; }

        public async Task<SeafSession> GetSession()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                return await SeafSession.FromToken(ServerUri, Token);
            }

            return await SeafSession.Establish(ServerUri, Username, Password.ToCharArray());
        }
    }
}