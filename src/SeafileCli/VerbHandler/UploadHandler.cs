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
        private class FileFolder {
            public string File { get; set; }
            public string Folder { get; set; }
            public string Filename => Path.GetFileName(File);
        }

        private readonly UploadSubOptions _options;

        public UploadHandler(UploadSubOptions options)
        {
            _options = options;
        }

        private async Task RunAsync()
        {
            var session = await _options.GetSession();
            var library = await session.GetLibrary(_options.Library);

            var allFiles = CreateFilepathList(_options.Files).Select(n => new FileFolder {
                File = n
            }).ToList();
            if (_options.PreserveFolders)
            {
                string commonBase = CalculateCommonBaseFolder(allFiles.Select(n => n.File));
                foreach (var file in allFiles)
                {
                    string folder = Path.GetDirectoryName(file.File);
                    if (!string.IsNullOrEmpty(commonBase))
                    {
                        folder = folder.Substring(commonBase.Length);
                    }
                    file.Folder = $"{_options.Directory}/{folder}";
                }
            }
            else
            {
                foreach (var file in allFiles)
                {
                    file.Folder = _options.Directory;
                }
            }

            // Create folders
            IEnumerable<string> folders = allFiles.Select(n => n.Folder).Distinct();
            Console.WriteLine($"Creating {folders.Count()} folder(s)...");
            foreach (string folder in folders)
            {
                await session.CreateDirectoryWithParents(library, folder);
            }

            // Upload files
            foreach (var file in allFiles)
            {
                try
                {
                    using (var stream = File.Open(file.File, FileMode.Open, FileAccess.Read))
                    {
                        await session.UploadSingle(library, file.Folder, file.Filename, stream, progress => { });
                    }
                    Console.WriteLine($"Successfully uploaded file: {file.File}");
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to upload file: {file.File}");
                }
            }
        }

        public void Run()
        {
            RunAsync().Wait();
        }

        private string CalculateCommonBaseFolder(IEnumerable<string> files)
        {
            IEnumerable<string> folders = files.Select(n => Path.GetDirectoryName(n)).Distinct();
            string commonBase = folders.OrderBy(n => n.Length).FirstOrDefault();
            if (commonBase != null)
            {
                while (commonBase.Length > 0 && folders.Any(n => !n.StartsWith(commonBase)))
                {
                    commonBase = commonBase.Substring(0, commonBase.Length - 1);
                }
            }

            return commonBase;
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