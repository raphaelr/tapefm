using System;

namespace TapeFM.Server.Code.StreamServer.Mux
{
    [Flags]
    public enum HeaderType : byte
    {
        Default = 0,
        Continuation = 1,
        BeginOfStream = 2,
        EndOfStream = 4
    }
}