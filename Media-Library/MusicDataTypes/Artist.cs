using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MusicDataTypes
{
    public class Artist
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoFilePath { get; set; }
        [JsonIgnore]
        public List<Song> SongList { get; set; }
        [JsonIgnore]
        public List<Album> AlbumList { get; set; }

        public Artist(string name)
        {
            this.Name = name;
            SongList = new List<Song>();
            AlbumList = new List<Album>();
        }

        public void AddSongToList(Song newSong)
        {
            SongList.Add(newSong);
        }

        public void AddAlbumToList(Album newAlbum)
        {
            AlbumList.Add(newAlbum);
        }

        public override string ToString()
        {
            string summary = "Artist:: Name: " + Name;
            return summary;
        }

        public void PrintSongList()
        {
            foreach (Song song in SongList)
            {
                song.Print();
            }
        }

        public void PrintAlbumList()
        {
            foreach (Album album in AlbumList)
            {
                album.Print();
            }
        }

        public void Print()
        {
            Console.WriteLine("{0,30}{1,100}{2,70}",
                Name,
                Description,
                PhotoFilePath);
        }
    }
}