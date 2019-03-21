using System;
using CommandLine;

namespace SeafileCli.Argparse
{
    [Verb("server-info", HelpText = "Retrieves the server information.")]
    public class CommonOptions
    {
        [Option('s', "server", HelpText = "URL to the server. e.g. https://seafile.example.com/", Required = true)]
        public string Server { get; set; }

        public Uri ServerUri => new Uri(Server);
    }
}