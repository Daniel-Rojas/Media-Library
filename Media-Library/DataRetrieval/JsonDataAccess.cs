using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MediaDataTypes.MusicDataTypes;

namespace DataRetrieval
{
    public class JsonDataAccess
    {
        private string dataFilePath = @".\Data";
        private string songFilePath = @".\Data\Songs.json";
        private string albumFilePath = @".\Data\Albums.json";
        private string artistFilePath = @".\Data\Artist.json";

        public JsonDataAccess()
        {
            if (!Directory.Exists(dataFilePath))
            {
                Directory.CreateDirectory(dataFilePath);
            }
        }

        public List<Song> DeserializeSongData()
        {
            List<Song> songList = new List<Song>();
            if (File.Exists(songFilePath))
            {
                using (FileStream fs = File.OpenRead(songFilePath))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        bool okToRead = true;
                        while (okToRead)
                        {
                            string line = sr.ReadLine();
                            if (line != null)
                            {
                                Song song = JsonConvert.DeserializeObject<Song>(line);
                                songList.Add(song);
                            }
                            else
                            {
                                okToRead = false;
                            }
                        }
                    }
                }
            }
            return songList;
        }

        public List<Album> DeserializeAlbumData()
        {
            List<Album> albumList = new List<Album>();
            if (File.Exists(albumFilePath))
            {
                using (FileStream fs = File.OpenRead(albumFilePath))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        bool okToRead = true;
                        while (okToRead)
                        {
                            string line = sr.ReadLine();
                            if (line != null)
                            {
                                Album album = JsonConvert.DeserializeObject<Album>(line);
                                albumList.Add(album);
                            }
                            else
                            {
                                okToRead = false;
                            }
                        }
                    }
                }
            }
            return albumList;
        }

        public List<Artist> DeserializeArtistData()
        {
            List<Artist> artistList = new List<Artist>();
            using (FileStream fs = File.OpenRead(artistFilePath))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    bool okToRead = true;
                    while (okToRead)
                    {
                        string line = sr.ReadLine();
                        if (line != null)
                        {
                            Artist artist = JsonConvert.DeserializeObject<Artist>(line);
                            artistList.Add(artist);
                        }
                        else
                        {
                            okToRead = false;
                        }
                    }
                }
            }
            return artistList;
        }

        public void SerializeSongData(List<Song> songList)
        {
            using (StreamWriter sw = new StreamWriter(songFilePath))
            {
                foreach (Song song in songList)
                {
                    string jsonSong = JsonConvert.SerializeObject(song);
                    sw.WriteLine(jsonSong);
                }
            }
        }

        public void SerializedAlbumData(List<Album> albumList)
        {
            using (StreamWriter sw = new StreamWriter(albumFilePath))
            {
                foreach (Album album in albumList)
                {
                    string jsonAlbum = JsonConvert.SerializeObject(album);
                    sw.WriteLine(jsonAlbum);
                }
            }
        }

        public void SerializeArtistData(List<Artist> artistList)
        {
            using (StreamWriter sw = new StreamWriter(artistFilePath))
            {
                foreach (Artist artist in artistList)
                {
                    string jsonArtist = JsonConvert.SerializeObject(artist);
                    sw.WriteLine(jsonArtist);
                }
            }
        }
    }
}