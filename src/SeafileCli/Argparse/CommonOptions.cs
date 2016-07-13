using System;
using CommandLine;

namespace SeafileCli.Argparse
{
    public class CommonOptions
    {
        [Option('s', "server", HelpText = "URL to the server. e.g. https://seafile.example.com/", Required = true)]
        public string Server { get; set; }

        public Uri ServerUri => new Uri(Server);
    }
}