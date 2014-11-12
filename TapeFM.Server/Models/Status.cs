namespace TapeFM.Server.Models
{
    public class Status
    {
        public string CurrentTrack { get; set; }
        public bool IsPaused { get; set; }
        public int BitrateKbps { get; set; }
        public EmptyPlaylistMode EmptyPlaylistMode { get; set; }
    }
}