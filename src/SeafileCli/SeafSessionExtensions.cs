using System;
using System.Linq;
using System.Threading.Tasks;
using SeafClient;
using SeafClient.Types;

namespace SeafileCli
{
    public static class SeafSessionExtensions
    {
        public static async Task<bool> ExistsDirectory(this SeafSession session, SeafLibrary library, string path)
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

        public static async Task CreateDirectory(this SeafSession session, SeafLibrary library, string path)
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

            var dirsToCreate = path.Substring(currentPath.Length)
                .Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            if (dirsToCreate.Length > 0)
            {
                foreach (string dir in dirsToCreate)
                {
                    currentPath = $"{currentPath}/{dir}";
                    await session.CreateDirectory(library, currentPath);
                }
            }
        }

        public static async Task<SeafLibrary> GetLibrary(this SeafSession session, string libraryName)
        {
            var libs = await session.ListLibraries();
            return libs.SingleOrDefault(n => n.Name == libraryName);
        }
    }
}