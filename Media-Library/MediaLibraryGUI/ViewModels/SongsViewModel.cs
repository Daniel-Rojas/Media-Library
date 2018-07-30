using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaInterfaces;
using MediaDataTypes;
using MediaDataTypes.MusicDataTypes;
using System.Windows.Input;

namespace MediaLibraryGUI.ViewModels
{
    public class SongsViewModel : BaseViewModel
    {
        private Album _currentAlbum;
        private List<Song> _songList;
        private int _selectedSongIndex;
        private Song _selectedSongItem;

        private bool _suppressSelect;

        private MainWindowViewModel _mainWindowVM;

        public SongsViewModel()
        {
            _songList = new List<Song>();
            _suppressSelect = false;
            BackCommand = new RelayCommand(OnBack, CanBack);
        }

        public Album CurrentAlbum
        {
            get { return _currentAlbum; }
            set
            {
                _currentAlbum = value;
                OnPropertyRaised("CurrentAlbum");
            }
        }

        public List<Song> SongList
        {
            get { return _songList; }
            set
            {
                _songList = value;
                OnPropertyRaised("SongList");
                if (MainWindowVM.MediaPlayerVM.CurrentMedia != null)
                {
                    SongSearch((Song)MainWindowVM.MediaPlayerVM.CurrentMedia);
                }
                
            }
        }

        public int SelectedSongIndex
        {
            get { return _selectedSongIndex; }
            set
            {
                if (!_suppressSelect)
                {
                    _selectedSongIndex = value;
                    OnPropertyRaised("SelectedSong");
                    if (value > -1)
                    {
                        if (MainWindowVM.MediaPlayerVM.ActiveMediaList != SongList.Cast<Media>().ToList())
                        {
                            MainWindowVM.MediaPlayerVM.ActiveMediaList = SongList.Cast<Media>().ToList();
                        }
                        MainWindowVM.MediaPlayerVM.CurrentMedia = MainWindowVM.MediaPlayerVM.ActiveMediaList[value];
                        MainWindowVM.MediaPlayerVM.ActiveListIndex = value;
                        MainWindowVM.MediaPlayerVM.PlayNewMedia();
                    }
                }
            }
        }

        public Song SelectedSongItem
        {
            get { return _selectedSongItem; }
            set
            {
                _selectedSongItem = value;
                OnPropertyRaised("SelectedSongItem");
            }
        }

        public void SongSearch(Song songToFind)
        {
            for (int i = 0; i < SongList.Count; ++i)
            {
                Song song = SongList[i];
                if (songToFind.Equals(song))
                {
                    _suppressSelect = true;
                    SelectedSongItem = song;
                }
            }
        }

        public MainWindowViewModel MainWindowVM
        {
            get { return _mainWindowVM; }
            set
            {
                _mainWindowVM = value;
                OnPropertyRaised("MainWindowVM");
            }
        }


        private void OnBack(object obj)
        {
            MainWindowVM.MainFrameContent = MainWindowVM.AlbumsView;
            MainWindowVM.AlbumsVM.SelectedAlbum = -1;
        }
        private bool CanBack(object obj) { return true; }

        public ICommand BackCommand { get; set; }
    }
}
