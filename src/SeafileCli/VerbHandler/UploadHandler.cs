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
            List<string> result = new List<string>();

            foreach (var file in _options.Files)
            {
                string fileName = Path.GetFileName(file);
                string pathName = Path.GetDirectoryName(file);
                string searchBase = Directory.GetCurrentDirectory();
                if (Path.IsPathRooted(file))
                {
                    searchBase = Path.GetPathRoot(file);
                    pathName = pathName.Substring(searchBase.Length);
                }

                IEnumerable<string> directoryList = ResolvePaths(searchBase, pathName);
                foreach (string dir in directoryList)
                {
                    result.AddRange(Directory.GetFiles(dir, fileName, SearchOption.TopDirectoryOnly));
                }
            }

            return result.Distinct();
        }

        private IEnumerable<string> ResolvePaths(string root, string pathName)
        {
            List<string> result = new List<string>();
            List<string> parts = pathName.Split(Path.DirectorySeparatorChar).ToList();
            string currentPart = parts.ElementAt(0);

            string[] matchedDirectories;
            if (currentPart.Equals("**"))
            {
                // '**' matches any directory
                matchedDirectories = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);
            }
            else if (currentPart.Contains("*") || currentPart.Contains("?"))
            {
                // Use default matching for '*' and '?' wildcards
                matchedDirectories = Directory.GetDirectories(root, currentPart, SearchOption.TopDirectoryOnly);
            }
            else
            {
                matchedDirectories = new[] {
                    Path.Combine(root, currentPart)
                };
            }

            if (parts.Count > 1)
            {
                string remainingPath = string.Join(Path.DirectorySeparatorChar.ToString(), parts.GetRange(1, parts.Count - 1).ToArray());
                foreach (string dir in matchedDirectories)
                {
                    result.AddRange(ResolvePaths(dir, remainingPath));
                }
            }
            else
            {
                result.AddRange(matchedDirectories);
            }

            return result.Distinct();
        }
    }
}