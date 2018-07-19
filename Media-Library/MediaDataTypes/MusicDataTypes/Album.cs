using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaDataTypes.MusicDataTypes;
using Newtonsoft.Json;

namespace MediaDataTypes.MusicDataTypes
{
    public class Album
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public uint Year { get; set; }
        public uint NumberOfTracks { get; set; }
        public string CoverFilePath { get; set; }
        [JsonIgnore]
        public List<Song> SongList { get; set; }

        public Album(string title, string artist)
        {
            this.Title = title;
            this.Artist = artist;
            SongList = new List<Song>();
        }

        public void AddSongToList(Song newSong)
        {
            SongList.Add(newSong);
        }

        public override string ToString()
        {
            string summary = "Album:: Title: " + Title + ", Artist: " + Artist;
            return summary;
        }
    }
}