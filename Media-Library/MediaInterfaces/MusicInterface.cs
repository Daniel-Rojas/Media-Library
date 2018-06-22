using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRetrieval;
using MusicDataTypes;

namespace MediaInterfaces
{
    public class MusicInterface
    {
        public List<Song> totalSongList { get; set; }
        public List<Album> totalAlbumList { get; set; }
        public List<Artist> totalArtistList { get; set; }

        private JsonDataAccess jsonData;

        public MusicInterface()
        {
            jsonData = new JsonDataAccess();
            importMusicData();
            initializeData();
        }

        ~MusicInterface()
        {
            exportMusicData();
        }

        private void exportMusicData()
        {
            jsonData.serializeSongData(totalSongList);
            jsonData.serializedAlbumData(totalAlbumList);
            jsonData.serializeArtistData(totalArtistList);
        }

        private void importMusicData()
        {
            totalSongList = jsonData.deserializeSongData();
            totalAlbumList = jsonData.deserializeAlbumData();
            totalArtistList = jsonData.deserializeArtistData();
        }

        private void initializeData()
        {
            foreach (Artist artist in totalArtistList)
            {
                foreach (Album album in totalAlbumList)
                {
                    if (album.Artist == artist.Name)
                    {
                        artist.AlbumList.Add(album);
                        foreach (Song song in totalSongList)
                        {
                            if (song.AlbumTitle == album.Title && song.Artist == album.Artist)
                            {
                                album.SongList.Add(song);
                                artist.SongList.Add(song);
                            }
                        }
                    }
                }
            }
        }

        public void addSong()
        {

            Console.Write("Enter Song Title: ");
            string songTitle = Console.ReadLine();
            Console.Write("Enter Song Artist: ");
            string songArtist = Console.ReadLine();
            Console.Write("Enter Song File Path");
            string filePath = Console.ReadLine();
            Song song = new Song(songTitle, songArtist, filePath);
            Console.Write("Does this song have a feaetured artist(s) Enter T if true");
            string boolfeatArtist = Console.ReadLine().ToUpper();
            if (boolfeatArtist == "T")
            {
                Console.Write("Enter Song Featured Artist(s)");
                song.FeaturedArtist = Console.ReadLine();
            }
            Console.Write("Enter Song Genre");
            song.Genre = Console.ReadLine();

            Console.Write("Enter Song Album Title");

        }

        public void addSong(Song song)
        {
            totalSongList.Add(song);
        }

        public void addAlbum(Album album)
        {
            totalAlbumList.Add(album);
        }

        public void addArtist(Artist artist)
        {
            totalArtistList.Add(artist);
        }
    }
}
