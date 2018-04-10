using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAccess
{
    public class Artist
    {
        public string name { get; set; }
        public string description { get; set; }
        public string photoFilePath { get; set; }
        public List<Song> songList { get; set; }
        public List<Album> albumList { get; set; }

        public Artist(string name)
        {
            this.name = name;
        }

        public void addSongToList(Song newSong)
        {
            songList.Add(newSong);
        }

        public void addAlbumToList(Album newAlbum)
        {
            albumList.Add(newAlbum);
        }


    }
}
