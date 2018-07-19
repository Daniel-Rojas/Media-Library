using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRetrieval;
using MediaDataTypes.MusicDataTypes;

namespace MediaInterfaces
{
    public class MusicInterface
    {
        public List<Song> TotalSongList { get; set; }
        public List<Album> TotalAlbumList { get; set; }
        public List<Artist> TotalArtistList { get; set; }

        public List<Song> ActiveSongList { get; set; }
        

        private JsonDataAccess jsonData;

        public MusicInterface()
        {
            jsonData = new JsonDataAccess();
            ImportMusicData();
            InitializeData();
        }

        ~MusicInterface()
        {
            ExportMusicData();
        }

        private void ExportMusicData()
        {
            jsonData.SerializeSongData(TotalSongList);
            jsonData.SerializedAlbumData(TotalAlbumList);
            jsonData.SerializeArtistData(TotalArtistList);
        }

        private void ImportMusicData()
        {
            TotalSongList = jsonData.DeserializeSongData();
            TotalAlbumList = jsonData.DeserializeAlbumData();
            TotalArtistList = jsonData.DeserializeArtistData();
        }

        private void InitializeData()
        {
            foreach (Artist artist in TotalArtistList)
            {
                foreach (Album album in TotalAlbumList)
                {
                    if (album.Artist == artist.Name)
                    {
                        artist.AlbumList.Add(album);
                        foreach (Song song in TotalSongList)
                        {
                            if (song.AlbumTitle == album.Title && song.Artist == album.Artist)
                            {
                                album.SongList.Add(song);
                            }
                        }
                    }
                }
            }
        }

        public void AddSong()
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

        public void AddSong(Song song)
        {
            TotalSongList.Add(song);
        }

        public void AddAlbum(Album album)
        {
            TotalAlbumList.Add(album);
        }

        public void AddArtist(Artist artist)
        {
            TotalArtistList.Add(artist);
        }
    }
}
