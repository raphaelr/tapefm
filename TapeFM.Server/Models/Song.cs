using System.Runtime.Serialization;

namespace TapeFM.Server.Models
{
    [DataContract]
    public class Song
    {
        [DataMember]
        public string Path { get; set; }
    }
}