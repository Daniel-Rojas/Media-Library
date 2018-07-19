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
        private Media _currentMedia;
        private bool _isPaused;

        // Properties dealing with Button Content
        private PackIconKind _playButtonKind;
        private PackIconKind _volumeButtonKind;

        // Constructor
        public MediaPlayerViewModel()
        {
            // Initializes the Media Player
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaOpened += OnMediaOpen;
            _mediaPlayer.MediaEnded += OnMediaEnd;
            _volume = 50.0;
            _isPaused = true;
            _isMuted = false;
            //_hasActiveMediaList = false;
            //ActiveMediaList = new List<Media>();

            // Initializes Media Player Button Content
            _volumeButtonKind = PackIconKind.VolumeHigh;
            _playButtonKind = PackIconKind.PlayCircleOutline;

            //Initializes Media Player Timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += OnTimerTick;
            _suppressMediaPositionUpdate = false;

            // Initializes Media Player Control Commands
            PlayPauseCommand = new RelayCommand(OnPlayPause, CanPlayPause);
            MuteCommand = new RelayCommand(OnMute, CanMute);
            NextCommand = new RelayCommand(OnNext, CanNext);
            PrevCommand = new RelayCommand(OnPrev, CanPrev);
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

        /********** Media Player Current Media **********/
        public Media CurrentMedia
        {
            get { return _currentMedia; }
            set
            {
                _currentMedia = value;
                OnPropertyRaised("CurrentMedia");
                if (value != null)
                {
                    //if (_hasActiveMediaList)
                    //{
                    //    _activeListIndex = ActiveMediaList.FindIndex(value.Equals);
                    //}
                    _mediaPlayer.Open(new Uri(value.FilePath));
                    _mediaPlayer.Stop();
                }
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

        private void OnNext(object obj)
        {
            //if (_activeListIndex < ActiveMediaList.Count - 1)
            //{
            //    ++_activeListIndex;
            //    CurrentMedia = ActiveMediaList[_activeListIndex];
            //    _isPaused = false;
            //    PlayButtonKind = PackIconKind.PauseCircleOutline;
            //    PlayNewMedia();
            //}
            //else
            //{
            //    CurrentMedia = ActiveMediaList[0];
            //    _isPaused = true;
            //    PlayButtonKind = PackIconKind.PlayCircleOutline;
            //    _mediaPlayer.Stop();

            //}


            if (OnNextMedia != null)
            {
                OnNextMedia("message");
            }

        }

        private bool CanNext(object obj) { return true; }

        public Action<string> OnNextMedia { get; set; }

        private void OnPrev(object obj)
        {
            //if (_activeListIndex > 0)
            //{
            //    --_activeListIndex;
            //    CurrentMedia = ActiveMediaList[_activeListIndex];
            //    _isPaused = false;
            //    PlayButtonKind = PackIconKind.PauseCircleOutline;
            //    PlayNewMedia();

            //}
            //else
            //{
            //    _isPaused = false;
            //    PlayButtonKind = PackIconKind.PauseCircleOutline;
            //    PlayNewMedia();
            //}

            if (OnPrevMedia != null)
            {
                OnPrevMedia("message");
            }
        }

        private bool CanPrev(object obj) { return true; }

        public Action<string> OnPrevMedia { get; set; }

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
            Type currentMediaType = CurrentMedia.GetType();
            if (currentMediaType == typeof(Song) || currentMediaType == typeof(Episode))
            {
                if (OnNextMedia != null)
                {
                    OnNextMedia("message");
                }
            }

            
            //if (_activeListIndex < ActiveMediaList.Count - 1)
            //{
            //    ++_activeListIndex;
            //    CurrentMedia = ActiveMediaList[_activeListIndex];
            //    PlayNewMedia();
            //}
            //else
            //{
            //    CurrentMedia = ActiveMediaList[0];
            //    _mediaPlayer.Stop();
            //    _isPaused = true; 
            //    PlayButtonKind = PackIconKind.PlayCircleOutline;
            //}
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

        /********** Media Player Control Command Functions **********/
        public ICommand PlayPauseCommand { get; set; }

        public ICommand MuteCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }
    }
}
