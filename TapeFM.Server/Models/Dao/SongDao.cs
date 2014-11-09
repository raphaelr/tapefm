using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TapeFM.Server.Code;

namespace TapeFM.Server.Models.Dao
{
    public static class SongDao
    {
        private static readonly string[] ValidExtensions = {".mp3", ".m4a", ".flac"};
        private static readonly ICacheEntry<List<Song>> CachedAllSongs =
            ApplicationCache.CreateEntry("songs_all", GetAllUncached);

        public static List<Song> GetAll()
        {
            return CachedAllSongs.Get();
        }

        private static List<Song> GetAllUncached()
        {
            return Directory
                .EnumerateFileSystemEntries(TapeFmConfig.LibraryDirectory, "*", SearchOption.AllDirectories)
                .Where(x => ValidExtensions.Contains(Path.GetExtension(x)))
                .Select(SongFromPath)
                .ToList();
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