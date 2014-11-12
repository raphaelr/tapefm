using System.Diagnostics;
using System.IO;

namespace TapeFM.Server.Code.StreamServer
{
    public class Decoder
    {
        private Process _process;

        public Stream Stream { get; private set; }

        public void Decode(string filename)
        {
            Stream = null;

            filename = Path.Combine(TapeFmConfig.LibraryDirectory, filename);
            filename = Path.GetFullPath(filename);
            if (!File.Exists(filename) || filename.Contains("\""))
            {
                return;
            }

            var cmdline = new[]
            {
                "-loglevel", "quiet",
                "-hide_banner",
                "-i", "\"" + filename + "\"",
                "-af", "aresample=48000",
                "-f", "s16le",
                "-"
            };
            _process = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                UseShellExecute = false,
                Arguments = string.Join(" ", cmdline),
                RedirectStandardOutput = true,
            });

            Stream = _process.StandardOutput.BaseStream;
        }

        public void Stop()
        {
            if (_process != null && !_process.HasExited)
            {
                _process.Kill();
            }
        }
    }
}