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

            if (!Parser.Default.ParseArguments(args, options, (verb, subOptions) =>
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
                    new TokenHandler((CommonOptions) invokedVerbInstance).Run();
                    break;
                case "upload":
                    new UploadHandler((UploadSubOptions) invokedVerbInstance).Run();
                    break;
                case "account-info":
                    new AccountInfoHandler((CommonOptions) invokedVerbInstance).Run();
                    break;
                default:
                    Environment.Exit(Parser.DefaultExitCodeFail);
                    break;
            }
        }
    }
}