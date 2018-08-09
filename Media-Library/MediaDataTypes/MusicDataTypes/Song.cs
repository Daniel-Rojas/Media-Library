using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaDataTypes;

namespace MediaDataTypes.MusicDataTypes
{
    public class Song : Media
    {
        public string Artist { get; set; }
        public string FeaturedArtist { get; set; }
        public string AlbumTitle { get; set; }
        public string Genre { get; set; }
        public uint TrackNumber { get; set; }

        //public Song(string filePath)
        //{
        //    FilePath = filePath;
        //}

        public Song(string filePath, string title, string artist)
        {
            FilePath = filePath;
            Title = title;
            Artist = artist;
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string songSummary = "Song:: Title: " + Title + ", Artist: " + Artist + ", Path: " + FilePath;
            return songSummary;
        }
    }
}