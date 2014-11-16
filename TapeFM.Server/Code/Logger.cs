using System;
using System.Diagnostics;

namespace TapeFM.Server.Code
{
    public static class Logger
    {
        public static TraceSource GetComponent(string component)
        {
            var sourceLevelString =
                TapeFmConfig.Configuration.Get("logging:" + component) ??
                TapeFmConfig.Configuration.Get("logging:All");

            SourceLevels sourceLevel;

            if (!Enum.TryParse(sourceLevelString, out sourceLevel))
            {
                sourceLevel = SourceLevels.Information;
            }

            var source = new TraceSource("tapefm." + component, sourceLevel);
            source.Listeners.Clear();
            source.Listeners.Add(new ConsoleTraceListener());
            return source;
        }

        public static void TraceException(this TraceSource source, string message, Exception e)
        {
            source.TraceEvent(TraceEventType.Error, 0, "{0}: {1}: {2}\nStack Trace:\n{3}",
                    message, e.GetType().Name, e.Message, e.StackTrace);
        }
    }
}