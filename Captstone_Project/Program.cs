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

            DisplayHeader("DARTH FINCH");
            Console.WriteLine();
            
            Console.WriteLine("Initializing...");

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
                hitTracker.IsFrozen = Monitor.HitDetection(myFinch);
                hitTracker.IsHit = Monitor.FreezeDetection(myFinch);

                DisplayHeader("DARTH FINCH:");

                Console.WriteLine("Darth Finch is active!");
                Console.WriteLine($"Time Remaining: [{myFinchOperator.TimeAvailable - timeInLoop}]");
                DisplayHealthStatus(myFinchOperator);

                DisplayVulnerbilityStatus(myFinch, hitTracker, myFinchOperator);

                myFinchOperator.isDefeated = CheckHealth(myFinchOperator);

                //FinchOperator.LEDSettings(myFinch, myFinchOperator);
                // FinchOperator.MotorSettings(myFinch, myFinchOperator);
                myFinch.wait(1000);
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
            Console.WriteLine("***************[Goodbye!]*****************");
            Console.WriteLine();
            Console.WriteLine("Thank you, {0}, for using this application!", closingText);
            Console.WriteLine();
            Console.WriteLine("Press any key to end the application {0}.", closingText);
            Console.ReadKey();

            myFinch.disConnect();
        }

        /// <summary>
        /// Saves the players score to a file:
        /// </summary>
        /// <param name="playerScore"></param>
        static void DisplaySavePlayerScoreToFile(PlayerScoreTracker playerScore)
        {
            string dataPath = @"Data\Scores.txt";
            List<string> playerScoreList = new List<string>();
            string playerScoreToSave = playerScore.PlayerName + "," + playerScore.PlayerScore.ToString();

            playerScoreList.Add(playerScoreToSave);

            File.AppendAllText(dataPath, playerScoreToSave);

            Console.WriteLine("Score Saved.");
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
                string[] scoreArray = new string[2];

                DisplaySpacer();
                foreach (string score in myArray)
                {
                    scoreArray = score.Split(',');
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
                Console.WriteLine("[{0}]",e);
            }
        }
     
        /// <summary>
        /// Game Over Screen:
        /// </summary>
        /// <param name="currentScore"></param>
        static void DisplayGameOver(PlayerScoreTracker currentScore, Finch myFinch)
        {
            DisplayHeader("GAME OVER");

            DisplayPlayerScore(currentScore);

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
            PlayerScoreTracker playerScore = new PlayerScoreTracker();
            playerScore.PlayerName = userName;
           

            bool runApp = true;

            switch (menuChoice.ToUpper())
            {
                case "1":
                    DisplayGameInProgress(myFinch, myFinchOperator);
                    CurrentPlayerScore(userName, playerScore, myFinchOperator);
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
                    DisplayHeader("DARTH FINCH");
                    Console.WriteLine("\tDarth Finch is amused by your request, but is willing to humor you.");
                    Console.WriteLine("\t(Please allow the Finch to finish, you will see a continue prompt when it is done)");
                    FinchOperator.PlayImpMarch(myFinch);
                    DisplayContinuePrompt();
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
            Console.WriteLine($"You selected {myFinchOperator.DifficultySetting}");
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
            // Statment to indicate a hit while shields are up.
            if (hitTracker.IsHit == true && hitTracker.IsFrozen == false)
            {
                Console.WriteLine();
                Console.WriteLine("The blast is deflected!");
                FinchOperator.LEDSettings(myFinch, myFinchOperator);
                myFinch.wait(1000);
            }

            // Statment to indicate a hit while shields are down.
            if (hitTracker.IsHit == false && hitTracker.IsFrozen == true)
            {
                Console.WriteLine();
                Console.WriteLine("That got him! Strike now while he is stunned!");

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
            myFinch.wait(500);
            FinchOperator.LEDSettings(myFinch, myFinchOperator);


        }
        
        /// <summary>
        /// Displays current health and how many hits have been sustained.
        /// </summary>
        /// <param name="myFinchOperator"></param>
        static void DisplayHealthStatus(FinchOperator myFinchOperator)
        {
          
            Console.Write($"Current Hitpoints: {myFinchOperator.HitPoints - myFinchOperator.HitsSuffered}");
            
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
        #endregion

        #region SCORING METHODS:
        
        /// <summary>
        /// Display the player's score:
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="myFinchOperator"></param>
        static void DisplayPlayerScore(PlayerScoreTracker currentScore)
        {
            Console.WriteLine("\t{0} scored {1}", currentScore.PlayerName, currentScore.PlayerScore);      
        }


        /// <summary>
        /// Keeps track of the current player score and updates the player's score.
        /// </summary>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        static PlayerScoreTracker CurrentPlayerScore(string userName, PlayerScoreTracker currentScore, FinchOperator myFinchOperator)
        {
            currentScore.PlayerScore = myFinchOperator.HitsSuffered * myFinchOperator.ScoreModifier;
            currentScore.PlayerName = userName;

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
