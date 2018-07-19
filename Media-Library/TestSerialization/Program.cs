using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaDataTypes;
using MediaInterfaces;

namespace TestSerialization
{
    class Program
    {

        static public MusicInterface music;

        static public void intialize()
        {
            music = new MusicInterface();
        }

        static public void displayMainMenu()
        {
            Console.WriteLine("Basic Menu (Check data intialization)");
            Console.WriteLine("A: Display Song List");
            Console.WriteLine("B: Display Album List");
            Console.WriteLine("C: Display Artist List");
            Console.WriteLine("E: Exit Program");

            mainMenuInput();
        }

        static public void mainMenuInput()
        {
            string input = Console.ReadLine().ToUpper();
            if (input == "A")
            {
                displaySongList();
                Console.WriteLine("\nPress the spacebar return back to main menu.");
                Console.ReadKey();
                Console.WriteLine();
                displayMainMenu();

            }
            else if (input == "B")
            {
                displayAlbumList();
                Console.WriteLine("\nPress the spacebar to return back to main menu.");
                Console.ReadKey();
                Console.WriteLine();
                displayMainMenu();
            }
            else if (input == "C")
            {
                displayArtistList();
                Console.WriteLine("\nPress the spacebar to return back to main menu.");
                Console.ReadKey();
                Console.WriteLine();
                displayMainMenu();
            }
            else if (input == "E")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Input does not have a function.");
                mainMenuInput();
            }
        }

        static public void displaySongList()
        {
            foreach (Song song in music.totalSongList)
            {
                song.Print();
            }
        }

        static public void displayAlbumList()
        {
            foreach (Album album in music.TotalAlbumList)
            {
                album.Print();
                Console.WriteLine();
                album.PrintSongList();
                Console.WriteLine("\n");
            }
        }

        static public void displayArtistList()
        {
            foreach (Artist artist in music.TotalArtistList)
            {
                artist.Print();
                Console.WriteLine();
                artist.PrintAlbumList();
                Console.WriteLine("\n");
            }
        }



        static void Main(string[] args)
        {
            intialize();
            Console.WriteLine("Importing Data...");
            Console.WriteLine("Press any key to to continue...");
            Console.ReadKey();
            Console.Clear();

            displayMainMenu();
            Console.ReadKey();
        }
    }
}
