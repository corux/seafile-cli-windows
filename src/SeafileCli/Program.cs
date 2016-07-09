using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using SeafClient;
using SeafClient.Types;

namespace SeafileCli
{
    static class Program
    {
        static void Main(string[] args)
        {
            string invokedVerb = null;
            object invokedVerbInstance = null;
            var options = new Options();

            var parser = new Parser(settings =>
            {
                settings.HelpWriter = Console.Out;
            });
            if (!parser.ParseArguments(args, options, (verb, subOptions) =>
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
                    HandleVerbToken((TokenSubOptions)invokedVerbInstance).Wait();
                    break;
                case "upload":
                    HandleVerbUpload((UploadSubOptions)invokedVerbInstance).Wait();
                    break;
            }
        }

        private static async Task HandleVerbUpload(UploadSubOptions options)
        {
            var session = await GetSession(options);
            var library = await GetLibrary(session, options.Library);
            await CreateDirectory(session, library, options.Directory);

            foreach (var file in options.Files)
            {
                try
                {
                    using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
                    {
                        await session.UploadSingle(library, options.Directory, Path.GetFileName(file), stream, progress => { });
                    }
                    Console.WriteLine("Successfully uploaded file: {0} ", file);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to upload file: {0}", file);
                }
            }
        }

        private static async Task<SeafSession> GetSession(CommonOptions options)
        {
            if (!string.IsNullOrEmpty(options.Token))
            {
                return await SeafSession.FromToken(options.ServerUri, options.Token);
            }

            return await SeafSession.Establish(options.ServerUri, options.Username, options.Password.ToCharArray());
        }

        private static async Task HandleVerbToken(TokenSubOptions options)
        {
            var session = await GetSession(options);
            Console.WriteLine(session.AuthToken);
        }

        private static async Task<bool> ExistsDirectory(SeafSession session, SeafLibrary library, string path)
        {
            try
            {
                var dirs = await session.ListDirectory(library, path);
                return dirs != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task CreateDirectory(SeafSession session, SeafLibrary library, string path)
        {
            var currentPath = path;
            for (int i = 0; i <= path.Count(n => n == '/'); i++)
            {
                if (await ExistsDirectory(session, library, currentPath))
                {
                    break;
                }

                if (currentPath.Contains('/'))
                {
                    currentPath = currentPath.Substring(0, currentPath.LastIndexOf('/'));
                }
                else
                {
                    currentPath = string.Empty;
                }
            }

            var dirsToCreate = path.Substring(currentPath.Length).Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (dirsToCreate.Length > 0)
            {
                for (int i = 0; i < dirsToCreate.Length; i++)
                {
                    currentPath = string.Format("{0}/{1}", currentPath, dirsToCreate[i]);
                    await session.CreateDirectory(library, currentPath);
                }
            }
        }

        private static async Task<SeafLibrary> GetLibrary(SeafSession session, string libraryName)
        {
            var libs = await session.ListLibraries();
            return libs.SingleOrDefault(n => n.Name == libraryName);
        }
    }
}
