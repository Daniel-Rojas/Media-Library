using MaterialDesignThemes.Wpf;
using MediaDataTypes;
using MediaInterfaces;
using MediaLibraryGUI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MediaLibraryGUI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        // Main Menu 
        private int _musicMenuSelectedIndex;
        private TextBlock _musicMenuSelector;
        private TextBlock _filmsMenuSelector;
        private TextBlock _tvShowsMenuSelector;

        //Media Interfaces
        private readonly MusicInterface _music;
        //private readonly MoviesInterface _movies; 
        //private readonly TVInterface _tvshows;

        // Frame Content Properties
        private UserControl _mainFrameContent;
        private UserControl _mediaPlayerFrameContent;

        // Views
        private UserControl _songsView;
        private UserControl _albumsView;
        private UserControl _artistView;
        private UserControl _mediaPlayerView;

        // View Models
        private SongsViewModel _songsVM;
        private AlbumsViewModel _albumsVM;
        private ArtistsViewModel _artistsVM;
        private MediaPlayerViewModel _mediaPlayerVM;

        public UserControl ArtistView
        {
            get { return _artistView; }
            set
            {
                _artistView = value;
                OnPropertyRaised("ArtistView");
            }
        }

        public ArtistsViewModel ArtistsVM
        {
            get { return _artistsVM; }
            set
            {
                _artistsVM = value;
                OnPropertyRaised("ArtistVM");
            }
        }

        public UserControl AlbumsView
        {
            get { return _albumsView; }
            set
            {
                _albumsView = value;
                OnPropertyRaised("AlbumsView");
            }
        }

        public AlbumsViewModel AlbumsVM
        {
            get { return _albumsVM; }
            set
            {
                _albumsVM = value;
                OnPropertyRaised("AlbumsVM");
            }
        }

        public UserControl SongsView
        {
            get { return _songsView; }
            set
            {
                _songsView = value;
                OnPropertyRaised("SongsView");
            }
        }

        public SongsViewModel SongsVM
        {
            get { return _songsVM; }
            set
            {
                _songsVM = value;
                OnPropertyRaised("SongsVM");
            }
        }



        public MainWindowViewModel()
        {
            // Initalizes Media Interfaces
            _music = new MusicInterface();

            // Initializes View
            InitializeViews();
            InitializeViewModels();

            //MainFrameContent = _songsView;
            //MainFrameContent = _albumsView;
            MainFrameContent = _artistView;

            _albumsVM.AlbumList = _music.TotalAlbumList;
            _artistsVM.ArtistList = _music.TotalArtistList; 
            //Test = new MediaPlayerViewModel();
            MediaPlayerFrameContent = _mediaPlayerView;

            _songsVM.SongList = _music.TotalSongList;
            _mediaPlayerVM.CurrentMedia = _songsVM.CurrentSong;
            _mediaPlayerVM.OnNextMedia = OnNextSong;
            _mediaPlayerVM.OnPrevMedia = OnPrevSong;
            _songsVM.OnCurrentSong = OnSelectedNewSong;
        }

        public void InitializeViews()
        {
            // Initializes Views
            _songsView = new SongsView();
            _albumsView = new AlbumsView();
            _artistView = new ArtistsView();

            // Initializes Media Player 
            _mediaPlayerView = new MediaPlayerView();
        }

        public void InitializeViewModels()
        {
            SongsVM = (SongsViewModel)_songsView.DataContext;
            AlbumsVM = (AlbumsViewModel)_albumsView.DataContext;
            ArtistsVM = (ArtistsViewModel)_artistView.DataContext;

            _mediaPlayerVM = (MediaPlayerViewModel)_mediaPlayerView.DataContext;

            SongsVM.MainWindowVM = this;
            AlbumsVM.MainWindowVM = this;
            ArtistsVM.MainWindowVM = this;
        }

        private void OnNextSong(string message)
        {
            //if (_mediaPlayerVM.Repeat == -1)
            //{
            if (_songsVM.SelectedSong < _songsVM.SongList.Count - 1)
            {
                _songsVM.MoveNext();
                _mediaPlayerVM.CurrentMedia = _songsVM.CurrentSong;
                _mediaPlayerVM.PlayNewMedia();
            }
            else
            {
                _songsVM.MoveNext();
                _mediaPlayerVM.CurrentMedia = _songsVM.CurrentSong;
                _mediaPlayerVM.StopMedia();
            }
            //)
            //else if (_mediaPlayerVM.Repeat == 0)
            //{
            /*
                    _songsVM.MoveNext();
                    _mediaPlayerVM.CurrentMedia = _songsVM.CurrentSong;
                    _mediaPlayerVM.PlayNewMedia();
            */
            //}
        }

        private void OnPrevSong(string message)
        {
            //if (_mediaPlayerVM.Repeat == -1) 
            if (_songsVM.SelectedSong > 0)
            {
                _songsVM.MovePrev();
                _mediaPlayerVM.CurrentMedia = _songsVM.CurrentSong;
                _mediaPlayerVM.PlayNewMedia();
            }
            else
            {
                _mediaPlayerVM.StopMedia();
            }
        }

        private void OnSelectedNewSong(string message)
        {
            _mediaPlayerVM.CurrentMedia = _songsVM.CurrentSong;
            _mediaPlayerVM.PlayNewMedia();
        }

        /********** Main Menu Function **********/
        public UserControl MainFrameContent
        {
            get { return _mainFrameContent; }
            set
            {
                _mainFrameContent = value;
                OnPropertyRaised("MainFrameContent");
            }
        }

        public UserControl MediaPlayerFrameContent
        {
            get { return _mediaPlayerFrameContent; }
            set
            {
                _mediaPlayerFrameContent = value;
                OnPropertyRaised("MediaPlayerFrameContent");
            }
        }

        public int MusicMenuSelectedIndex
        {
            get { return _musicMenuSelectedIndex; }
            set
            {
                _musicMenuSelectedIndex = value;
                OnPropertyRaised("MusicMenuSelectedIndex");
            }
        }

        public TextBlock MusicMenuSelector
        {
            get
            {
                return _musicMenuSelector;
            }
            set
            {
                _musicMenuSelector = value;
                OnPropertyRaised("MusicmenuSelector");
                if (value != null)
                {
                    if (!String.IsNullOrEmpty(value.Text))
                    {
                        MusicMenuSelectedIndex = -1;
                        OpenPage(value.Text);
                    }
                }
            }
        }

        public TextBlock FilmsMenuSelector
        {
            get
            {
                return _filmsMenuSelector;
            }
            set
            {
                _filmsMenuSelector = value;
                OnPropertyRaised("FilmsMenuSelector"); 
                if (value != null)
                {
                    if (!String.IsNullOrEmpty(value.Text))
                    {
                        OpenPage(value.Text);
                    }
                }
            }
        }

        public TextBlock TvShowsMenuSelector
        {
            get
            {
                return _tvShowsMenuSelector;
            }
            set
            {
                _tvShowsMenuSelector = value;
                OnPropertyRaised("TvShowsMenuSelector");
                if (value != null)
                {
                    if (!String.IsNullOrEmpty(value.Text))
                    {
                        OpenPage(value.Text);
                    }
                }
            }
        }

        private void OpenPage(string page)
        {
            // Music Menu Options
            /*if (page ==  "Songs")
            {
                //MainFrameContent = _songsView;
                FilmsMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Albums")
            {
                //MainFrameContent = _albumsView;
                FilmsMenuSelector = null;
                TvShowsMenuSelector = null;
            }*/
            if (page == "Artists")
            {

                MainFrameContent = ArtistView;
                MusicMenuSelectedIndex = -1;
                FilmsMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Playlists")
            {
                //FrameView = @"..\Views\PlaylistsView.xaml";
                FilmsMenuSelector = null;
                TvShowsMenuSelector = null;
            }

            // Films Menu Options
            else if (page == "Movies")
            {
                //FrameView = @"..\Views\MoviesView.xaml";
                MusicMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Actors")
            {
                //FrameView = @"..\ViewsActorsView.xaml";
                MusicMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Directors")
            {
                //FrameView = @"..\Views\DirectorsView.xaml";
                MusicMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Studios")
            {
                //FrameView = @"..\Views\StudiosView.xaml";
                MusicMenuSelector = null;
                TvShowsMenuSelector = null; 
            }

            // TV Shows Menu Options
            else if (page == "Series")
            {
                //FrameView = @"..\Views\SeriesView.xaml";
                MusicMenuSelector = null;
                FilmsMenuSelector = null;
            }
            else if (page == "Networks")
            {
                //FrameView = @"..\Views\NetworksView.xaml";
                MusicMenuSelector = null;
                FilmsMenuSelector = null;
            }
        }

        /********** Media Player Functions **********/

        public MusicInterface Music
        {
            get { return _music; }
        }







    }
}
