namespace OceanLogic
{
    public struct OceanData
    {

        #region =====----- PROPERTIES -----=====

        public int numPreys { get; set; }
        public int numPredators { get; set; }
        public int numObstacles { get; set; }
        public int numCycles { get; private set; }

        #endregion

        #region =====----- PUBLIC METHODS -----=====

        public void NextCycle()
        {
            numCycles++;
        }
        public void ResetData()
        {
            numPreys = 0;
            numPredators = 0;
            numObstacles = 0;
            numCycles = 0;
        }

        #endregion

    }
}
