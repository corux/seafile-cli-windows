using System;
using System.Threading.Tasks;
using CommandLine;
using SeafClient;

namespace SeafileCli.Argparse
{
    public class CommonOptions
    {
        [Option('s', "server", HelpText = "URL to the server. e.g. https://seafile.example.com/", Required = true)]
        public string Server { get; set; }

        [Option('u', "username", HelpText = "The username used for authorization.")]
        public string Username { get; set; }

        [Option('p', "password", HelpText = "The password used for authorization.")]
        public string Password { get; set; }

        [Option('t', "token", HelpText = "The API Token used for authorization. If the token is used, username and password are not required.")]
        public string Token { get; set; }

        public Uri ServerUri => new Uri(Server);

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