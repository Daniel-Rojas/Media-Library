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
        private int _selectedSong;
        private MainWindowViewModel _mainWindowVM;
        private Song _currentSong;

        public SongsViewModel()
        {
            _songList = new List<Song>();
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
            }
        }

        public int SelectedSong
        {
            get { return _selectedSong; }
            set
            {
                _selectedSong = value;
                OnPropertyRaised("SelectedSong");
                if (value > -1)
                {
                    CurrentSong = SongList[value];
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
                if (OnCurrentSong != null)
                {
                    OnCurrentSong("message");
                }
            }
        }

        public Action<string> OnCurrentSong { get; set; }

        public void MoveNext()
        {
            if (SelectedSong < SongList.Count - 1)
            {
                ++SelectedSong;
            }
            else
            {
                SelectedSong = 0;
            }
        }

        public void MovePrev()
        {
            if (SelectedSong > 0)
            {
                --SelectedSong;
            }
            else
            {
                SelectedSong = SongList.Count - 1;
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
