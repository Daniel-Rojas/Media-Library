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
using System.Windows.Threading;
using MusicDataTypes;
using MediaInterfaces;

namespace MusicPlayerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool suppresssaMediaPositionUpdate = false;
        private double volumeBeforeMute = 0.00;
        private Song currentSong;
        private DispatcherTimer timer;
        private int currentIndex;

        public MainWindow()
        {
            InitializeComponent();
            MusicInterface music = new MusicInterface();
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            SongListView.ItemsSource = music.totalSongList;
            currentSong = (Song)SongListView.Items[0];
            mediaPlayer.Open(new Uri(currentSong.FilePath));
            SongListView.SelectedIndex = 0;

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
            SongListView.SelectedItems[0] = SongListView.Items[currentIndex + 1];
            currentSong = (Song)SongListView.Items[currentIndex + 1];
            currentIndex = currentIndex + 1;
            playNewSong();
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

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex == 0)
            {
                playNewSong();
            }
            else
            {
                SongListView.SelectedItems[0] = SongListView.Items[currentIndex - 1];
                currentSong = (Song)SongListView.Items[currentIndex - 1];
                currentIndex = currentIndex - 1;
                playNewSong();
            }

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex == SongListView.Items.Count - 1)
            {
                playNewSong();
            }
            else
            {
                SongListView.SelectedItems[0] = SongListView.Items[currentIndex + 1];
                currentSong = (Song)SongListView.Items[currentIndex + 1];
                currentIndex = currentIndex + 1;
                playNewSong();
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
                currentIndex = SongListView.SelectedIndex;
                playNewSong();
            }
        }

        private void playNewSong()
        {
            mediaPlayer.Open(new Uri(currentSong.FilePath));
            mediaPlayer.Stop();
            mediaPlayer.Play();
        }
    }
}
