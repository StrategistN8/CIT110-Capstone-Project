﻿using System;
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
    // Date Last Modified: 12/9/2018
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
            myFinchOperator.DifficultySetting = Difficulty.NORMAL;
            bool runApp = true;
            string userName;

            // Setting up the console:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            // Connecting to Finch to make debugging easier:
            // myFinch.connect();

            // Welcome Screen:
            userName = DisplayWelcomeScreen();

            //
            while (runApp)
            {
                runApp = DisplayMainMenu(userName, myFinch, myFinchOperator);
            }

            DisplayClosingScreen(userName, myFinch);
        }

        #region APPLICATION SCREENS

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
            Console.WriteLine("\tPlease enter player name:");
            Console.Write("\t-> ");
            userName = Console.ReadLine();

            // Echoing the name to confirm:

            Console.Clear();
            DisplayHeader("DARTH FINCH");
            Console.WriteLine();
            Console.WriteLine("\t{0}, welcome!", userName);


            DisplayContinuePrompt();

            return userName;
        }

        /// <summary>
        /// Screen that runs the gameplay:
        /// </summary>
        /// <param name="myFinch"></param>
        /// <param name="myFinchOperator"></param>
        /// <param name="difficulty"></param>
        static void DisplayGameInProgress(Finch myFinch, FinchOperator myFinchOperator)
        {
            // Instantiates a new hit tracker:
            Monitor hitTracker = new Monitor();
            int timeInLoop = 0;

            //Variables:
            myFinchOperator.isDefeated = false;
            
            // Resets Finch Operator to default hitpoints.
            myFinchOperator.HitsSuffered = 0;
            

            DisplayHeader("DARTH FINCH");
            Console.WriteLine();

            Console.WriteLine("\t\tInitializing...");

            // Just in case the user attempts to run the game without a Finch - it doesn't crash, but it doesn't make sense either.
            if (!myFinch.connect())
            {
                DisplayHeader("DARTH FINCH");
                Console.WriteLine("\tNo finch detected. Please connect a Finch before proceeding!");
                DisplayContinuePrompt();
                FinchOperator.EstablishFinchConnection(myFinch);
            }

            // Little indicator that the game has begun:
            FinchOperator.PlayImpMarchShort(myFinch);

            // Main Gameplay Loop:
            while (!myFinchOperator.isDefeated && timeInLoop <= myFinchOperator.TimeAvailable)
            {
                DisplayHeader("DARTH FINCH:");

                Console.WriteLine("\tDarth Finch is active!");
                Console.WriteLine("\tTime Remaining:" + $"[{myFinchOperator.TimeAvailable - timeInLoop}]");
                DisplayHealthStatus(myFinchOperator);

                //hitTracker.IsFrozen = Monitor.FreezeDetection(myFinch);
                //hitTracker.IsHit = Monitor.HitDetection(myFinch);

                DisplayVulnerbilityStatus(myFinch, hitTracker, myFinchOperator);
                myFinch.wait(500);
              
                myFinchOperator.isDefeated = CheckHealth(myFinchOperator);

                FinchOperator.LEDSettings(myFinch, myFinchOperator);
                // FinchOperator.MotorSettings(myFinch, myFinchOperator); Disabling this for the time being.
                myFinch.wait(500);
                timeInLoop++;
            }



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
            Console.WriteLine("\t***************[Goodbye!]*****************");
            Console.WriteLine();
            Console.WriteLine("\tThank you, {0}, for using this application!", closingText);
            Console.WriteLine();
            Console.WriteLine("\tPress any key to end the application {0}.", closingText);
            Console.ReadKey();

            myFinch.disConnect();
        }

        /// <summary>
        /// Saves the players score to a file:
        /// </summary>
        /// <param name="currentScore"></param>
        static void DisplaySavePlayerScoreToFile(PlayerScoreTracker currentScore)
        {
           
            // Adds the player's score to a list and then adds it to the file.
            try
            {
                // Varables:
                string dataPath = @"Data\Scores.txt";
                List<string> playerScoreList = new List<string>();
                string playerScoreToSave = "\n" + currentScore.PlayerName + ", " + currentScore.PlayerScore.ToString();
                playerScoreList.Add(playerScoreToSave);
                File.AppendAllLines(dataPath, playerScoreList);

                // Message for user.
                DisplayHeader("Save Scores");
                Console.WriteLine();
                Console.WriteLine("\tScore have been successfully saved.");
            }
            catch (FileNotFoundException)
            {
                DisplayHeader("ERROR");
                Console.WriteLine("\tUnable find the file containing scores. The file may have been deleted or corrupted.");
                Console.WriteLine();
            }
            catch (NullReferenceException)
            {
                DisplayHeader("ERROR");
                Console.WriteLine("\tUnable to save scores. No scores were found.");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                DisplayHeader("ERROR");
                Console.WriteLine("\tAn error has occured. Please see the following for more information");
                Console.WriteLine("[{0}]", e);
            }

            DisplayContinuePrompt();

        }

        /// <summary>
        /// Reads the high scores list from the file.
        /// </summary>
        static void DisplayReadScoreFromFile()
        {
            // Local Variables:
            string dataPath = @"Data\Scores.txt";
            List<PlayerScoreTracker> scoreList = new List<PlayerScoreTracker>();
            PlayerScoreTracker tempPlayerScore = new PlayerScoreTracker();
            try
            {
                DisplayHeader("Load Scores:");
                Console.WriteLine();
                Console.WriteLine("\tPress any key to retrieve saved scores:");
                Console.ReadKey();

                //File.WriteAllLines(dataPath, scoreList);

                string[] myArray = File.ReadAllLines(dataPath);

                DisplaySpacer();
                foreach (string score in myArray)
                {
                    string[] scoreArray = score.Split(',');
                    tempPlayerScore.PlayerName = scoreArray[0];
                    tempPlayerScore.PlayerScore = int.Parse(scoreArray[1]);
                    Console.WriteLine($"{tempPlayerScore.PlayerName}: {tempPlayerScore.PlayerScore} points");
                }

                DisplayContinuePrompt();
            }
            catch (FileNotFoundException)
            {
                DisplayHeader("ERROR");
                Console.WriteLine("\tUnable to read scores from the file. The file may have been deleted or corrupted.");
                Console.WriteLine();
            }
            catch (NullReferenceException)
            {
                DisplayHeader("ERROR");
                Console.WriteLine("\tUnable to retrieve scores. No scores were found.");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                DisplayHeader("ERROR");
                Console.WriteLine("\tAn error has occured. Please see the following for more information");
                Console.WriteLine("[{0}]", e);
            }
        }

        /// <summary>
        /// Game Over Screen:
        /// </summary>
        /// <param name="currentScore"></param>
        static void DisplayGameOver(PlayerScoreTracker currentScore, Finch myFinch)
        {
            DisplayHeader("GAME OVER");

            // Displays the player's score.
            DisplayPlayerScore(currentScore);

            // Bit of a musical blurb for the user.
            FinchOperator.PlayGameOverMarch(myFinch);

            DisplayContinuePrompt();


        }

        #endregion

        #region MENU SCREENS: 

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
            Console.WriteLine("\t6.) [The Imperial March]");
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


        #endregion

        #region MENU HELPER METHODS: 

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
                    Console.WriteLine("'{0}' is not a valid input. Please try again!", menuChoice);
                    DisplayContinuePrompt();
                    Console.Clear();
                    break;
            }

            return returnToPrevious;
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
            PlayerScoreTracker playerScore = new PlayerScoreTracker();
            playerScore.PlayerName = userName;

            switch (menuChoice.ToUpper())
            {
                case "1":
                    DisplayGameInProgress(myFinch, myFinchOperator);
                    playerScore = CurrentPlayerScore(userName, playerScore, myFinchOperator);
                    DisplayGameOver(playerScore, myFinch);
                    break;
                case "2":
                    DisplayInformationScreen(userName);
                    break;
                case "3":
                    DisplaySetupMenu(userName, myFinch, myFinchOperator);
                    break;
                case "4":
                    DisplaySavePlayerScoreToFile(playerScore);
                    break;
                case "5":
                    DisplayReadScoreFromFile();
                    break;
                case "6":
                    FinchOperator.PlayImpMarch(myFinch);
                    DisplayHeader("END OF SONG");
                    DisplayContinuePrompt();
                    break;
                case "E":
                    runApp = false;
                    break;
                default:
                    Console.WriteLine("\t'{0}' is not a valid input. Please try again!", menuChoice);
                    DisplayContinuePrompt();
                    Console.Clear();
                    break;
            }

            return runApp;
        }
        #endregion

        #region GAMEPLAY METHODS:

        /// <summary>
        /// Gets the desired difficulty from the user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        static void GetDifficultySetting(FinchOperator myFinchOperator)
        {
            // Local Variables:
            Difficulty difficulty;

            DisplayHeader("DARTH FINCH: SELECT DIFFICULTY");
            Console.WriteLine();
            foreach (var setting in Enum.GetValues(typeof(Difficulty)))
            {
                Console.WriteLine("\t" + setting);
            }

            Console.WriteLine();
            Console.Write("\tSelect Difficulty -> ");
            while (!Enum.TryParse(Console.ReadLine().ToUpper(), out difficulty))
            {
                Console.WriteLine("\tThat is not a valid difficulty level. Please try again!");
                DisplayContinuePrompt();
            }


            myFinchOperator.DifficultySetting = difficulty;
            Console.WriteLine();
            Console.WriteLine("\tYou selected " + $"{myFinchOperator.DifficultySetting}");
            DisplayContinuePrompt();

        }

        /// <summary>
        /// Takes the difficulty selected by the player and assigns values for use in other Finch operations based on that difficulty.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        static FinchOperator DifficultySettings(FinchOperator myFinchOperator)
        {
            // Sets certain values depending on the selected difficulty. The default is NORMAL.
            switch (myFinchOperator.DifficultySetting)
            {
                case Difficulty.MERCIFUL:
                    myFinchOperator.vulnerabilityDuration = 6000;
                    myFinchOperator.MotorSpeed = 100;
                    myFinchOperator.HitPoints = 1;
                    myFinchOperator.TimeAvailable = 60;
                    myFinchOperator.ScoreModifier = 5;
                    break;
                case Difficulty.NORMAL:
                    myFinchOperator.vulnerabilityDuration = 5000;
                    myFinchOperator.MotorSpeed = 175;
                    myFinchOperator.HitPoints = 3;
                    myFinchOperator.TimeAvailable = 120;
                    myFinchOperator.ScoreModifier = 10;
                    break;
                case Difficulty.RUTHLESS:
                    myFinchOperator.vulnerabilityDuration = 4000;
                    myFinchOperator.MotorSpeed = 255;
                    myFinchOperator.HitPoints = 5;
                    myFinchOperator.TimeAvailable = 120;
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
            // Checks for hits upon entering the method:
            hitTracker.IsHit = Monitor.HitDetection(myFinch);
            hitTracker.IsFrozen = Monitor.FreezeDetection(myFinch);
            
            // Statment to indicate a hit while shields are up.
            if (hitTracker.IsHit == true && hitTracker.IsFrozen == false)
            {
                Console.WriteLine();
                Console.WriteLine("\tThe blast is deflected! You need to freeze him first.");
                FinchOperator.LEDSettings(myFinch, myFinchOperator);
                myFinch.wait(1000);
            }

            // Statment to indicate a hit while shields are down.
            if (hitTracker.IsFrozen == true)
            {
                Console.WriteLine();
                Console.WriteLine("\tThat got him! Strike now while he is stunned!");

                // Sets the finch to vulnerable so it can take hits:
                myFinchOperator.IsVulnerable = true;

                // Turns off the LED to indicate shields are down.
                FinchOperator.LEDSettings(myFinch, myFinchOperator);

                // Enters this method to determine if a hit is suffered or not.
                ShieldsDown(myFinch, hitTracker, myFinchOperator);
            }

        }

        /// <summary>
        /// Method that controls hits while shields are down.
        /// </summary>
        /// <param name="myFinch"></param>
        /// <param name="hitTracker"></param>
        /// <param name="myFinchOperator"></param>
        static void ShieldsDown(Finch myFinch, Monitor hitTracker, FinchOperator myFinchOperator)
        {
            myFinch.wait(myFinchOperator.vulnerabilityDuration);
            hitTracker.IsHit = Monitor.HitDetection(myFinch);
           
            if (hitTracker.IsHit == true)
            {
                Console.WriteLine();
                Console.WriteLine("\tNice hit! He'll feel that for sure.");
                myFinchOperator.HitsSuffered += 1;
                myFinch.wait(500);
            }

            else
            {
                Console.WriteLine();
                Console.WriteLine("\tThe moment to strike has passed. Try again!");
                myFinch.wait(500);
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

            Console.Write("\tCurrent Hitpoints: " + $"{myFinchOperator.HitPoints - myFinchOperator.HitsSuffered}");

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
            {
                isDefeated = true;

                Console.WriteLine("\tDarth Finch has been defeated!");

            }

            return isDefeated;

        }
        #endregion

        #region SCORING METHODS:

        /// <summary>
        /// Display the player's score:
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="myFinchOperator"></param>
        static void DisplayPlayerScore(PlayerScoreTracker currentScore)
        {
            Console.WriteLine("\t{0} scored {1}", currentScore.PlayerName, currentScore.PlayerScore, " points");
        }


        /// <summary>
        /// Keeps track of the current player score and updates the player's score.
        /// </summary>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        static PlayerScoreTracker CurrentPlayerScore(string userName, PlayerScoreTracker playerScore, FinchOperator myFinchOperator)
        {
            
            playerScore.PlayerScore = myFinchOperator.HitsSuffered * myFinchOperator.ScoreModifier;
            playerScore.PlayerName = userName;

            return playerScore;
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
