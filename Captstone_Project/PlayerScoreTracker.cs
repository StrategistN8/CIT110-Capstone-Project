using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captstone_Project
{
    class PlayerScoreTracker
    {
        #region FIELDS
        private int playerScore = 0;
        private string playerName;
        #endregion

        #region PROPERTIES
        public int PlayerScore
        {
            get { return playerScore; }
            set { playerScore = value; }
        }

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
        #endregion

        #region CONSTRUCTORS

        static PlayerScoreTracker()
        { }

        #endregion

        #region METHODS
        // Might move some of the score processing methods over here in later refactoring.
        #endregion
    }
}
