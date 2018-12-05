using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinchAPI;
using System.IO;



namespace Captstone_Project
{
    //**************************************************************************************
    // Application Name: DARTH FINCH
    // Author: Jim Lyons
    // Description: CIT:110 Capstone Project.  A Finch-based game.
    // Date Created: 12/1/2018
    // Date Last Modified: 12/1/2018
    //**************************************************************************************


    enum Difficulty
    {
        MERCIFUL,
        NORMAL,
        RUTHLESS,
    }


    class Program
    {
        static void Main(string[] args)
        {
            // Local Variables:
            Finch myFinch = new Finch();
            FinchOperator myFinchOperator = new FinchOperator();
            bool runApp = true;
            string userName;

            // Setting up the console:
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            // Welcome Screen:
            userName = DisplayWelcomeScreen();

            //
            while (runApp)
            {
                runApp = DisplayMainMenu(userName, myFinch, myFinchOperator);
            }

            DisplayClosingScreen(userName, myFinch);
        }

        #region SCREENS

        /// <summary>
        /// Welcome Screen: Greets the user and asks for their name.
        /// </summary>
        /// <returns></returns>
        static string DisplayWelcomeScreen()
        {
            // Variables:
            string userName;

            // Getting the User's name:
            Console.Clear();
            DisplayHeader("DARTH FINCH");
            Console.WriteLine();
            Console.WriteLine("Please enter player name:");
            Console.Write("-> ");
            userName = Console.ReadLine();

            // Echoing the name to confirm:

            Console.Clear();
            DisplayHeader("DARTH FINCH");
            Console.WriteLine();
            Console.WriteLine("{0}, welcome!", userName);


            DisplayContinuePrompt();

            return userName;
        }

        /// <summary>
        /// Displays the main menu: Main application screen.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="myFinch"></param>
        /// <param name="myFinchOperator"></param>
        /// <returns></returns>
        static bool DisplayMainMenu(string userName, Finch myFinch, FinchOperator myFinchOperator)
        {
            // Local Variables:
            bool runApp;
            string menuChoice;


            DisplayHeader("DARTH FINCH");

            //Display Menu:
            DisplaySpacer();
            Console.WriteLine("\t1.) [PLAY!]");
            Console.WriteLine("\t2.) [How to play]");
            Console.WriteLine("\t3.) [Setup]");
            Console.WriteLine("\t4.) [Save Score]");
            Console.WriteLine("\t5.) [Load Scores]");
            Console.WriteLine();
            Console.WriteLine("\tE) Exit Game");
            DisplaySpacer();
            Console.Write("\tInput -> ");
            menuChoice = Console.ReadLine();

            // Processes menu selection: 
            runApp = MenuSwitchBoard(userName, menuChoice, myFinch, myFinchOperator);

            return runApp;
        }

        /// <summary>
        /// Processes menu selection for DisplayMainMenu.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="menuChoice"></param>
        /// <returns></returns>
        static bool MenuSwitchBoard(string userName, string menuChoice, Finch myFinch, FinchOperator myFinchOperator)
        {
            //Local Variables:
            Monitor saberHit = new Monitor();
            bool runApp = true;

            switch (menuChoice.ToUpper())
            {
                case "1":
                    DisplayGameInProgress(myFinch, myFinchOperator);
                    break;
                case "2":
                    DisplayInformationScreen(userName);
                    break;
                case "3":
                    DisplaySetupMenu(userName, myFinch, myFinchOperator);
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "E":
                    runApp = false;
                    break;
                default:
                    Console.WriteLine("'{0}' is not a valid input. Please try again!", menuChoice);
                    DisplayContinuePrompt();
                    Console.Clear();
                    break;
            }

            return runApp;
        }

        /// <summary>
        /// Gets the desired difficulty from the user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        static void GetDifficultySetting(FinchOperator myFinchOperator)
        {
            // Local Variables:
            Difficulty difficulty;

            foreach (var setting in Enum.GetValues(typeof(Difficulty)))
            {
                Console.WriteLine(setting);
            }

            Console.WriteLine("Select Difficulty -> ");
            while (!Enum.TryParse(Console.ReadLine().ToUpper(), out difficulty))
            {
                Console.WriteLine("That is not a valid difficulty level. Please try again!");
                DisplayContinuePrompt();
            }

            myFinchOperator.DifficultySetting = difficulty;

        }

        /// <summary>
        /// Setup menu: Allows the user to access Finch connection and difficulty settings.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="myFinch"></param>
        /// <param name="myFinchOperator"></param>
        static void DisplaySetupMenu(string userName, Finch myFinch, FinchOperator myFinchOperator)
        {
            // Local Variables:
            string menuChoice;
            bool returnToPrevious = false;

            while (!returnToPrevious)
            {
                DisplayHeader("SET UP");

                //Display Menu:
                DisplaySpacer();
                Console.WriteLine("\t1.) [Connect to Finch Robot]");
                Console.WriteLine("\t2.) [Select Difficulty]");
                Console.WriteLine("\t3.) [Change Player Name]");

                Console.WriteLine("\tM) Return to Main Menu");
                DisplaySpacer();
                Console.Write("\tInput -> ");
                menuChoice = Console.ReadLine();

                // Processes menu selection: 
                returnToPrevious = SettingsSwitchBoard(menuChoice, myFinch, myFinchOperator);
            }

        }

        /// <summary>
        /// Processes menu selection for DisplaySetupMenu
        /// </summary>
        /// <param name="menuChoice"></param>
        /// <param name="myFinch"></param>
        /// <param name="myFinchOperator"></param>
        static bool SettingsSwitchBoard(string menuChoice, Finch myFinch, FinchOperator myFinchOperator)
        {
            // local variables: 
            bool returnToPrevious = false;

            switch (menuChoice.ToUpper())
            {
                case "1":
                    FinchOperator.EstablishFinchConnection(myFinch);
                    break;
                case "2":
                    GetDifficultySetting(myFinchOperator);
                    DifficultySettings(myFinchOperator);
                    break;
                case "3":
                    DisplayWelcomeScreen();
                    break;
                case "M":
                    returnToPrevious = true;
                    break;
                default:
                    break;
            }

            return returnToPrevious;
        }

        /// <summary>
        /// Information Screen: Tells the user how to play and what materials they need.
        /// </summary>
        /// <param name="userName"></param>
        static void DisplayInformationScreen(string userName)
        {
            DisplayHeader("DARTH FINCH: Information:");
            Console.WriteLine();
            Console.WriteLine("\tTo play DARTH FINCH, you will require a Finch robot, a can of compressed air (''carbonite'')\n\tand a flashlight (your 'light saber').");
            Console.WriteLine();
            DisplayContinuePrompt();

            DisplayHeader("DARTH FINCH");
            Console.WriteLine("\t The evil Darth Finch is on the rampage! Only you, {0}, stand any chance of stopping him.", userName);
            Console.WriteLine();
            Console.WriteLine("\tIn order to land a telling blow on the dark lord, you must first drop his defenses.\n\tAs long as Darth Finch's lightsaber is ignited, he will block all attempts made against him.\n\tTo land a blow, you must first spray him with a carbonite blast and then strike while he is reeling.\n\tBe careful! He won't leave his guard down long.");
            Console.WriteLine();
            Console.WriteLine("\tDarth Finch can take a number of hits based on the selected difficulty,\n\twith harder difficulties requiring more strikes to stop him. Good luck!");
            Console.WriteLine();
            DisplayContinuePrompt();
        }

        /// <summary>
        /// Closing Screen:  Thanks the user for using the application and disconnects the Finch.
        /// </summary>
        /// <param name="closingText"></param>
        /// <param name="myFinch"></param>
        static void DisplayClosingScreen(string closingText, Finch myFinch)
        {
            Console.Clear();
            Console.WriteLine("***************[Goodbye!]*****************");
            Console.WriteLine();
            Console.WriteLine("Thank you, {0}, for using this application!", closingText);
            Console.WriteLine();
            Console.WriteLine("Press any key to end the application {0}.", closingText);
            Console.ReadKey();

            myFinch.disConnect();
        }

        /// <summary>
        /// Reads the high scores list from the file.
        /// </summary>
        static void DisplayReadScoreFromFile()
        {
            // Local Variables:
            string dataPath = @"Data\Scores.txt";

            try
            {
                DisplayHeader("Scores:");
                Console.WriteLine();
                Console.WriteLine("Press any key to Read Data from File:");
                Console.ReadKey();
                Console.WriteLine();

                DisplayContinuePrompt();
            }
            catch (FileNotFoundException)
            {
                DisplayHeader("ERROR");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to read data file. The file may have been deleted or corrupted.");
                Console.WriteLine();
            }
            catch (NullReferenceException)
            {
                DisplayHeader("ERROR");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to retrieve data. No data was found.");
                Console.WriteLine();
            }

        }

        #endregion

        #region GAMEPLAY ELEMENTS

        /// <summary>
        /// Runs the gameplay:
        /// </summary>
        /// <param name="myFinch"></param>
        /// <param name="myFinchOperator"></param>
        /// <param name="difficulty"></param>
        static void DisplayGameInProgress(Finch myFinch, FinchOperator myFinchOperator)
        {
            bool isDefeated = false;
            Monitor hitTracker = new Monitor();


            for (int i = 0; i < 255; i++)
            {
                myFinch.setLED(i, 0, 0);
            }

            // FinchOperator.PlayImpMarchShort(myFinch);
            while (!isDefeated)
            {

                hitTracker.IsFrozen = Monitor.FreezeDetection(myFinch);
                hitTracker.IsHit = Monitor.HitDetection(myFinch);

                DisplayVulnerbilityStatus(myFinch, hitTracker, myFinchOperator);


                DisplayHealthStatus(myFinchOperator);
                isDefeated = CheckHealth(myFinchOperator);

                FinchOperator.LEDSettings(myFinch, myFinchOperator);
                /*FinchOperator.MotorSettings(myFinch, difficulty, myFinchOperator);*/
                myFinch.wait(myFinchOperator.vulnerabilityDuration);
            }

            for (int i = 0; i < 255; i++)
            {
                myFinch.setLED(255 - i, 0, 0);
            }
        }

        /// <summary>
        /// Takes the difficulty selected by the player and assigns values for use in other Finch operations based on that difficulty.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        static FinchOperator DifficultySettings(FinchOperator myFinchOperator)
        {
            switch (myFinchOperator.DifficultySetting)
            {
                case Difficulty.MERCIFUL:
                    myFinchOperator.vulnerabilityDuration = 4000;
                    myFinchOperator.MotorSpeed = 100;
                    myFinchOperator.HitPoints = 1;
                    myFinchOperator.ScoreModifier = 5;
                    break;
                case Difficulty.NORMAL:
                    myFinchOperator.vulnerabilityDuration = 2000;
                    myFinchOperator.MotorSpeed = 175;
                    myFinchOperator.HitPoints = 3;
                    myFinchOperator.ScoreModifier = 10;
                    break;
                case Difficulty.RUTHLESS:
                    myFinchOperator.vulnerabilityDuration = 1000;
                    myFinchOperator.MotorSpeed = 255;
                    myFinchOperator.HitPoints = 5;
                    myFinchOperator.ScoreModifier = 20;
                    break;
                default:
                    break;

            }

            return myFinchOperator;
        }

        /// <summary>
        /// Controls whether the Finch can be hurt or not and gives the user a prompt if they have scored a hit. 
        /// </summary>
        static void DisplayVulnerbilityStatus(Finch myFinch, Monitor hitTracker, FinchOperator myFinchOperator)
        {
            if (hitTracker.IsHit == true && hitTracker.IsFrozen == false)
            {
                Console.WriteLine();
                Console.WriteLine("The blast is deflected!");
                //myFinchOperator.IsVulnerable = false;
                FinchOperator.LEDSettings(myFinch, myFinchOperator);
            }

            if (hitTracker.IsHit == false && hitTracker.IsFrozen == true)
            {
                Console.WriteLine();
                Console.WriteLine("That got him! Strike now while he is stunned!");
                myFinchOperator.IsVulnerable = true;
                FinchOperator.LEDSettings(myFinch, myFinchOperator);
                ShieldsDown(myFinch, hitTracker, myFinchOperator);
            }

        }

        static void ShieldsDown(Finch myFinch, Monitor hitTracker, FinchOperator myFinchOperator)
        {
            hitTracker.IsHit = Monitor.HitDetection(myFinch);
            myFinch.wait(myFinchOperator.vulnerabilityDuration);

            if (hitTracker.IsHit == true)
            {
                Console.WriteLine("Nice hit! He'll feel that for sure.");
                myFinchOperator.HitsSuffered += 1;

            }

            else
            {
                Console.WriteLine("The moment to strike has passed. Try again!");

            }
            myFinchOperator.IsVulnerable = false;

            FinchOperator.LEDSettings(myFinch, myFinchOperator);


        }


        /// <summary>
        /// Displays current health and how many hits have been sustained.
        /// </summary>
        /// <param name="myFinchOperator"></param>
        static void DisplayHealthStatus(FinchOperator myFinchOperator)
        {
            Console.WriteLine(myFinchOperator.HitPoints);
            Console.WriteLine();
            Console.WriteLine(myFinchOperator.HitsSuffered);
        }

        /// <summary>
        /// Checks health against hitpoints and returns a bool to desginate the Finch is defeated.
        /// </summary>
        /// <param name="myFinchOperator"></param>
        /// <returns></returns>
        static bool CheckHealth(FinchOperator myFinchOperator)
        {
            bool isDefeated = false;

            if (myFinchOperator.HitsSuffered >= myFinchOperator.HitPoints)
            { isDefeated = true; }

            return isDefeated;

        }

        /// <summary>
        /// Keeps track of the current player score and updates the player's score.
        /// </summary>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        static int PlayerScore(string userName, FinchOperator myFinchOperator)
        {
            int currentScore = myFinchOperator.HitsSuffered * myFinchOperator.ScoreModifier;
            return currentScore;
        }


        #endregion

        #region HELPER METHODS:
        static void DisplayHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t*************************[{0}]****************************", headerText);
        }

        static void DisplaySpacer()
        {
            Console.WriteLine();
            Console.WriteLine("\t**********************************************************");
            Console.WriteLine();

        }

        static void DisplayContinuePrompt()

        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }


        #endregion


    }
}
