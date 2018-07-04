using MediaInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MediaLibraryGUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Main Menu 
        private TextBlock _musicMenuSelector;
        private TextBlock _filmsMenuSelector;
        private TextBlock _tvShowsMenuSelector;
        private string _frameView;

        // Media Player
        private readonly MediaPlayer _mediaPlayer;
        private readonly DispatcherTimer _timer;
        private TimeSpan _audioPosition;
        private TimeSpan _audioDuration;
        private double _sliderPosition;
        private double _sliderDuration;
        private double _volume;
        private double _volumeBeforeMute;
        private bool _isMuted;
        private bool _isPaused;

        //Media Interfaces
        private readonly MusicInterface _music;
        //private readonly MoviesInterface _movies; 
        //private readonly TVInterface _tvshows;

        public MainWindowViewModel()
        {

        }


        public string FrameView
        {
            get
            {
                return _frameView;
            }
            set
            {
                _frameView = value;
                OnPropertyRaised("FrameView");
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
            if (page ==  "Songs")
            {
                FrameView = @"..\Views\SongsView.xaml";
                FilmsMenuSelector = null;
                TvShowsMenuSelector = null;

            }
            else if (page == "Albums")
            {
                FrameView = @"Views\AlbumsView.xaml";
                FilmsMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Artists")
            {
                FrameView = @"Views\ArtistsView.xaml";
                FilmsMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Playlists")
            {
                FrameView = @"Views\PlaylistsView.xaml";
                FilmsMenuSelector = null;
                TvShowsMenuSelector = null;
            }

            // Films Menu Options
            else if (page == "Movies")
            {
                FrameView = @"Views\MoviesView.xaml";
                MusicMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Actors")
            {
                FrameView = @"ViewsActorsView.xaml";
                MusicMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Directors")
            {
                FrameView = @"Views\DirectorsView.xaml";
                MusicMenuSelector = null;
                TvShowsMenuSelector = null;
            }
            else if (page == "Studios")
            {
                FrameView = @"Views\StudiosView.xaml";
                MusicMenuSelector = null;
                TvShowsMenuSelector = null; 
            }

            // TV Shows Menu Options
            else if (page == "Series")
            {
                FrameView = @"Views\SeriesView.xaml";
                MusicMenuSelector = null;
                FilmsMenuSelector = null;
            }
            else if (page == "Networks")
            {
                FrameView = @"Views\NetworksView.xaml";
                MusicMenuSelector = null;
                FilmsMenuSelector = null;
            }
        }



        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

    }
}
