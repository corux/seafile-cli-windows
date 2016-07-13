using CommandLine;
using CommandLine.Text;

namespace SeafileCli.Argparse
{
    /// <summary>
    /// Contains the CLI Arguments.
    /// </summary>
    public class Options
    {
        public Options()
        {
            TokenVerb = new AuthorizationOptions();
            AccountInfoVerb = new AuthorizationOptions();
            ServerInfoVerb = new CommonOptions();
            UploadVerb = new UploadSubOptions();
        }

        [VerbOption("token", HelpText = "Retrieves the authorization token with the provided username and password.")]
        public AuthorizationOptions TokenVerb { get; set; }

        [VerbOption("account-info", HelpText = "Retrieves the user account information.")]
        public AuthorizationOptions AccountInfoVerb { get; set; }

        [VerbOption("server-info", HelpText = "Retrieves the server information.")]
        public CommonOptions ServerInfoVerb { get; set; }

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