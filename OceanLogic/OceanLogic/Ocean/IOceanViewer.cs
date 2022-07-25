namespace OceanLogic
{
    public interface IOceanViewer
    {

        #region =====----- PROPERTIES -----=====

        public DefaultSettings _viewGameSettings { get; }

        #endregion

        #region =====----- PUBLIC METHODS -----=====

        public void Display(in Cell[,] field, in OceanData data);

        public void DisplayMessage(string message);

        #endregion

        #region =====----- EVENTS -----=====

        public event EventHandler GetEmergencyExit;

        public event EventHandler GetPause;

        public event EventHandler GetStep;

        #endregion

    }
}
