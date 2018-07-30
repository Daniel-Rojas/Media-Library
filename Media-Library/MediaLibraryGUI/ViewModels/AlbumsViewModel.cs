using MediaDataTypes.MusicDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MediaLibraryGUI.ViewModels
{
    public class AlbumsViewModel : BaseViewModel
    {
        private Artist _currentArtist;
        private List<Album> _albumList;
        private int _selectedAlbum;
        private MainWindowViewModel _mainWindowVM;

        public AlbumsViewModel()
        {
            _albumList = new List<Album>();
            BackCommand = new RelayCommand(OnBack, CanBack);
        }

        public Artist CurrentArtist
        {
            get { return _currentArtist; }
            set
            {
                _currentArtist = value;
                OnPropertyRaised("CurrentArtist");
            }
        }

        public List<Album> AlbumList
        {
            get { return _albumList; }
            set
            {
                _albumList = value;
                OnPropertyRaised("AlbumList");
            }
        }

        public int SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                _selectedAlbum = value;
                OnPropertyRaised("SelectedAlbum");
                if (value > -1)
                {
                    Album album = _albumList[_selectedAlbum];
                    UserControl songsView = MainWindowVM.SongsView;
                    SongsViewModel songsVM = MainWindowVM.SongsVM;

                    songsVM.SongList = album.SongList;
                    songsVM.CurrentAlbum = album;
                    MainWindowVM.MainFrameContent = MainWindowVM.SongsView;
                    //songsVM.SongSearch((Song)MainWindowVM.MediaPlayerVM.CurrentMedia);
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
            MainWindowVM.MainFrameContent = MainWindowVM.ArtistView;
            MainWindowVM.ArtistsVM.SelectedArtist = -1;
        }
        private bool CanBack(object obj) { return true; }

        public ICommand BackCommand { get; set; }
    }
}
