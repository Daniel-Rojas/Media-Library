using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MusicDataTypes
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

        public void PrintSongList()
        {
            foreach (Song song in SongList)
            {
                song.Print();
            }
        }

        public void Print()
        {
            Console.WriteLine("{0,30}{1,20}{2,10}{3,5}{4,70}",
                Title,
                Artist,
                Year,
                NumberOfTracks,
                CoverFilePath);
        }
    }
}