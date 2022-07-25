namespace OceanLogic
{
    public class Ocean : IOceanCell
    {
        #region =====----- PRIVATE DATA -----=====

        private Cell[,] _cells;

        private DefaultSettings _settings;
        private OceanRandom _random;

        private OceanData _data;

        #endregion

        #region =====----- CTOR -----=====

        public Ocean(DefaultSettings settings)
        {
            _settings = settings;

            _random = new OceanRandom(_settings);

            _data.ResetData();

            Initialize();
        }

        #endregion

        #region =====----- PRIVATE METHODS -----=====

        private Cell GenerateCellFromImage(char img)
        {
            switch (img)
            {
                case DefaultSettings.DEFAULT_OBSTACLE_IMAGE:
                    _data.numObstacles++;
                    return General.ObstacleAlone;
                case DefaultSettings.DEFAULT_PREDATOR_IMAGE:
                    _data.numPredators++;
                    return new Predator(_settings);
                case DefaultSettings.DEFAULT_PREY_IMAGE:
                    _data.numPreys++;
                    return new Prey(_settings);
                default:
                    return General.CellAlone;
            }
        }

        private void Initialize()
        {
            _cells = new Cell[_settings.OceanHeight, _settings.OceanWidth];

            for (int i = 0; i < _settings.OceanHeight; i++)
            {
                for (int j = 0; j < _settings.OceanWidth; j++)
                {
                    _cells[i, j] = GenerateCellFromImage(_random.NextCellImage());
                }
            }
        }

        private void Process()
        {
            var processed = new HashSet<int>();

            for (int i = 0; i < _settings.OceanHeight; i++)
            {
                for (int j = 0; j < _settings.OceanWidth; j++)
                {
                    int hash = _cells[i, j].GetHashCode();

                    if (!processed.Contains(hash))
                    {
                        _cells[i, j].Process(j, i, this);
                        processed.Add(hash);
                    }
                }
            }
        }
        private bool IsOutOfBorder(int x, int y)
        {
            return x >= _settings.OceanWidth || y >= _settings.OceanHeight || x < 0 || y < 0;
        }

        #endregion

        #region =====----- PUBLIC METHODS -----=====

        public Cell GetCell(int x, int y)
        {
            if (IsOutOfBorder(x, y))
            {
                return General.ObstacleAlone;
            }

            return _cells[y, x];
        }

        public void TrySetCell(int x, int y, Cell cell)
        {
            if (IsOutOfBorder(x, y))
            {
                throw new IndexOutOfRangeException("Cell coordinates is out of game field");
            }
            _cells[y, x] = cell;
        }

        public void PreyEated()
        {
            _data.numPreys--;
        }

        public void PreyMultiplied()
        {
            _data.numPreys++;
        }

        public void PredatorDied()
        {
            _data.numPredators--;
        }

        public void PredatorMultiplied()
        {
            _data.numPredators++;
        }

        public void Step()
        {
            _data.NextCycle();

            if (OceanDataChanged != null)
            {
                OceanDataChanged(this, new OceanDataChangedEventArgs(_cells, _data));
            }

            if (_data.numPreys == 0 || _data.numPredators == 0)
            {
                Console.WriteLine("\nThe End Of The Simulation!");
                Console.ReadKey();
                Environment.Exit(0);

            }

            Process();

        }

        #endregion


        public event OceanDataChangedEventHandler OceanDataChanged;

    }

    public class OceanDataChangedEventArgs : EventArgs
    {
        public Cell[,] _field;
        public OceanData _data;

        public OceanDataChangedEventArgs(Cell[,] field, OceanData data)
        {
            _field = field;
            _data = data;
        }
    }

    public delegate void OceanDataChangedEventHandler(object sender, OceanDataChangedEventArgs e);
}
