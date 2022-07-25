namespace OceanLogic
{
    public class Controller
    {
        #region =====----- PRIVATE DATA -----=====

        private Ocean _ocean;
        private readonly IOceanViewer _view;
        private OceanStarter _oceanStarter;

        #endregion

        #region =====----- CTOR -----=====

        public Controller(IOceanViewer view)
        {
            _view = view;
            _ocean = new Ocean(view._viewGameSettings);
            _ocean.OceanDataChanged += RefreshView;

            _oceanStarter = new OceanStarter(_ocean, _view, view._viewGameSettings.IntervalOfIterations, view._viewGameSettings.CountCycles);

            _view.GetPause += View_GetPause;
            _view.GetStep += View_GetStep;
            _view.GetEmergencyExit += View_GetEmergencyExit;
        }

        #endregion

        #region =====----- PUBLIC METHODS -----=====

        public bool IsOceanAlive()
        {
            return _oceanStarter.IsAlive();
        }

        #endregion

        #region =====----- PRIVATE METHODS -----=====

        private void View_GetEmergencyExit(object sender, EventArgs e)
        {
            _oceanStarter.ForceEnd();
        }

        private void View_GetPause(object sender, EventArgs e)
        {
            _oceanStarter.Switch();
        }

        private void View_GetStep(object sender, EventArgs e)
        {
            _oceanStarter.Step();
        }

        private void RefreshView(object sender, OceanDataChangedEventArgs e)
        {
            _view.Display(e._field, e._data);
        }

        #endregion


    }
}
