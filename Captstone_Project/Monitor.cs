using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinchAPI;

namespace Captstone_Project
{
    /// <summary>
    /// Class of functions which collect and store data for main application.
    /// </summary>
    class Monitor
    {
        #region FIELDS:
        private double _currentTemperature;
        private int _currentLight;
        private bool _isFrozen;
        private bool _isHit;
        #endregion

        #region PROPERTIES
             
        public double CurrentTemperature
        {
            get { return _currentTemperature; }
            set { _currentTemperature = value; }
        }

        public int CurrentLight
        {
            get { return _currentLight; }
            set { _currentLight = value; }
        }

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
        #endregion

        #region CONSTRUCTORS:
        static Monitor()
        { }
        #endregion

        #region METHODS:

        /// <summary>
        /// Hit Detection: Checks to see if the Finch has been "hit" by an attack. This will only inflict damage if the Finch's light is off.
        /// </summary>
        /// <param name="myFinch"></param>
        /// <returns></returns>
      public static bool HitDetection(Finch myFinch)
        {
            // Local Variables: 
            double ambientLight = GetLightAverage(myFinch);
            double threshold = 1;
            double maxLightThreshold = ambientLight + threshold;
            int currentLight = GetLightAverage(myFinch);

            bool isHit = false;

            currentLight = GetLightAverage(myFinch);


                if (currentLight > maxLightThreshold)
                {
                    isHit = true;
                }
               
               // myFinch.wait(1000);
            
            return isHit;
        }

        /// <summary>
        /// Gets average light values for above:
        /// </summary>
        /// <param name="myFinch"></param>
        /// <returns></returns>
      public static int GetLightAverage(Finch myFinch)
        {
            // Variables: 
            int leftSensor = myFinch.getLeftLightSensor();
            int rightSensor = myFinch.getRightLightSensor();
            int lightAverage = (rightSensor + leftSensor) / 2;

            return lightAverage;
        }

        /// <summary>
        /// Freeze Detection: Checks to see if the Finch has been frozen by "carbonite" and returns a value.
        /// </summary>
        /// <param name="myFinch"></param>
        /// <returns>isHit</returns>
      public static bool FreezeDetection(Finch myFinch)
        {
            // Variable: 
            double ambientTemperature = myFinch.getTemperature();
            const double threshold = 1;

            double minTemperatureThreshold = ambientTemperature - threshold;
            double currentTemperature = myFinch.getTemperature();

            bool isHit = false;
                        
                currentTemperature = myFinch.getTemperature();
            
                if (currentTemperature < minTemperatureThreshold)
                {
                    isHit = true;
                }
             
            //   myFinch.wait(1000);
            

            return isHit;
        }

        
        #endregion

    }
}
