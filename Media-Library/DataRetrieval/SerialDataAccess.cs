using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace DataRetieval
{
    using MusicAccess;

    public class SerialDataAccess
    {
        public SerialDataAccess()
        {
            string dataFilePath = @".\Data";
            if (!Directory.Exists(dataFilePath))
            {
                Directory.CreateDirectory(dataFilePath);
            }
        }

        public List<Song> deserializeSongList()
        {
            string songsFile = @".\Data\songs.xml";
            List<Song> songList = new List<Song>();
            if (File.Exists(songsFile))
            {
                
                XmlSerializer serializer = new XmlSerializer(typeof(List<Song>));
                using (FileStream stream = File.OpenRead(songsFile))
                {
                    songList = (List<Song>)serializer.Deserialize(stream);
                }
                
            }
            return songList;
        }

        public List<Album> deserializeAlbumList()
        {
            string albumsFile = @".\Data\albums.xml";
            List<Album> albumList = new List<Album>();
            if (File.Exists(albumsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Album>));
                using (FileStream stream = File.OpenRead(albumsFile))
                {
                    albumList = (List<Album>)serializer.Deserialize(stream);
                }
            }
            return albumList;
        }

        public List<Artist> deserializeArtistList()
        {
            string artistsFile = @".\Data\artists.xml";
            List<Artist> artistList = new List<Artist>();
            if (File.Exists(artistsFile))
            {
                using (FileStream stream = File.OpenRead(artistsFile))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Artist>));
                    artistList = (List<Artist>)serializer.Deserialize(stream);
                }
            }
            return artistList;
        }

        public void serializeSongList(List<Song> songList)
        {
            using (Stream stream = new FileStream(@".\Data\songs.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Song>));
                serializer.Serialize(stream, songList);
            }
        }

        public void serializeAlbumList(List<Album> albumList)
        {
            using (Stream stream = new FileStream(@".\Data\albums.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Album>));
                serializer.Serialize(stream, albumList);
            }
        }

        public void serializeArtistList(List<Artist> artistList)
        {
            using (Stream stream = new FileStream(@".\Data\artists.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Artist>));
                serializer.Serialize(stream, artistList);
            }
        }
    }
}
