using System;
using System.IO;
using System.Threading.Tasks;
using SeafileCli.Argparse;

namespace SeafileCli.VerbHandler
{
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
            await session.CreateDirectory(library, _options.Directory);

            foreach (var file in _options.Files)
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
    }
}