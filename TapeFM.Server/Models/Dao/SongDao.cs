using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TapeFM.Server.Code;

namespace TapeFM.Server.Models.Dao
{
    public static class SongDao
    {
        private static readonly string[] ValidExtensions = {".mp3", ".m4a", ".flac", ".ogg"};
        private static readonly ICacheEntry<List<Song>> CachedAllSongs =
            Database.CreateEntry(Database.CacheKeySongs, GetAllUncached);

        public static List<Song> GetAll()
        {
            return CachedAllSongs.Get();
        }

        private static List<Song> GetAllUncached()
        {
            var paths = new List<string>();
            AddDirectory(paths, TapeFmConfig.LibraryDirectory);
            return paths
                .Select(SongFromPath)
                .ToList();
        }

        private static void AddDirectory(List<string> target, string directory)
        {
            var entries = new DirectoryInfo(directory);
            foreach (var info in entries.EnumerateFileSystemInfos())
            {
                if (info.Name == "." || info.Name == "..")
                {
                    continue;
                }
                if ((info.Attributes & (FileAttributes.ReparsePoint | FileAttributes.Directory)) != 0)
                {
                    AddDirectory(target, info.FullName);
                }
                else if(ValidExtensions.Contains(info.Extension))
                {
                    target.Add(info.FullName);
                }
            }
        }

        private static readonly Uri LibraryUri = new Uri(TapeFmConfig.LibraryDirectory);
        private static Song SongFromPath(string path)
        {
            var songUri = new Uri(path);
            var relativePath = Uri.UnescapeDataString(
                LibraryUri.MakeRelativeUri(songUri).ToString());

            return new Song
            {
                Path = relativePath
            };
        }
    }
}