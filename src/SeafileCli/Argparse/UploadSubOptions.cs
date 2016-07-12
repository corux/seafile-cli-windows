using CommandLine;

namespace SeafileCli.Argparse
{
    public class UploadSubOptions : CommonOptions
    {
        [Option('l', "library", HelpText = "The name of the remote library, where the files should be uploaded to.", Required = true)]
        public string Library { get; set; }

        [Option('d', "directory", HelpText = "The remote directory, where the files should be uploaded to. Path seperator is the forward slash (/).", DefaultValue = "/")]
        public string Directory { get; set; }

        [OptionArray('f', "files", HelpText = "The files to upload.", Required = true)]
        public string[] Files { get; set; }
    }
}