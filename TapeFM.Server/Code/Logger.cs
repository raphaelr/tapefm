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
    }
}