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

        public LinkedList<Song> deserializeSongList()
        {
            string songsFile = @".\Data\songs.xml";
            List<Song> songList = new List<Song>();
            LinkedList<Song> songLinkedList = new LinkedList<Song>();
            if (File.Exists(songsFile))
            {
                
                XmlSerializer serializer = new XmlSerializer(typeof(List<Song>));
                using (FileStream stream = File.OpenRead(songsFile))
                {
                    songList = (List<Song>)serializer.Deserialize(stream);
                }
                
                foreach (Song song in songList)
                {
                    songLinkedList.AddLast(song);
                }   
            }
            return songLinkedList;
        }

        public LinkedList<Album> deserializeAlbumList()
        {
            string albumsFile = @".\Data\albums.xml";
            List<Album> albumList = new List<Album>();
            LinkedList<Album> albumLinkedList = new LinkedList<Album>();
            if (File.Exists(albumsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Album>));
                using (FileStream stream = File.OpenRead(albumsFile))
                {
                    albumList = (List<Album>)serializer.Deserialize(stream);
                }

                foreach (Album album in albumList)
                {
                    albumLinkedList.AddLast(album);
                }
            }
            return albumLinkedList;
        }

        public LinkedList<Artist> deserializeArtistList()
        {
            string artistsFile = @".\Data\artists.xml";
            List<Artist> artistList = new List<Artist>();
            LinkedList<Artist> artistLinkedList = new LinkedList<Artist>();
            if (File.Exists(artistsFile))
            {
                using (FileStream stream = File.OpenRead(artistsFile))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Artist>));
                    artistList = (List<Artist>)serializer.Deserialize(stream);
                }

                foreach (Artist artist in artistLinkedList)
                {
                    artistLinkedList.AddLast(artist);
                }
            }
            return artistLinkedList;
        }

        public void serializeSongList(LinkedList<Song> songLinkedList)
        {
            List<Song> songList = songLinkedList.ToList<Song>();
            using (Stream stream = new FileStream(@".\Data\songs.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Song>));
                serializer.Serialize(stream, songList);
            }
        }

        public void serializeAlbumList(LinkedList<Album> albumLinkedList)
        {
            List<Album> albumList = albumLinkedList.ToList<Album>();
            using (Stream stream = new FileStream(@".\Data\albums.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Album>));
                serializer.Serialize(stream, albumList);
            }
        }

        public void serializeArtistList(LinkedList<Artist> artistLinkedList)
        {
            List<Artist> artistList = artistLinkedList.ToList<Artist>();
            using (Stream stream = new FileStream(@".\Data\artists.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Artist>));
                serializer.Serialize(stream, artistList);
            }
        }
    }
}
