using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDataTypes
{
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string FeaturedArtist { get; set; }
        public string AlbumTitle { get; set; }
        public string Genre { get; set; }
        public string FilePath { get; set; }
        public uint TrackNumber { get; set; }

        public Song(string title, string artist, string filePath)
        {
            Title = title;
            Artist = artist;
            FilePath = filePath;
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                Song objSong = (Song)obj;
                if (objSong.Title == this.Title && objSong.Artist == this.Artist && objSong.FilePath == this.FilePath)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public override string ToString()
        {
            string songSummary = "Song:: Title: " + Title + ", Artist: " + Artist + ", Path: " + FilePath;
            return songSummary;
        }

        public void Print()
        {
            Console.WriteLine("{0,30}{1,20}{2,20}{3,20}{4,15}{5,3}{6,70}",
                Title,
                Artist,
                FeaturedArtist,
                AlbumTitle,
                Genre,
                TrackNumber,
                FilePath);
        }
    }
}