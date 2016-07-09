using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;

namespace SeafileCli
{
    public class TokenSubOptions : CommonOptions
    {
    }

    public class UploadSubOptions : CommonOptions
    {
        [Option('l', "library", HelpText = "The name of the remote library, where the files should be uploaded to.", Required = true)]
        public string Library { get; set; }

        [Option('d', "directory", HelpText = "The remote directory, where the files should be uploaded to. Path seperator is the forward slash (/).", DefaultValue = "/")]
        public string Directory { get; set; }

        [OptionArray('f', "files", HelpText = "The files to upload.", Required = true)]
        public string[] Files { get; set; }
    }

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

        public Uri ServerUri
        {
            get
            {
                return new Uri(Server);
            }
        }
    }

    /// <summary>
    /// Contains the CLI Arguments.
    /// </summary>
    public class Options
    {
        public Options()
        {
            TokenVerb = new TokenSubOptions();
            UploadVerb = new UploadSubOptions();
        }

        [VerbOption("token", HelpText = "Retrieves the authorization token with the provided username and password.")]
        public TokenSubOptions TokenVerb { get; set; }

        [VerbOption("upload", HelpText = "Uploads files and folders to seafile.")]
        public UploadSubOptions UploadVerb { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}