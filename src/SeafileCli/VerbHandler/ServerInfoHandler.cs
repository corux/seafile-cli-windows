using System;
using SeafClient;
using SeafileCli.Argparse;

namespace SeafileCli.VerbHandler
{
    /// <summary>
    /// Retrieves the server information and prints it on the console.
    /// </summary>
    public class ServerInfoHandler : IVerbHandler
    {
        private readonly CommonOptions _options;

        public ServerInfoHandler(CommonOptions options)
        {
            _options = options;
        }

        public void Run()
        {
            var serverInfo = SeafSession.GetServerInfo(_options.ServerUri).Result;
            Console.WriteLine($"Version: {serverInfo.Version}");
            Console.WriteLine("Features:");
            foreach (var feature in serverInfo.Features)
            {
                Console.WriteLine($"- {feature}");
            }
        }
    }
}
