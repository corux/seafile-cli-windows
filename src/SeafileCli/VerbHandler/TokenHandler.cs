using System;
using SeafileCli.Argparse;

namespace SeafileCli.VerbHandler
{
    /// <summary>
    /// Retrieves the seafile API token and prints it on the console.
    /// </summary>
    public class TokenHandler : IVerbHandler
    {
        private readonly CommonOptions _options;

        public TokenHandler(CommonOptions options)
        {
            _options = options;
        }

        public void Run()
        {
            var session = _options.GetSession().Result;
            Console.WriteLine(session.AuthToken);
        }
    }
}