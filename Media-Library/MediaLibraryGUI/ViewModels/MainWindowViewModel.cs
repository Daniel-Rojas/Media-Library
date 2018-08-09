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
        private int _musicMenuSelected;
        private int _filmMenuSelected;
        private int _tvMenuSelected;

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
        private UserControl _addMediaView;
        private UserControl _addMusicView;
        // View Models
        private SongsViewModel _songsVM;
        private AlbumsViewModel _albumsVM;
        private ArtistsViewModel _artistsVM;
        private MediaPlayerViewModel _mediaPlayerVM;
        private AddMediaViewModel _addMediaVM;
        private AddMusicViewModel _addMusicVM;

        public MainWindowViewModel()
        {
            // Initializes Main Menu Selection
            MusicMenuSelected = -1;
            FilmMenuSelected = -1;
            TVMenuSelected = -1;

            // Initalizes Media Interfaces
            _music = new MusicInterface();

            // Initializes View
            InitializeViews();
            InitializeViewModels();

            // Initializes Frame Contents
            MainFrameContent = _artistView;
            MediaPlayerFrameContent = _mediaPlayerView;

            _artistsVM.ArtistList = _music.TotalArtistList;

            AddMediaCommand = new RelayCommand(OnAddMedia, CanAddMedia);
        }

        /********** Media Interface Access Functions **********/
        public MusicInterface Music
        {
            get { return _music; }
        }

        /********** View and View Model Initializer Functions **********/
        public void InitializeViews()
        {
            // Initializes Views
            _songsView = new SongsView();
            _albumsView = new AlbumsView();
            _artistView = new ArtistsView();
            _mediaPlayerView = new MediaPlayerView();
            _addMediaView = new AddMediaView();
            _addMusicView = new AddMusicView();
            // Initializes Media Player 
            
        }

        public void InitializeViewModels()
        {
            SongsVM = (SongsViewModel)_songsView.DataContext;
            AlbumsVM = (AlbumsViewModel)_albumsView.DataContext;
            ArtistsVM = (ArtistsViewModel)_artistView.DataContext;
            MediaPlayerVM = (MediaPlayerViewModel)_mediaPlayerView.DataContext;
            AddMediaVM = (AddMediaViewModel)_addMediaView.DataContext;
            AddMusicVM = (AddMusicViewModel)_addMusicView.DataContext;

            SongsVM.MainWindowVM = this;
            AlbumsVM.MainWindowVM = this;
            ArtistsVM.MainWindowVM = this;
            MediaPlayerVM.MainWindowVM = this;
            AddMediaVM.MainWindowVM = this;
            AddMusicVM.MainWindowVM = this;

            AddMediaVM.AddMediaFrameContent = AddMusicView;
        }

        /********** View and View Model Access Functions **********/
        // View Access Functions
        public UserControl ArtistView
        {
            get { return _artistView; }
            set
            {
                _artistView = value;
                OnPropertyRaised("ArtistView");
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

        public UserControl SongsView
        {
            get { return _songsView; }
            set
            {
                _songsView = value;
                OnPropertyRaised("SongsView");
            }
        }

        public UserControl AddMediaView
        {
            get { return _addMediaView; }
            set
            {
                _addMediaView = value;
                OnPropertyRaised("AddMediaView");
            }
        }

        public UserControl AddMusicView
        {
            get { return _addMusicView; }
            set
            {
                _addMusicView = value;
                OnPropertyRaised("AddMusicView");
            }
        }

        public UserControl MediaPlayerView
        {
            get { return _mediaPlayerView; }
            set
            {
                _mediaPlayerView = value;
                OnPropertyRaised("MediaPlayerView");
            }
        }

        // View Model Access Functions
        public ArtistsViewModel ArtistsVM
        {
            get { return _artistsVM; }
            set
            {
                _artistsVM = value;
                OnPropertyRaised("ArtistVM");
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

        public SongsViewModel SongsVM
        {
            get { return _songsVM; }
            set
            {
                _songsVM = value;
                OnPropertyRaised("SongsVM");
            }
        }

        public AddMediaViewModel AddMediaVM
        {
            get { return _addMediaVM; }
            set
            {
                _addMediaVM = value;
                OnPropertyRaised("AddMediaVM");
            }
        }

        public AddMusicViewModel AddMusicVM
        {
            get { return _addMusicVM; }
            set
            {
                _addMusicVM = value;
                OnPropertyRaised("AddMusicVM");
            }
        }

        public MediaPlayerViewModel MediaPlayerVM
        {
            get { return _mediaPlayerVM; }
            set
            {
                _mediaPlayerVM = value;
                OnPropertyRaised("MediaPlayerVM");
            }
        }

        /********** Main Menu Function **********/
        public UserControl MainFrameContent
        {
            get { return _mainFrameContent; }
            set
            {
                _mainFrameContent = value;
                OnPropertyRaised("MainFrameContent");
                if (value != ArtistView)
                {
                    MusicMenuSelected = -1;
                }
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

        public int MusicMenuSelected
        {
            get { return _musicMenuSelected; }
            set
            {
                _musicMenuSelected = value;
                OnPropertyRaised("MusicMenuSelected");
                if (value > -1)
                {
                    if (value == 0)
                    {
                        MainFrameContent = ArtistView;
                        ArtistsVM.SelectedArtist = -1;
                    }
                    else if (value == 1)
                    {
                        //MainFrameContent = PlaylistView;

                    }
                    else if (value == 2)
                    {
                        
                    }
                    FilmMenuSelected = -1;
                    TVMenuSelected = -1;
                }
            }
        }

        public int FilmMenuSelected
        {
            get { return _filmMenuSelected; }
            set
            {
                _filmMenuSelected = value;
                OnPropertyRaised("FilmsMenuSelected");
                if (value > -1)
                {
                    _filmMenuSelected = -1;
                    MusicMenuSelected = -1;
                    TVMenuSelected = -1;
                }
            }
        }

        public int TVMenuSelected
        {
            get { return _tvMenuSelected; }
            set
            {
                _tvMenuSelected = value;
                OnPropertyRaised("TvShowsMenuSelected");
                if (value > -1)
                {
                    _tvMenuSelected = -1;
                    MusicMenuSelected = -1;
                    FilmMenuSelected = -1;
                }
            }
        }

        public void OnAddMedia(object obj)
        {
            MainFrameContent = AddMediaView;
        }

        public bool CanAddMedia(object obj)
        {
            return true; ;
        }

        public ICommand AddMediaCommand { get; set; }
    }
}
