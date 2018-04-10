using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAccess
{
    public class Album
    {
        public string title { get; set; }
        public string artist { get; set; }
        public uint year { get; set; }
        public uint numberOfTracks { get; set; }
        public string photoFilePath { get; set; }
        public List<Song> songList { get; set; }

        public Album(string title, string artist)
        {
            this.title = title;
            this.artist = artist;
        }

        public void addSongToList(Song newSong)
        {
            songList.Add(newSong);
        }

    }
}
