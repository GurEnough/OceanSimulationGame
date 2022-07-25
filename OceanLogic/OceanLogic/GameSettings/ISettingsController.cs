namespace OceanLogic
{
    public interface ISettingsController
    {

        #region =====----- PUBLIC METHODS -----=====

        public void AddEntry(string name, double defaultValue, double min, double max);

        public void Make();

        #endregion

        #region =====----- EVENTS -----=====

        public event MenuEndEventHandler MenuExit;

        #endregion

    }


    public class MenuEndEventArgs : EventArgs
    {
        public List<(string entry, double val)> entered;

        public MenuEndEventArgs(List<(string entry, double val)> mEntered)

        {

            entered = mEntered;

        }

    }

    public delegate void MenuEndEventHandler(object sender, MenuEndEventArgs e);

}
