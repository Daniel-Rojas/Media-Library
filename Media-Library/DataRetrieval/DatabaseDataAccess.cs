using System; 
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MusicAccess
{
    public class DatabaseDataAccess
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string userID;
        private string password;

        public DatabaseDataAccess()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "localhost";
            database = "Media_Library";
            userID = "drojas95";
            password = "Napoleon113";
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                                      database + ";" + "UID=" + userID + ";" + "PASSWORD=" + password + ";";

             connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server. Contact administrator");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void InsertSong(Song song)
        {
            string query = "INSERT INTO songs (song_title, song_artist, song_featured_artist, song_album," +
                           " song_genre, song_track_number, song_filepath) VALUES('";

            query += song.title + "', '" + song.artist + "', '" + song.featuredArtist + "', '" + song.albumTitle + "', '";
            query += song.genre + "', " + song.trackNumber + ", '" + song.filePath + "')";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void InsertAlbum(Album album)
        {
            string query = "INSERT INTO albums (album_title, album_artist, album_year, album_num_tracks, album_photo_filepath) VALUES('";

            query += album.title + "', '" + album.artist + "', " + album.year + ", " + album.numberOfTracks + ", '" + album.photoFilePath + "')";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void InsertArtist(Artist artist)
        {
            string query = "INSERT INTO artists (artist_name, artist_description, artist_photo_filepath) VALUES ('";

            query +=  artist.name + "', '" + artist.description + "', '" + artist.photoFilePath + "')";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void DeleteSong(string title, string artist)
        {
            string query = "DELETE FROM songs WHERE song_title='" + title + "' AND song_artist='" + artist + "'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void DeleteAlbum(string title, string artist)
        {
            string query = "DELETE FROM songs WHERE album_title='" + title + "' AND album_artist='" + artist + "'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void DeleteArtist(string name)
        {
            string query = "DELETE FROM artists WHERE artist_name='" + name + "'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public LinkedList<Song> ImportAllSongs()
        {
            string query = "SELECT * FROM songs";

            LinkedList<Song> songList = new LinkedList<Song>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string title = dataReader.GetString("song_title");
                    string artist = dataReader.GetString("song_artist");
                    string filepath = dataReader.GetString("song_filepath");

                    Song currentSong = new Song(title, artist, filepath);

                    currentSong.featuredArtist = dataReader.GetString("song_featured_artist");
                    currentSong.albumTitle = dataReader.GetString("song_album");
                    currentSong.genre = dataReader.GetString("song_album");
                    currentSong.trackNumber = dataReader.GetUInt32("song_track_number");

                    songList.AddLast(currentSong);
                }

                dataReader.Close();
                this.CloseConnection();
                return songList;
            }
            else
            {
                return songList;
            }
        }

        public LinkedList<Album> ImportAllAlbums()
        {
            string query = "SELECT * FROM albums";

            LinkedList<Album> albumList = new LinkedList<Album>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string title = dataReader.GetString("album_title");
                    string artist = dataReader.GetString("album_artist");

                    Album currentAlbum = new Album(title, artist);

                    currentAlbum.year = dataReader.GetUInt32("album_year");
                    currentAlbum.numberOfTracks = dataReader.GetUInt32("album_num_tracks");
                    currentAlbum.photoFilePath = dataReader.GetString("album_photo_filepath");

                    albumList.AddLast(currentAlbum);
                }

                dataReader.Close();
                this.CloseConnection();
                return albumList;
            }
            else
            {
                return albumList;
            }
        }

        public LinkedList<Artist> ImportAllArtists()
        {
            string query = "SELECT * FROM artists";

           LinkedList<Artist> artistList = new LinkedList<Artist>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string name = dataReader.GetString("artist_name");

                    Artist currentArtist= new Artist(name);

                    currentArtist.description = dataReader.GetString("artist_description");
                    currentArtist.photoFilePath = dataReader.GetString("artist_photo_filepath");

                    artistList.AddLast(currentArtist);
                }

                dataReader.Close();
                this.CloseConnection();
                return artistList;
            }
            else
            {
                return artistList;
            }
        }


    }
}
