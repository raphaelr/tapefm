using System.Diagnostics;
using System.IO;

namespace TapeFM.Server.Code.StreamServer
{
    public class Decoder
    {
        private static readonly TraceSource Trace = Logger.GetComponent("Decoder");

        private Process _process;

        public Stream Stream { get; private set; }

        public void Decode(string filename)
        {
            Stream = null;

            filename = Path.Combine(TapeFmConfig.LibraryDirectory, filename);
            filename = Path.GetFullPath(filename);
            if (!File.Exists(filename) || filename.Contains("\""))
            {
                Trace.TraceEvent(TraceEventType.Warning, 0,
                    "Rejecting decode of '{0}' because file does not exist or path contains invalid characters",
                    filename);
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

            Trace.TraceEvent(TraceEventType.Verbose, 0, "Starting decoder process to decode {0}", filename);
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
            Trace.TraceEvent(TraceEventType.Verbose, 0, "Killing decoder process");
            if (_process != null && !_process.HasExited)
            {
                _process.Kill();
            }
        }
    }
}