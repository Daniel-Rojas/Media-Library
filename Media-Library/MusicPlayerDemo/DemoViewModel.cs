using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MediaInterfaces;
using MusicDataTypes;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MusicPlayerDemo
{
    public class DemoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly MusicInterface _music; // MusicInterface provides the music data and function to modify data
        private readonly MediaPlayer _mediaPlayer; // Media Player provides functionality of the player
        private readonly DispatcherTimer _timer; // provides the time functionality and media position functionality

        private TimeSpan _audioPosition;
        private TimeSpan _audioDuration;
        private double _sliderPosition;
        private double _sliderDuration;
        private double _volume;
        private double _volumeBeforeMute;
        private bool _isPaused = true;



        private int _listIndex;
        private Song _currentSong;


        


        public DemoViewModel()
        {
            _music = new MusicInterface();
            _mediaPlayer = new MediaPlayer();
            _timer = new DispatcherTimer();

            // Initialized the event Functions
            _mediaPlayer.MediaOpened += OnAudioOpen;
            _mediaPlayer.MediaEnded += OnAudioEnd;

            // Initializes the timer even function
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += OnTimerTick;

            // Initializes the first song and the index
            _currentSong = _music.totalSongList[0];
            _mediaPlayer.Open(new Uri(CurrentSong.FilePath));
            _listIndex = 0;

            // Initializes the volume to 0.5
            Volume = 0.5;

            PlayCommand = new RelayCommand(OnPlay, CanPlay);
            PauseCommand = new RelayCommand(OnPause, CanPause);
            MuteCommand = new RelayCommand(OnMute, CanMute);
            NextCommand = new RelayCommand(OnNext, CanNext);
            PrevCommand = new RelayCommand(OnPrev, CanPrev);
        }



        /********** Properties **********/
        public MusicInterface Music
        {
            get
            {
                return _music;
            }
        }

        public TimeSpan AudioPosition
        {
            get { return _audioPosition; }
            set
            {
                _audioPosition = value;
                OnPropertyRaised("AudioPosition");
            }
        }

        public TimeSpan AudioDuration
        {
            get { return _audioDuration; }
            set
            {
                _audioDuration = value;
                OnPropertyRaised("AudioDuration");
            }
        }

        public double SliderDuration
        {
            get
            {
                return _sliderDuration;
            }
            set
            {
                _sliderDuration = value;
                OnPropertyRaised("SliderDuration");
            }
        }

        public double SliderPosition
        {
            get
            {
                return _sliderPosition;
            }
            set
            {
                _sliderPosition = value;
                OnPropertyRaised("SliderPosition");
                _mediaPlayer.Position = TimeSpan.FromSeconds(value);
            }
        }

        public Song CurrentSong
        {
            get
            {
                return _currentSong;
            }
            set
            {
                _currentSong = value;
                OnPropertyRaised("CurrentSong");
                _listIndex = _music.totalSongList.FindIndex(value.Equals);
                _mediaPlayer.Open(new Uri(value.FilePath));
                _mediaPlayer.Stop();
                _mediaPlayer.Play();
                _isPaused = false;
                 
            }
        }

        public double Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                OnPropertyRaised("Volume");
                _mediaPlayer.Volume = value;
            }
        }


        /********** Event Functions **********/
        private void OnAudioOpen(object sender, EventArgs e)
        {
            SliderDuration = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            SliderPosition = 0.0;
            AudioDuration = _mediaPlayer.NaturalDuration.TimeSpan;
            _timer.Start();
        }

        private void OnAudioEnd(object sender, EventArgs e)
        {
            _mediaPlayer.Stop();
            if (_listIndex < _music.totalSongList.Count - 1)
            {
                ++_listIndex;
                CurrentSong = _music.totalSongList[_listIndex];
            }
            else
            {
                CurrentSong = _music.totalSongList[0];
                _mediaPlayer.Pause();
                _isPaused = true;
                _mediaPlayer.Stop();
            }
            
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_mediaPlayer.Source != null)
            {
                AudioPosition = _mediaPlayer.Position;
                SliderPosition = _mediaPlayer.Position.TotalSeconds;
            }
        }

        private void OnPlay(object obj)
        {
            _mediaPlayer.Play();
            _isPaused = false;
        }

        private bool CanPlay(object obj)
        {
            if (CurrentSong != null)
            {
                if (_isPaused)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnPause(object obj)
        {
            _mediaPlayer.Pause();
            _isPaused = true;
        }

        private bool CanPause(object obj)
        {
            if (_isPaused)
            {
                return false;
            }
            return true;
        }

        private void OnMute(object obj)
        {
            if (_mediaPlayer.Volume != 0.0)
            {
                _volumeBeforeMute = _volume;
                _mediaPlayer.Volume = 0.0;
                Volume = 0.0;
            }
            else
            {
                _mediaPlayer.Volume = _volumeBeforeMute;
                Volume = _volumeBeforeMute;
            }
        }

        private bool CanMute(object obj)
        {
            return true;
        }

        private void OnNext(object obj)
        {
            ++_listIndex;
            CurrentSong = _music.totalSongList[_listIndex];
        }

        private bool CanNext(object obj)
        {
            if (_listIndex == _music.totalSongList.Count - 1)
            {
                return false;
            }
            return true;
        }

        private void OnPrev(object obj)
        {
            --_listIndex;
            CurrentSong = _music.totalSongList[_listIndex];
        }

        private bool CanPrev(object obj)
        {
            if (_listIndex == 0)
            {
                return false;
            }
            return true;
        }


        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        /********** Commands **********/
        public ICommand PlayCommand { get; set; }

        public ICommand PauseCommand { get; set; }
        
        public ICommand MuteCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }


        
    }
}
