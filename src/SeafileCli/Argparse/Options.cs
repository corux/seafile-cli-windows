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