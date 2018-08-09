using MediaDataTypes.MusicDataTypes;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MediaLibraryGUI.ViewModels
{
    public class AddMusicViewModel : BaseViewModel
    {
        private string _artistName;
        private string _artistPhotoFilePath;
        private string _artistDescription;

        private Album _newAlbum;
        private string _albumTitle;
        private string _albumArtist;
        private string _albumYear;
        private string _albumNumTracks;
        private string _albumFilePath;
        private string _albumCoverFilePath;
        private List<Song> _albumSongList;

        private bool _selectAlbumFolderEnabled;
        private bool _flipCardEnabled;
        private Visibility _submissionVisibility;

        private MainWindowViewModel _mainWindowVM;

        public AddMusicViewModel()
        {
            SelectAlbumFolderEnabled = false;
            FlipCardEnabled = false;
            AlbumSongList = new List<Song>();
            SelectArtistPhot = new RelayCommand(OnSelectArtistPhoto, CanSelectArtistPhoto);
            SelectAlbumFolder = new RelayCommand(OnSelectAlbumFolder, CanSelectAlbumFolder);
            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
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

        /********** Artist Information **********/
        public string ArtistName
        {
            get { return _artistName; }
            set
            {
                _artistName = value;
                OnPropertyRaised("ArtistName");
                if (!String.IsNullOrEmpty(value))
                {
                    AlbumArtist = value;
                    bool found = false;
                    foreach (Artist artist in MainWindowVM.Music.TotalArtistList)
                    {
                        if (artist.Name == _artistName)
                        {
                            ArtistPhotoFilePath = artist.PhotoFilePath;
                            ArtistDescription = artist.Description;
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        ArtistPhotoFilePath = null;
                        ArtistDescription = null;
                    }
                }
            }
        }

        public string ArtistPhotoFilePath
        {
            get { return _artistPhotoFilePath; }
            set
            {
                _artistPhotoFilePath = value;
                OnPropertyRaised("ArtistPhotoFilePath");
            }
        }

        public string ArtistDescription
        {
            get { return _artistDescription; }
            set
            {
                _artistDescription = value;
                OnPropertyRaised("ArtistDescription");
            }
        }

        /********** Album Information **********/
        public string AlbumTitle
        {
            get { return _albumTitle; }
            set
            {
                _albumTitle = value;
                if (AlbumArtist.Length > 0 && value.Length > 0)
                {
                    SelectAlbumFolderEnabled = true;
                }
                else
                {
                    SelectAlbumFolderEnabled = false;
                }
                OnPropertyRaised("AlbumTitle");
            }
        }

        public string AlbumArtist
        {
            get { return _albumArtist; }
            set
            {
                _albumArtist = value;
                OnPropertyRaised("AlbumArtist");
            }
        }

        public string AlbumYear
        {
            get { return _albumYear; }
            set
            {
                _albumYear = value;
                OnPropertyRaised("AlbumYear");
            }
        }

        public string AlbumNumTracks
        {
            get { return _albumNumTracks; }
            set
            {
                _albumNumTracks = value;
                OnPropertyRaised("AlbumNumTracks");
            }
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

        public string AlbumCoverFilePath
        {
            get { return _albumCoverFilePath; }
            set
            {
                _albumCoverFilePath = value;
                OnPropertyRaised("AlbumCoverFilePath");
            }
        }

        public List<Song> AlbumSongList
        {
            get { return _albumSongList; }
            set
            {
                _albumSongList = value;
                OnPropertyRaised("AlbumSongList");
            }
        }

        public void OnSelectArtistPhoto(object obj)
        {
            CommonOpenFileDialog selectArtistPhotoDialog = new CommonOpenFileDialog();
            selectArtistPhotoDialog.IsFolderPicker = false;
            selectArtistPhotoDialog.Title = "Select Artist Photo Location";
            selectArtistPhotoDialog.InitialDirectory = @"C:\Users\Daniel Rojas\Music\MP3Library\Artists\";
            selectArtistPhotoDialog.DefaultDirectory = @"C:\Users\Daniel Rojas\Music\MP3Library\Artists\";
            selectArtistPhotoDialog.EnsureFileExists = true;
            selectArtistPhotoDialog.EnsurePathExists = true;
            selectArtistPhotoDialog.EnsureReadOnly = false;
            selectArtistPhotoDialog.EnsureValidNames = true;
            selectArtistPhotoDialog.Multiselect = false;
            selectArtistPhotoDialog.ShowPlacesList = true;

            if (selectArtistPhotoDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ArtistPhotoFilePath = selectArtistPhotoDialog.FileName;
            }
        }

        public bool CanSelectArtistPhoto(object obj) { return true; }

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
            string[] fileList;
            List<Song> songList = new List<Song>();
            if (selectAlbumDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                albumFilePath = selectAlbumDialog.FileName;
                AlbumFilePath = albumFilePath;
                fileList = Directory.GetFiles(AlbumFilePath);

                int index = 0;
                foreach (string fileName in fileList)
                {
                    if (fileName.Substring(fileName.Length - 3) == "jpg" || fileName.Substring(fileName.Length - 3) == "png")
                    {
                        AlbumCoverFilePath = fileName;
                    }
                    else
                    {
                        int songTitleIndex = fileName.LastIndexOf('\\');
                        string songFile = fileName.Substring(songTitleIndex + 1);
                        string songTitle = songFile.Substring(0, songFile.Length - 4);

                        Song newSong = new Song(fileName, songTitle, AlbumArtist);
                        newSong.AlbumTitle = AlbumTitle;
                        newSong.TrackNumber = (uint)index + 1;
                        songList.Add(newSong);
                        ++index;
                    }
                }
                FlipCardEnabled = true;
                AlbumSongList = songList;
                SubmissionVisibility = Visibility.Hidden;
            }
        }

        public bool CanSelectAlbumFolder(object obj) { return true; }

        public bool SelectAlbumFolderEnabled
        {
            get { return _selectAlbumFolderEnabled; }
            set
            {
                _selectAlbumFolderEnabled = value;
                OnPropertyRaised("SelectAlbumFolderEnabled");
            }
        }

        public bool FlipCardEnabled
        {
            get { return _flipCardEnabled; }
            set
            {
                _flipCardEnabled = value;
                OnPropertyRaised("FlipCardEnabled");
            }

        }

        public void OnSubmit(object obj)
        {
            Artist artist = new Artist(ArtistName);
            artist.PhotoFilePath = ArtistPhotoFilePath;
            artist.Description = ArtistDescription;

            Album album = new Album(AlbumTitle, AlbumArtist);
            uint albumYear = 0;
            uint.TryParse(AlbumYear, out albumYear);
            album.Year = albumYear;
            uint albumNumTracks = 0;
            uint.TryParse(AlbumNumTracks, out albumNumTracks);
            album.NumberOfTracks = albumNumTracks;
            album.CoverFilePath = AlbumCoverFilePath;

            bool foundArtist = false;
            foreach (Artist currentArtist in MainWindowVM.Music.TotalArtistList)
            {
                if (artist.Name == currentArtist.Name)
                {
                    foundArtist = true;
                    currentArtist.Description = artist.Description;
                    currentArtist.PhotoFilePath = artist.PhotoFilePath;
                    artist = currentArtist;
                }
            }
            if (!foundArtist)
            {
                MainWindowVM.Music.TotalArtistList.Add(artist);
            }

            bool foundAlbum = false;
            foreach (Album currentAlbum in MainWindowVM.Music.TotalAlbumList)
            {
                if (album.Title == currentAlbum.Title && album.Artist == currentAlbum.Artist)
                {
                    foundAlbum = true;
                    currentAlbum.Year = album.Year;
                    currentAlbum.NumberOfTracks = album.NumberOfTracks;
                    currentAlbum.CoverFilePath = album.CoverFilePath;
                    currentAlbum.Artist = album.Artist;

                    album = currentAlbum;

                }
            }
            if (!foundAlbum)
            {
                MainWindowVM.Music.TotalAlbumList.Add(album);
                artist.AlbumList.Add(album);
            }
            album.SongList.AddRange(AlbumSongList);
            MainWindowVM.Music.TotalSongList.AddRange(AlbumSongList);

            CleanForm();
            SubmissionVisibility = Visibility.Visible;
            FlipCardEnabled = false;
        }

        public Visibility SubmissionVisibility
        {
            get { return _submissionVisibility; }
            set
            {
                _submissionVisibility = value;
                OnPropertyRaised("SubmissionVisibility");
            }
        }

        public bool CanSubmit(object obj)
        {
            return true;
        }

        public void CleanForm()
        {
            ArtistName = "";
            ArtistPhotoFilePath = "";
            ArtistDescription = "";
            AlbumTitle = "";
            AlbumYear = "";
            AlbumNumTracks = "";
            AlbumFilePath = "";
            AlbumSongList.Clear();
        }

        public ICommand SelectArtistPhot { get; set; }

        public ICommand SelectAlbumFolder { get; set; }

        public ICommand SubmitCommand { get; set; }

    }
}
