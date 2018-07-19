using MediaDataTypes.MusicDataTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaDataTypes.MusicDataTypes
{
    class Playlist
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Song> SongList { get; set; }

        public Playlist(string title)
        {
            Title = title;
        }

        public override string ToString()
        {
            string summary = "Playlist:: Title: " + Title + ", Description: " + Description;
            return summary;
        }
    }
}
