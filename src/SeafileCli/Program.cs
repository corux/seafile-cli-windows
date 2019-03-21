using System;
using CommandLine;
using SeafileCli.Argparse;
using SeafileCli.VerbHandler;

namespace SeafileCli
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<AccountInfoOptions, CommonOptions, TokenOptions, UploadSubOptions>(args)
                .WithParsed<AccountInfoOptions>(n => new AccountInfoHandler(n).Run())
                .WithParsed<TokenOptions>(n => new TokenHandler(n).Run())
                .WithParsed<UploadSubOptions>(n => new UploadHandler(n).Run())
                .WithParsed<CommonOptions>(n => new ServerInfoHandler(n).Run())
                .WithNotParsed(errors => Environment.Exit(1));
        }
    }
}