using MaterialDesignThemes.Wpf;
using MediaDataTypes.MusicDataTypes;
using MediaLibraryGUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MediaLibraryGUI.ViewModels
{
    public class ArtistsViewModel : BaseViewModel
    {
        private List<Artist> _artistList;
        private int _selectedArtist;
        private MainWindowViewModel _mainWindowVM;

        public ArtistsViewModel()
        {
            _artistList = new List<Artist>();
            SelectedArtist = -1;
        }

        public List<Artist> ArtistList
        {
            get { return _artistList; }
            set
            {
                _artistList = value;
                OnPropertyRaised("ArtistList");
            }
        }

        public int SelectedArtist
        {
            get { return _selectedArtist; }
            set
            {
                _selectedArtist = value;
                OnPropertyRaised("SelectedArtist");
                if (value > -1)
                {
                    Artist artist = _artistList[_selectedArtist];
                    UserControl albumsView = MainWindowVM.AlbumsView;
                    AlbumsViewModel albumsVM = MainWindowVM.AlbumsVM;

                    albumsVM.AlbumList = artist.AlbumList;
                    albumsVM.CurrentArtist = artist;
                    MainWindowVM.MainFrameContent = albumsView;
                    albumsVM.SelectedAlbum = -1;
                    MainWindowVM.MusicMenuSelected = -1;
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
    }
}
