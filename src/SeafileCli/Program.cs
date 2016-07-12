using System;
using CommandLine;
using SeafileCli.Argparse;
using SeafileCli.VerbHandler;

namespace SeafileCli
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            string invokedVerb = null;
            object invokedVerbInstance = null;
            var options = new Options();

            var parser = new Parser(settings => { settings.HelpWriter = Console.Out; });
            if (!parser.ParseArguments(args, options, (verb, subOptions) =>
            {
                invokedVerb = verb;
                invokedVerbInstance = subOptions;
            }))
            {
                Environment.Exit(Parser.DefaultExitCodeFail);
            }

            switch (invokedVerb)
            {
                case "token":
                    new TokenHandler((TokenSubOptions) invokedVerbInstance).Run();
                    break;
                case "upload":
                    new UploadHandler((UploadSubOptions) invokedVerbInstance).Run();
                    break;
                default:
                    Environment.Exit(Parser.DefaultExitCodeFail);
                    break;
            }
        }
    }
}