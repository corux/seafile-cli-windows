using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SeafileCli.Argparse;

namespace SeafileCli.VerbHandler
{
    /// <summary>
    /// Handles the uploading of files.
    /// </summary>
    public class UploadHandler : IVerbHandler
    {
        private readonly UploadSubOptions _options;

        public UploadHandler(UploadSubOptions options)
        {
            _options = options;
        }

        private async Task RunAsync()
        {
            var session = await _options.GetSession();
            var library = await session.GetLibrary(_options.Library);
            await session.CreateDirectoryWithParents(library, _options.Directory);

            IEnumerable<string> allfiles = CreateFilepathList(_options.Files);

            foreach (var file in allfiles)
            {
                try
                {
                    using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
                    {
                        await session.UploadSingle(library, _options.Directory, Path.GetFileName(file),
                            stream, progress => { });
                    }
                    Console.WriteLine($"Successfully uploaded file: {file} ");
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to upload file: {file}");
                }
            }
        }

        public void Run()
        {
            RunAsync().Wait();
        }


        private IEnumerable<string> CreateFilepathList(string[] fileNames)
        {
            string[] allfiles = fileNames;
            IEnumerable<string> calcFiles = new List<string>();
            string directory = Directory.GetCurrentDirectory();

            foreach (var file in _options.Files)
            {
                if (file.Contains("*")) // handle wildcard
                {
                    if (file.Contains(@"\")) // check if directory path is included
                    {
                        directory = Path.GetDirectoryName(file);
                    }
                    
                    var fileName = Path.GetFileName(file); // e.g. *.zip
                    calcFiles = calcFiles.Concat(Directory.GetFiles(directory, fileName, SearchOption.TopDirectoryOnly));
                }
            }

            if (calcFiles.Count() > 0)
            {
                allfiles = calcFiles.Distinct().ToArray(); // rm duplicated files
            }

            return allfiles;
        }
    }
}