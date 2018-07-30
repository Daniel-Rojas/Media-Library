using MaterialDesignThemes.Wpf;
using MediaDataTypes;
using MediaDataTypes.MusicDataTypes;
using MediaDataTypes.TVDataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MediaLibraryGUI.ViewModels
{
    public class MediaPlayerViewModel : BaseViewModel
    {
        private readonly MediaPlayer _mediaPlayer;

        // Properties dealing with the time aspect of the media
        private readonly DispatcherTimer _timer;
        private TimeSpan _mediaPosition;
        private TimeSpan _mediaDuration;
        private double _sliderPosition;
        private double _sliderDuration;
        private bool _suppressMediaPositionUpdate;

        // Properties dealing with the volume aspect of the media
        private double _volume;
        private double _volumeBeforeMute;
        private bool _isMuted;

        // Properties dealing with playback
        private bool _isPaused;
        private bool _shuffleOn;
        private int _repeatValue;

        // Properties dealing with Button Content
        private Brush _shuffleColor;
        private Brush _repeatColor;
        private PackIconKind _repeatButtonKind;
        private PackIconKind _playButtonKind;
        private PackIconKind _volumeButtonKind;

        private Media _currentMedia;
        private string _albumCover;
        private List<Media> _activeMediaList;
        private List<Media> _shuffleList;
        public int ActiveListIndex;

        private MainWindowViewModel _mainWindowVM;

        private Random rand;

        // Constructor
        public MediaPlayerViewModel()
        {
            // Initializes the Active Media List and Index
            ActiveMediaList = new List<Media>();
            ActiveListIndex = -1;
            CurrentMedia = null;
            _shuffleList = new List<Media>();
            rand = new Random();


            // Initializes the Media Player
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaOpened += OnMediaOpen;
            _mediaPlayer.MediaEnded += OnMediaEnd;
            _volume = 50.0;
            _isMuted = false;

            _isPaused = true;
            _shuffleOn = false;
            _repeatValue = 0; // 0 = repeat off | 1 = repeat list | 2 = repeat single media

            // Initializes Media Player Button Content
            ShuffleColor = (Brush)Application.Current.Resources["PrimaryHueMidBrush"];
            RepeatColor = (Brush)Application.Current.Resources["PrimaryHueMidBrush"];
            _repeatButtonKind = PackIconKind.Repeat;
            _volumeButtonKind = PackIconKind.VolumeHigh;
            _playButtonKind = PackIconKind.PlayCircleOutline;

            //Initializes Media Player Timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += OnTimerTick;
            _suppressMediaPositionUpdate = false;

            // Initializes Media Player Control Commands
            PlayPauseCommand = new RelayCommand(OnPlayPause, CanPlayPause);
            MuteCommand = new RelayCommand(OnMute, CanMute);
            NextCommand = new RelayCommand(OnNext, CanNext);
            PrevCommand = new RelayCommand(OnPrev, CanPrev);
            ShuffleCommand = new RelayCommand(OnShuffle, CanShuffle);
            RepeatCommand = new RelayCommand(OnRepeat, CanRepeat);
        }

        /********** Media Player Time Functions **********/
        public TimeSpan MediaPosition
        {
            get { return _mediaPosition; }
            set
            {
                _mediaPosition = value;
                OnPropertyRaised("MediaPosition");
            }
        }

        public TimeSpan MediaDuration
        {
            get { return _mediaDuration; }
            set
            {
                _mediaDuration = value;
                OnPropertyRaised("MediaDuration");
            }
        }

        public double SliderPosition
        {
            get { return _sliderPosition; }
            set
            {
                _sliderPosition = value;
                OnPropertyRaised("SliderPosition");
                if (_suppressMediaPositionUpdate)
                {
                    _suppressMediaPositionUpdate = false;
                }
                else
                {
                    _mediaPlayer.Position = TimeSpan.FromSeconds(value);
                }
            }
        }

        public double SliderDuration
        {
            get { return _sliderDuration; }
            set
            {
                _sliderDuration = value;
                OnPropertyRaised("SliderDuration");
            }
        }

        /********** Media Player Volume Functions **********/
        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                OnPropertyRaised("Volume");
                _mediaPlayer.Volume = value / 100.0;
            }
        }

        private void OnMute(object obj)
        {
            if (_isMuted)
            {
                Volume = _volumeBeforeMute;
                VolumeButtonKind = PackIconKind.VolumeHigh;
                _isMuted = false;
            }
            else
            {
                _volumeBeforeMute = Volume;
                Volume = 0.0;
                VolumeButtonKind = PackIconKind.VolumeMute;
                _isMuted = true;
            }
        }

        private bool CanMute(object obj) { return true; }

        public PackIconKind VolumeButtonKind
        {
            get { return _volumeButtonKind; }
            set
            {
                _volumeButtonKind = value;
                OnPropertyRaised("VolumeButtonKind");
            }
        }

        /********** Media Player Active Media List and Current Media **********/
        public List<Media> ActiveMediaList
        {
            get { return _activeMediaList; }
            set
            {
                _activeMediaList = value;
                OnPropertyRaised("ActiveMediaList");
            }
        }

        public Media CurrentMedia
        {
            get { return _currentMedia; }
            set
            {
                _currentMedia = value;
                OnPropertyRaised("CurrentMedia");
                if (value != null)
                {
                    _mediaPlayer.Open(new Uri(value.FilePath));
                    if (value.GetType() == typeof(Song))
                    {
                        AlbumCover = AlbumSearch((Song)CurrentMedia);
                    }

                    _mediaPlayer.Stop();
                }
            }
        }

        public string AlbumSearch(Song toFind)
        {
            string albumTitle = toFind.AlbumTitle;
            foreach (Album album in MainWindowVM.Music.TotalAlbumList)
            {
                if (albumTitle == album.Title)
                {
                    return album.CoverFilePath;
                }
            }
            return "";
        }

        public string AlbumCover
        {
            get { return _albumCover; }
            set
            {
                _albumCover = value;
                OnPropertyRaised("AlbumCover");
            }
        }

        /********** Media Player Basic Functions **********/
        public void PlayNewMedia()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Play();
            _isPaused = false;
            PlayButtonKind = PackIconKind.PauseCircleOutline;
        }

        public void PlayMedia()
        {
            _mediaPlayer.Play();
            _isPaused = false;
            PlayButtonKind = PackIconKind.PauseCircleOutline;
        }

        public void PauseMedia()
        {
            _mediaPlayer.Pause();
            _isPaused = true;
            PlayButtonKind = PackIconKind.PlayCircleOutline;
        }

        public void StopMedia()
        {
            _mediaPlayer.Stop();
            _isPaused = true;
            PlayButtonKind = PackIconKind.PlayCircleOutline;
        }

        /********** Media Player Advanced Functions **********/

        // Play Pause Button Functions
        private void OnPlayPause(object obj)
        {
            if (_isPaused)
            {
                PlayMedia();
            }
            else
            {
                PauseMedia();
            }
        }

        private bool CanPlayPause(object obj)
        {
            if (_mediaPlayer.Source == null)
            {
                return false;
            }
            else
                return true;
        }

        public PackIconKind PlayButtonKind
        {
            get { return _playButtonKind; }
            set
            {
                _playButtonKind = value;
                OnPropertyRaised("PlayButtonKind");
            }
        }

        // Next Button Functions
        private void OnNext(object obj)
        {
            if (_repeatValue == 0)
            {
                if (ActiveListIndex < ActiveMediaList.Count - 1)
                {
                    ++ActiveListIndex;
                    CurrentMedia = ActiveMediaList[ActiveListIndex];
                    MainWindowVM.SongsVM.SongSearch((Song)CurrentMedia);
                    PlayNewMedia();
                }
                else
                {
                    ActiveListIndex = 0;
                    CurrentMedia = ActiveMediaList[0];
                    MainWindowVM.SongsVM.SongSearch((Song)CurrentMedia);
                    StopMedia();
                }
            }
            else if (_repeatValue == 1)
            {
                if (ActiveListIndex < ActiveMediaList.Count - 1)
                {
                    ++ActiveListIndex;
                }
                else
                {
                    ActiveListIndex = 0;
                }
                CurrentMedia = ActiveMediaList[ActiveListIndex];
                MainWindowVM.SongsVM.SongSearch((Song)CurrentMedia);
                PlayNewMedia();
            }
            else // _repeatValue == 2
            {
                PlayNewMedia();
            }
        }

        private bool CanNext(object obj)
        {
            //if (ActiveMediaList.Count > 1)
            //{
            //    return true;
            //}
            //return false;
            return true;
        }

        // Previous Button Functions
        private void OnPrev(object obj)
        {

            int prevThreshold = 15; 

            if (_repeatValue == 0)
            {
                if (MediaPosition.Seconds >= prevThreshold)
                {
                    PlayNewMedia();
                }
                else
                {
                    if (ActiveListIndex > 0)
                    {
                        --ActiveListIndex;
                        CurrentMedia = ActiveMediaList[ActiveListIndex];
                        MainWindowVM.SongsVM.SongSearch((Song)CurrentMedia);
                        PlayNewMedia();
                    }
                    else
                    {
                        PlayNewMedia();
                    }
                }
            }
            else if (_repeatValue == 1)
            {
                if (MediaPosition.Seconds >= prevThreshold)
                {
                    PlayNewMedia();
                }
                else
                {
                    if (ActiveListIndex > 0)
                    {
                        --ActiveListIndex;
                    }
                    else
                    {
                        ActiveListIndex = ActiveMediaList.Count - 1;
                    }
                    CurrentMedia = ActiveMediaList[ActiveListIndex];
                    MainWindowVM.SongsVM.SongSearch((Song)CurrentMedia);
                    PlayNewMedia();
                }
            }
            else
            {
                PlayNewMedia();
            }
        }

        private bool CanPrev(object obj)
        {
            //if (ActiveMediaList.Count > 1)
            //{
            //    return true;
            //}
            //return false;
            return true;
        }

        // Shuffle Button Functions
        private void OnShuffle(object obj)
        {
            if (_shuffleOn)
            {
                _shuffleOn = false;
                ShuffleColor = (Brush)Application.Current.Resources["PrimaryHueMidBrush"];
            }
            else
            {
                _shuffleOn = true;
                //CreateShuffleList();
                ShuffleColor = (Brush)Application.Current.Resources["SecondaryAccentBrush"];
                _repeatValue = 0;
                RepeatButtonKind = PackIconKind.Repeat;
                RepeatColor = (Brush)Application.Current.Resources["PrimaryHueMidBrush"];
            }
        }

        private void CreateShuffleList()
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < ActiveMediaList.Count; ++i)
            {
                indexList.Add(i);
            }

            int upperLimit = ActiveMediaList.Count;
            List<int> shuffledIndexList = new List<int>();
            while(upperLimit > 1)
            {
                if (upperLimit == ActiveMediaList.Count)
                {
                    shuffledIndexList.Add(ActiveListIndex);
                    indexList.Remove(ActiveListIndex);
                }
                else
                {
                    int randIndex = rand.Next(0, upperLimit);
                    shuffledIndexList.Add(randIndex);
                    indexList.Remove(randIndex);
                }
                --upperLimit;
            }

            foreach (int i in shuffledIndexList)
            {
                _shuffleList.Add(ActiveMediaList[i]);
            }
        }

        private bool CanShuffle(object obj)
        {
            return true;
        }

        public Brush ShuffleColor
        {
            get { return _shuffleColor; }
            set
            {
                _shuffleColor = value;
                OnPropertyRaised("ShuffleColor");
            }
        }

        // Repeat Button Functions
        private void OnRepeat(object obj)
        {
            if (_repeatValue == 0)
            {
                _repeatValue = 1;
                RepeatColor = (Brush)Application.Current.Resources["SecondaryAccentBrush"];
                _shuffleOn = false;
                ShuffleColor = (Brush)Application.Current.Resources["PrimaryHueMidBrush"];
            }
            else if (_repeatValue == 1)
            {
                _repeatValue = 2;
                RepeatButtonKind = PackIconKind.RepeatOnce;
                _shuffleOn = false;
                ShuffleColor = (Brush)Application.Current.Resources["PrimaryHueMidBrush"];
            }
            else // _repeatValue == 2
            {
                _repeatValue = 0;
                RepeatButtonKind = PackIconKind.Repeat;
                RepeatColor = (Brush)Application.Current.Resources["PrimaryHueMidBrush"];
                //_shuffleOn = false;
                //ShuffleColor = (Brush)Application.Current.Resources["PrimaryHueMidBrush"];
            }
        }

        private bool CanRepeat(object obj)
        {
            return true;
        }

        public PackIconKind RepeatButtonKind
        {
            get { return _repeatButtonKind; }
            set
            {
                _repeatButtonKind = value;
                OnPropertyRaised("RepeatButtonKind");
            }
        }

        public Brush RepeatColor
        {
            get { return _repeatColor; }
            set
            {
                _repeatColor = value;
                OnPropertyRaised("RepeatColor");
            }
        }

        /********** Media Player Event Functions **********/
        private void OnMediaOpen(object sender, EventArgs e)
        {
            SliderDuration = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            SliderPosition = 0.0;
            MediaDuration = _mediaPlayer.NaturalDuration.TimeSpan;
            _timer.Start();
        }

        private void OnMediaEnd(object sender, EventArgs e)
        {
            _mediaPlayer.Stop();
            if (_repeatValue == 0)
            {
                if (ActiveListIndex < ActiveMediaList.Count - 1)
                {
                    ++ActiveListIndex;
                    CurrentMedia = ActiveMediaList[ActiveListIndex];
                    MainWindowVM.SongsVM.SongSearch((Song)CurrentMedia);
                    PlayNewMedia();
                }
                else
                {
                    ActiveListIndex = 0;
                    CurrentMedia = ActiveMediaList[0];
                    MainWindowVM.SongsVM.SongSearch((Song)CurrentMedia);
                    StopMedia();
                }
            }
            else if (_repeatValue == 1)
            {
                if (ActiveListIndex < ActiveMediaList.Count - 1)
                {
                    ++ActiveListIndex;
                }
                else
                {
                    ActiveListIndex = 0;
                }
                CurrentMedia = ActiveMediaList[ActiveListIndex];
                MainWindowVM.SongsVM.SongSearch((Song)CurrentMedia);
                PlayNewMedia();
            }
            else // _repeatValue == 2
            {
                PlayNewMedia();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_mediaPlayer.Source != null)
            {
                MediaPosition = _mediaPlayer.Position;
                _suppressMediaPositionUpdate = true;
                SliderPosition = _mediaPlayer.Position.TotalSeconds;
            }
        }

        /********** MainWindowViewModel **********/
        public MainWindowViewModel MainWindowVM
        {
            get { return _mainWindowVM; }
            set
            {
                _mainWindowVM = value;
                OnPropertyRaised("MainWindowVM");
            }
        }

        /********** Media Player Control Command Functions **********/
        public ICommand PlayPauseCommand { get; set; }

        public ICommand MuteCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        public ICommand ShuffleCommand { get; set; }

        public ICommand RepeatCommand { get; set; }
    }
}
