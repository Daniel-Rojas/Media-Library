using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaDataTypes.MusicDataTypes;
using Newtonsoft.Json;

namespace MediaDataTypes.MusicDataTypes
{
    public class Artist
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoFilePath { get; set; }
        [JsonIgnore]
        public List<Album> AlbumList { get; set; }

        public Artist(string name)
        {
            this.Name = name;
            AlbumList = new List<Album>();
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
    }
}