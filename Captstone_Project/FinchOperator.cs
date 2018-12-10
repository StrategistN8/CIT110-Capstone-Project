using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinchAPI;

namespace Captstone_Project
{

    /// <summary>
    /// Class of Finch operational functions: Controls various Finch settings and operations.
    /// </summary>
    class FinchOperator
    {
        #region FIELDS:
        private bool _isFrozen;
        private bool _isHit;
        private bool _isVulnerable;
        private bool _isDefeated;
        private int _vulnerablityDuration;
        private int _motorSpeed;
        private int hitPoints;
        private int _hitsSuffered;
        private int _scoreModifier;
        private Difficulty _difficultySetting;
        private int _timeAvailable;
    #endregion

        #region PROPERTIES:
        public bool IsFrozen
        {
            get { return _isFrozen; }
            set { _isFrozen = value; }
        }

        public bool IsHit
        {
            get { return _isHit; }
            set { _isHit = value; }
        }

        public bool IsVulnerable
        {
            get { return _isVulnerable; }
            set { _isVulnerable = value; }
        }

        public bool isDefeated
        {
            get { return _isDefeated; }
            set { _isDefeated = value; }
        }

        public int vulnerabilityDuration
        {
            get { return _vulnerablityDuration; }
            set { _vulnerablityDuration = value; }
        }

        public int MotorSpeed
        {
            get { return _motorSpeed; }
            set { _motorSpeed = value; }
        }

        public int HitPoints
        {
            get { return hitPoints; }
            set { hitPoints = value; }
        }

        public int HitsSuffered
        {
            get { return _hitsSuffered; }
            set { _hitsSuffered = value; }
        }

        public int ScoreModifier
        {
            get { return _scoreModifier; }
            set { _scoreModifier = value; }
        }

        public Difficulty DifficultySetting
        {
            get { return _difficultySetting; }
            set { _difficultySetting = value; }
        }

        public int TimeAvailable
        {
            get { return _timeAvailable; }
            set { _timeAvailable = value; }
        }
        #endregion

        #region CONSTRUCTORS:
        static FinchOperator()
        { }
        #endregion

        #region METHODS:

        /// <summary>
        /// Connects to the Finch:
        /// </summary>
        /// <param name="myFinch"></param>
        public static void EstablishFinchConnection(Finch myFinch)
        {
            Console.WriteLine();
            Console.WriteLine("\tPlease plug your finch into the computer. Then, press any key to continue.");
            Console.ReadKey();

            //Attempts to Connect to the Finch Robot:
            myFinch.connect();
         
            if (!myFinch.connect()) // If connection fails:
            {
                Console.WriteLine("\tUnable to connect to the finch. Please check your cable connection and try again!");
                DisplayContinuePrompt();
            }
            
            if (myFinch.connect()) // If connection is successful:
            {
                // "Powering up" effect:
                for (int i = 0; i < 255; i++)
                {
                    myFinch.setLED(i, 0, 0);
                }
                PlayImpMarchShort(myFinch);
                Console.WriteLine();
                Console.WriteLine("\tYou are now connected!");
                DisplayContinuePrompt();
                Console.Clear();
            }
        }

        /* MotorSettings: Unfinished Component. Needs more time for testing. 
        /// <summary>
        /// Sets motor behavior for the Finch.
        /// </summary>
        /// <param name="myFinch"></param>
        /// <param name="difficulty"></param>
        /// <param name="myFinchOperator"></param>
        /// <param name="isDefeated"></param>
        public static void MotorSettings(Finch myFinch, FinchOperator myFinchOperator)
        {
            switch (myFinchOperator.DifficultySetting)
            {
                case Difficulty.MERCIFUL:

                    myFinch.setMotors(myFinchOperator.MotorSpeed, myFinchOperator.MotorSpeed);
                    myFinch.wait(4000);
                    myFinch.setMotors(myFinchOperator.MotorSpeed, myFinchOperator.MotorSpeed * -1);
                    myFinch.wait(1000);
                    break;
                case Difficulty.NORMAL:

                    myFinch.setMotors(myFinchOperator.MotorSpeed, myFinchOperator.MotorSpeed);
                    myFinch.wait(1000);
                    myFinch.setMotors(myFinchOperator.MotorSpeed, myFinchOperator.MotorSpeed * -1);
                    myFinch.wait(500);

                    break;
                case Difficulty.RUTHLESS:
                    Random ruthless = new Random();
                    myFinch.setMotors(myFinchOperator.MotorSpeed, myFinchOperator.MotorSpeed);
                    myFinch.wait(ruthless.Next() * 100);
                    myFinch.setMotors(myFinchOperator.MotorSpeed, myFinchOperator.MotorSpeed * -1);
                    myFinch.wait(ruthless.Next() * 100);
                    break;
                default:
                    myFinch.setMotors(0, 0);
                    break;
            }

            myFinch.setMotors(myFinchOperator.MotorSpeed, myFinchOperator.MotorSpeed);

        }
        */

        /// <summary>
        /// Turns the Finch's light on to indicate it is immune to damage (the light saber is active) and turns it off while vulnerable (saber is off).
        /// </summary>
        /// <param name="myFinch"></param>
        /// <param name="isVulnerable"></param>
        public static void LEDSettings(Finch myFinch, FinchOperator myFinchOperator)
        {
            if (myFinchOperator.IsVulnerable == true)
            {
                myFinch.setLED(0, 0, 0);
            }
            else
            {
                myFinch.setLED(255, 0, 0);
            }

        }

        /// <summary>
        /// Plays the Imperial March on the finch. WARNING: VERY LONG CODE BLOCK - CLOSE WHEN NOT IN USE
        /// </summary>
        /// <param name="myFinch"></param>
        public static void PlayImpMarch(Finch myFinch)
        {
            // Just in case the user attempts to run the game without a Finch - it doesn't crash, but it doesn't make sense either.
            if (!myFinch.connect())
            {
                DisplayHeader("DARTH FINCH");
                Console.WriteLine("\tNo finch detected. Please connect a Finch before proceeding!");
                DisplayContinuePrompt();
                FinchOperator.EstablishFinchConnection(myFinch);
            }

            DisplayHeader("DARTH FINCH");
            Console.WriteLine("\tDarth Finch is amused by your request, but is willing to humor you.");
            Console.WriteLine("\t(Please allow the Finch to finish, you will see a continue prompt when it is done)");

            // Source for notes: http://llrprt.blogspot.com/2013/11/programming-imperial-march.htmlhttp://llrprt.blogspot.com/2013/11/programming-imperial-march.html

            myFinch.wait(1000);
            myFinch.noteOn(440);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(349);
            myFinch.wait(350);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(150);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(349);
            myFinch.wait(350);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(150);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(1000);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(659);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(659);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(659);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(698);
            myFinch.wait(350);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(150);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(415);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(349);
            myFinch.wait(350);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(150);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(1000);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(800);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(350);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(150);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(880);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(830);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(784);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(740);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(740);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(250);

            myFinch.noteOn(455);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(622);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(587);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(554);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(466);  // I wasn't kidding when I said this was long.
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(250);

            myFinch.noteOn(349);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(415);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(349);
            myFinch.wait(375);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(375);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(659);
            myFinch.wait(1000);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(880);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(350);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(150);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(880);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(830);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(784);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(740);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(698);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(740);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(250);

            myFinch.noteOn(455);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(622);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(587);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(554);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(466);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);



            myFinch.noteOn(523);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(250);

            myFinch.noteOn(349);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(415);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(349);
            myFinch.wait(375);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(261);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(475);
            myFinch.wait(1000);
            myFinch.noteOff();
            myFinch.wait(100);

        }

        /// <summary>
        /// Plays the first few notes of Imperial March on the Finch. WARNING: LONG CODE BLOCK!
        /// </summary>
        /// <param name="myFinch"></param>
        public static void PlayImpMarchShort(Finch myFinch)
        {
            // Source for notes: http://llrprt.blogspot.com/2013/11/programming-imperial-march.htmlhttp://llrprt.blogspot.com/2013/11/programming-imperial-march.html

            myFinch.wait(1000);
            myFinch.noteOn(440);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(349);
            myFinch.wait(350);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(150);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(349);
            myFinch.wait(350);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(523);
            myFinch.wait(150);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(440);
            myFinch.wait(1000);
            myFinch.noteOff();
            myFinch.wait(10);
        }

        /// <summary>
        /// Plays the last few notes of Imperial March. WARNING: LONG CODE BLOCK!
        /// </summary>
        /// <param name="myFinch"></param>
        public static void PlayGameOverMarch(Finch myFinch)
        {
            // Just the last five notes:
            myFinch.noteOn(349);
            myFinch.wait(250);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(415);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(349);
            myFinch.wait(375);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(261);
            myFinch.wait(125);
            myFinch.noteOff();
            myFinch.wait(10);

            myFinch.noteOn(475);
            myFinch.wait(1000);
            myFinch.noteOff();
            myFinch.wait(100);
        }
        #endregion

        #region HELPER METHODS:
        static void DisplayHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("***************[{0}]*****************", headerText);
        }

        static void DisplaySpacer()
        {
            Console.WriteLine();
            Console.WriteLine("**********************************************************");
            Console.WriteLine();

        }

        static void DisplayContinuePrompt()

        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
        #endregion

    }
}
