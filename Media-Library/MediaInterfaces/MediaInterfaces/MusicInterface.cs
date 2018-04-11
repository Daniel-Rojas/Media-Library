using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaInterfaces
{
    using DataRetieval;
    using MusicAccess;

    public class MusicInterface
    {
        public LinkedList<Song> totalSongList { get; set; }
        public LinkedList<Album> totalAlbumList { get; set; }
        public LinkedList<Artist> totalArtistList { get; set; }

        private SerialDataAccess serialData;
        private DatabaseDataAccess databaseData;

        public MusicInterface(bool useOfServer)
        {
            //useOfServer = false; // temp for local testing
            initialize(useOfServer);
        }

        private void initialize(bool useOfServer)
        {
            // for client server versions the Music class retrieves
            // information from a database of the server side
            if (useOfServer)
            {
                databaseData = new DatabaseDataAccess();
                totalSongList = databaseData.ImportAllSongs();
                totalAlbumList = databaseData.ImportAllAlbums();
                totalArtistList = databaseData.ImportAllArtists();
            }
            // for the local version the Music class retrieves 
            // information from local xml files
            else
            {
                serialData = new SerialDataAccess();
                totalSongList = serialData.deserializeSongList();
                totalAlbumList = serialData.deserializeAlbumList();
                totalArtistList = serialData.deserializeArtistList();

 

                //serialData.serializeSongList(totalSongList);
                
            }
        }
    }
}
