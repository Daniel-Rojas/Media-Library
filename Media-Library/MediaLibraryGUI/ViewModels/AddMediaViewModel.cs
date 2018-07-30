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
    public class AddMediaViewModel : BaseViewModel
    {
        private MainWindowViewModel _mainWindowVM;
        private UserControl _addMediaFrameContent;




        public AddMediaViewModel()
        {



            AddMovieCommand = new RelayCommand(OnAddMovie, CanAddMovie);
            AddMusicCommand = new RelayCommand(OnAddMusic, CanAddMusic);
            AddTVShowCommand = new RelayCommand(OnAddTVShow, CanAddTVShow);
        }

        public UserControl AddMediaFrameContent
        {
            get { return _addMediaFrameContent; }
            set
            {
                _addMediaFrameContent = value;
                OnPropertyRaised("AddMediaFrameContent");
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

        /********** Add Media Selection Buttons **********/
        public void OnAddMusic(object obj)
        {
            AddMediaFrameContent = new AddMusicView();

        }

        public bool CanAddMusic(object obj)
        {
            return true;
        }

        public void OnAddMovie(object obj)
        {

        }

        public bool CanAddMovie(object obj)
        {
            return true;
        }

        public void OnAddTVShow(object obj)
        {

        }

        public bool CanAddTVShow(object obj)
        {
            return true;
        }

        public ICommand AddMusicCommand { get; set; }

        public ICommand AddMovieCommand { get; set; }

        public ICommand AddTVShowCommand { get; set; }
    }
}
