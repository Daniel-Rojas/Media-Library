using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAccess
{

    public class Song
    {
        public string title { get; set; }
        public string artist { get; set; }
        public string featuredArtist { get; set; }
        public string albumTitle { get; set; }
        public string genre { get; set; }
        public string filePath { get; set; }
        public uint trackNumber { get; set; }

        private Song() { }

        public Song(string title, string artist, string filePath)
        {
            this.title = title;
            this.artist = artist;
            this.filePath = filePath;
        }
    }
}
