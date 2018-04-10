using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayerDemo
{
    using MediaInterfaces;
    using MusicAccess;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool suppresssaMediaPositionUpdate = false;
        private double volumeBeforeMute = 0.00;
        private Song currentSong;
        private int currentSongIndex = 0;
        private int previousSongIndex = 0;
        private int nextSongIndex = 0;
        
        

        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            MusicInterface music = new MusicInterface(false);
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            SongListView.ItemsSource = music.totalSongList;
           
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {

            AudioPosition.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            durationTime.Content = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            timer.Start();
            //throw new NotImplementedException();
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {

            SongListView.SelectedItems[0] = SongListView.Items[nextSongIndex];
            //throw new NotImplementedException();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                currentTime.Content = mediaPlayer.Position.ToString(@"mm\:ss");
                suppresssaMediaPositionUpdate = true;
                AudioPosition.Value = mediaPlayer.Position.TotalSeconds;
            }
        }

        private void btnPlay_Pause_Click(object sender, RoutedEventArgs e)
        {
            if (btnPlay_Pause.Content.ToString() == "Play")
            {
                mediaPlayer.Play();
                btnPlay_Pause.Content = "Pause";
            }
            else
            {
                mediaPlayer.Pause();
                btnPlay_Pause.Content = "Play";
            }
            
        }
        
        private int findSelectedItem(Song song)
        {
            for (int i = 0; i < SongListView.Items.Count; ++i)
            {
                Song iteratorSong = (Song)SongListView.Items[i];
                if (iteratorSong.filePath.Equals(song.filePath))
                {
                    return i;
                }
            }
            return -1;
        }

        private void setNextPrevIndex()
        {
            if (currentSongIndex <= 0)
            {
                previousSongIndex = 0;
                if (SongListView.Items.Count > 1)
                {
                    nextSongIndex = currentSongIndex + 1;
                }
                else
                {
                    nextSongIndex = 0;
                }
            }
            else if (currentSongIndex == SongListView.Items.Count  - 1)
            {
                previousSongIndex = currentSongIndex - 1;
                nextSongIndex = currentSongIndex;
            }
            else
            {
                previousSongIndex = currentSongIndex - 1;
                nextSongIndex = currentSongIndex + 1;
            }

            

        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (previousSongIndex == currentSongIndex)
            {
                playNewSong();
            }
            else
            {
                SongListView.SelectedItems[0] = SongListView.Items[previousSongIndex];
            }

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (nextSongIndex == currentSongIndex)
            {
                playNewSong();
            }
            else
            {
                SongListView.SelectedItems[0] = SongListView.Items[nextSongIndex];
            }

        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.Volume == 0)
            {
                sldrVolume.Value = volumeBeforeMute;
                btnMute.Content = "Mute";
            }
            else
            {
                volumeBeforeMute = mediaPlayer.Volume * 100;
                sldrVolume.Value = 0;
                btnMute.Content = "Unmute";
            }
            

        }

        private void sldrVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)sldrVolume.Value / 100.0;
        }

        private void AudioPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (suppresssaMediaPositionUpdate)
            {
                suppresssaMediaPositionUpdate = false;
            }
            else
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(AudioPosition.Value);
            }
        }

        private void SongListView_doubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SongListView.SelectedItems.Count > 0)
            {

                currentSong = (Song)SongListView.SelectedItems[0];
                playNewSong();
            }
        }

        private void SongListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentSong = (Song)SongListView.SelectedItems[0];
            playNewSong();
        }

        private void playNewSong()
        {
            mediaPlayer.Open(new Uri(currentSong.filePath));
            mediaPlayer.Stop();
            mediaPlayer.Play();
            currentSongIndex = findSelectedItem(currentSong);
            setNextPrevIndex();
        }

        private void SongListView_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
