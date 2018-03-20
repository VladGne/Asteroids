using System;
using System.Media;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace Asteroids
{
    class Program
    {       
        private static SoundPlayer BackgroundSound = new SoundPlayer(Properties.Resources.Background);      // Player for sound in menu
        private static int GameFormWidth = 800, GameFormHeight = 600;                                       // Game window parametrs
        public static string SoundMode = "On";                                                                              
        public static int BestScore = 0;                                

        static void Main(string[] args)
        {
            ReadOptionFile();

            if (SoundMode == "On")
                BackgroundSound.PlayLooping();

            for (; ; )                        
                ShowMenu();           
        }

        // Show main menu
        static void ShowMenu()
        {                  
            int OptionNumber = 0;
            Console.Clear();
            Console.WriteLine("\n\t --- ASTEROIDS --- \n\n");
            Console.WriteLine("\n\t *Input Option Number* \n");
            Console.WriteLine("\t 1. Game Start \n");
            Console.WriteLine("\t 2. Controls \n");
            Console.WriteLine("\t 3. Settings \n");
            Console.WriteLine("\t 4. Records \n");
            Console.WriteLine("\t 5. Exit \n");

            try
            {
                OptionNumber = Convert.ToInt32(Console.ReadLine());

                switch (OptionNumber)
                {
                    case 1:
                        StartGame();
                        break;
                    case 2:
                        ShowControls();
                        break;
                    case 3:
                        ShowSettings();
                        break;
                    case 4:
                        ShowRecords();
                        break;
                    case 5:
                        WriteOptionFile();
                        Environment.Exit(0);
                        break;
                    default:
                        ShowMenu();
                        break;
                }
            }

            catch
            {
                ShowMenu();
            }
        }

        // Create and run game window
        static void StartGame()
        {
            Form GameForm = new Form();
            GameForm.Width = GameFormWidth;
            GameForm.Height = GameFormHeight;
            Game.Init(GameForm);
            GameForm.Show();
            Game.Draw();
            Application.Run(GameForm);
        }

        // Show controls menu
        static void ShowControls()
        {
            int OptionNumber = 0;
            Console.Clear();
            Console.WriteLine(Environment.NewLine + "\t --- Controls --- \n\n");
            Console.WriteLine("\t ↑ (Arrow Up) - To Move Up \n");
            Console.WriteLine("\t ↓ (Arrow Down) - To Move Down \n");
            Console.WriteLine("\t Ctrl - To Fire \n\n\n");
            Console.WriteLine("\t 1. Back To Main Menu\n");

            try
            {
                OptionNumber = Convert.ToInt32(Console.ReadLine());
                if (OptionNumber == 1)
                    ShowMenu();
                else
                    ShowControls();
            }

            catch
            {
                ShowControls();
            }
        }

        // Show settings menu
        static void ShowSettings()
        {
            int OptionNumber = 0;
            Console.Clear();
            Console.WriteLine(Environment.NewLine + "\t --- Settings --- \n\n");
            Console.WriteLine("\t Game's Window Width - {0}\n", +GameFormWidth);
            Console.WriteLine("\t Game's Window Hight - {0}\n", +GameFormHeight);
            Console.WriteLine("\t Sound - {0} \n\n\n", SoundMode);
            Console.WriteLine("\t 1. Change Settings\n");
            Console.WriteLine("\t 2. Back To Main Menu\n");

            try
            {
                OptionNumber = Convert.ToInt32(Console.ReadLine());
                switch (OptionNumber)
                {
                    case 1:
                        ChangeSettings();
                        break;
                    case 2:
                        ShowMenu();
                        break;
                    default:
                        ShowSettings();
                        break;
                }
            }

            catch
            {
                ShowSettings();
            }
        }

        // Show records menu
        static void ShowRecords()
        {
            Console.Clear();
            Console.WriteLine(Environment.NewLine + "\t --- Records --- \n\n");
            Console.WriteLine("\t Current Best Record - {0}\n\n", + BestScore);
            Console.WriteLine("\t 1. Back To Main Menu\n");

            try
            {
                if (Convert.ToInt32(Console.ReadLine()) == 1)
                    ShowMenu();
                else
                    ShowRecords();
            }

            catch
            {
                ShowRecords();
            }

        }


        // Changing settings and write them to settings file
        static void ChangeSettings()
        {
            Console.Clear();
            Console.WriteLine(Environment.NewLine + "\t --- Settings Changing --- \n\n");
            Console.WriteLine("\t Current Game's Window Width - {0}\n", + GameFormWidth);
            Console.WriteLine("\t Input New Game's Window Width :\n");

            try
            {
                GameFormWidth = Convert.ToInt32(Console.ReadLine());
            }

            catch
            {
                ChangeSettings();
            }

            Console.WriteLine("\t Current Game's Window Hight - {0}\n", +GameFormHeight);
            Console.WriteLine("\t Input New Game's Window Hight :\n");

            try
            {
                GameFormHeight = Convert.ToInt32(Console.ReadLine());
            }

            catch
            {
                ChangeSettings();
            }

            Console.WriteLine("\t Choice Sound Mode: \n 1. On \n 2. Off\n");
            try
            {
                int Mode = Convert.ToInt32(Console.ReadLine());
                if (Mode == 1)
                {
                    SoundMode = "On";
                    BackgroundSound.PlayLooping();
                }

                else
                {
                    SoundMode = "Off";
                    BackgroundSound.Stop();
                }
            }

            catch
            {
                ChangeSettings();
            }

            WriteOptionFile();
            ShowSettings();
        }

        // Read settings file or create with default if its doesnt exist
        private static void ReadOptionFile()
        {
            try
            {   // Open file if exist
                using (StreamReader Reader = new StreamReader(@"D:\Option.txt"))
                {
                    String Options;
                    // Read while have text
                    while ((Options = Reader.ReadLine()) != null)
                    {
                        string[] Option = Options.Split(' ');
                        GameFormWidth = Convert.ToInt32(Option[0]);
                        GameFormHeight = Convert.ToInt32(Option[1]);
                        SoundMode = Option[2];
                        BestScore = Convert.ToInt32(Option[3]);
                    }
                    Reader.Close();
                }
            }

            catch
            {   // create file with default settings
                using (FileStream Creator = File.Create(@"D:\Option.txt"))
                {
                    string Options = string.Format("{0} {1} {2} {3}", GameFormWidth, GameFormHeight, SoundMode, BestScore);
                    Byte[] info = new UTF8Encoding(true).GetBytes(Options);                   
                    Creator.Write(info, 0, info.Length);
                    Creator.Close();
                }
            }                          
        }

        //Write settings and best score to file
        private static void WriteOptionFile()
        {            
            using (StreamWriter Writer = new StreamWriter(@"D:\Option.txt"))
            {
                string Options = string.Format("{0} {1} {2} {3}", GameFormWidth, GameFormHeight, SoundMode, BestScore);
                Writer.WriteLine(Options);
                Writer.Close();
            }
        }
    }
}
