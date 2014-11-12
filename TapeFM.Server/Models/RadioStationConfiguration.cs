﻿using System.Runtime.Serialization;

namespace TapeFM.Server.Models
{
    [DataContract]
    public class RadioStationConfiguration
    {
        [DataMember]
        public int BitrateKbps { get; set; }
        [DataMember]
        public EmptyPlaylistMode EmptyPlaylistMode { get; set; }
    }
}