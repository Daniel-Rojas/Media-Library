using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MediaLibraryGUI.ViewModels
{
    public class AddMusicViewModel : BaseViewModel
    {
        private string _albumFilePath;

        public AddMusicViewModel()
        {
            SelectAlbumFolder = new RelayCommand(OnSelectAlbumFolder, CanSelectAlbumFolder);
        }

        public string AlbumFilePath
        {
            get { return _albumFilePath; }
            set
            {
                _albumFilePath = value;
                OnPropertyRaised("AlbumFilePath");
            }
        }

        public void OnSelectAlbumFolder(object obj)
        {
            CommonOpenFileDialog selectAlbumDialog = new CommonOpenFileDialog();
            selectAlbumDialog.IsFolderPicker = true;
            selectAlbumDialog.Title = "Select Album Folder Location";
            selectAlbumDialog.InitialDirectory = @"C:\Users\Daniel Rojas\Music\MP3Library\Artists\";
            selectAlbumDialog.DefaultDirectory = @"C:\Users\Daniel Rojas\Music\MP3Library\Artists\";
            selectAlbumDialog.EnsureFileExists = true;
            selectAlbumDialog.EnsurePathExists = true;
            selectAlbumDialog.EnsureReadOnly = false;
            selectAlbumDialog.EnsureValidNames = true;
            selectAlbumDialog.Multiselect = false;
            selectAlbumDialog.ShowPlacesList = true;


            string albumFilePath;
            if (selectAlbumDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                albumFilePath = selectAlbumDialog.FileName;
                AlbumFilePath = albumFilePath;
                string[] fileList = Directory.GetFiles(AlbumFilePath);

            }
        }

        public bool CanSelectAlbumFolder(object obj) { return true; }

        public ICommand SelectAlbumFolder { get; set; }
    }
}
