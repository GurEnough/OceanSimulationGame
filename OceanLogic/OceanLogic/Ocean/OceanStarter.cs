namespace OceanLogic
{
    public class OceanStarter
    {
        #region =====----- PRIVATE DATA -----=====

        private Ocean _ocean;
        private Mutex _mutex = new Mutex();

        private IOceanViewer _oceanView;
        private int _cycles;
        private bool _oceanPaused = true;
        private bool _oceanAlive;

        #endregion

        #region =====----- CTOR -----=====

        public OceanStarter(Ocean ocean, IOceanViewer view, int tick, int cycles)
        {
            _ocean = ocean;
            _oceanView = view;
            _cycles = cycles;
            _oceanAlive = true;

            int _cycleInterval = 1000 / tick;

            var tickThread = new Thread(() =>
            {
                while (_cycles > 0)
                {
                    _mutex.WaitOne();
                    if (!_oceanPaused && _oceanAlive)
                    {
                        TryStep();
                        Thread.Sleep(_cycleInterval);
                    }
                    _mutex.ReleaseMutex();
                }
            });

            tickThread.IsBackground = true;
            tickThread.Start();
        }

        #endregion

        #region =====----- PRIVATE METHODS -----=====

        private void TryStep()
        {
            try
            {
                _ocean.Step();
                _cycles--;

                if (_cycles == 0)
                {
                    _oceanView.DisplayMessage("The end of the Simulation!");
                    _oceanAlive = false;
                }

            }

            catch (IndexOutOfRangeException)
            {
                _oceanAlive = false;
                _oceanView.DisplayMessage("Error: Tried to place cell out of field!");
            }

        }

        #endregion

        #region =====----- PUBLIC METHODS -----=====

        public void Step()
        {

            if (_oceanPaused && _oceanAlive)
            {
                TryStep();
            }

        }

        public void Switch()
        {
            _oceanPaused = !_oceanPaused;
        }

        public void ForceEnd()
        {
            _mutex.WaitOne();
            _oceanAlive = false;
            _oceanView.DisplayMessage("The game was over suddenly!");
            _mutex.ReleaseMutex();
        }

        public bool IsAlive()
        {
            return _oceanAlive;
        }

        #endregion

    }
}
