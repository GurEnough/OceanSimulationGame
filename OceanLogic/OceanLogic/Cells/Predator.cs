namespace OceanLogic
{
    public class Predator : Prey
    {
        public override char Image { get; } = DefaultSettings.DEFAULT_PREDATOR_IMAGE;

        #region =====----- PRIVATE DATA -----=====

        private int _timeToFeed;

        #endregion

        #region =====----- CTOR -----=====

        public Predator(DefaultSettings settings) : base(settings: settings)
        {
            _defaultTimeToReproduce = _settings.PredatorTimeToMultiplied;
            _timeToFeed = _settings.PredatorTimeToEat;
            ResetReproduce();
        }

        #endregion


        public override void Process(int x, int y, IOceanCell ocean)
        {
            if (_timeToFeed <= 0)
            {
                ocean.TrySetCell(x, y, General.CellAlone);
                ocean.PredatorDied();
                return;
            }

            DirectionAppliances.DirectionRandomIterate((nx, ny) =>
            {
                nx += x;
                ny += y;

                Cell cell = ocean.GetCell(nx, ny);

                if (cell.Image == DefaultSettings.DEFAULT_PREY_IMAGE)
                {
                    _timeToFeed = _settings.PredatorTimeToEat;
                    cell = General.CellAlone;
                    ocean.TrySetCell(nx, ny, cell);
                    ocean.PreyEated();
                    return false;
                }
                if (cell.Image == DefaultSettings.DEFAULT_OCEAN_IMAGE)
                {
                    if (_timeToReproduce <= 0)
                    {
                        ResetReproduce();
                        ocean.TrySetCell(nx, ny, new Predator(_settings));
                        ocean.PredatorMultiplied();
                    }
                    else
                    {
                        ocean.TrySetCell(x, y, General.CellAlone);
                        ocean.TrySetCell(nx, ny, this);
                    }
                    return false;
                }
                return true;
            });

            _timeToReproduce--;
            _timeToFeed--;
        }
    }
}
