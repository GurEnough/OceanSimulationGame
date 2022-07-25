namespace OceanLogic
{
    public class Prey : Cell
    {
        public override char Image { get; } = DefaultSettings.DEFAULT_PREY_IMAGE;

        #region =====----- PROTECTED DATA -----=====

        protected int _defaultTimeToReproduce;
        protected int _timeToReproduce;

        protected readonly DefaultSettings _settings;

        #endregion

        #region =====----- CTOR -----=====

        public Prey(DefaultSettings settings)
        {
            _settings = settings;
            _defaultTimeToReproduce = settings.PreyTimeToMultiplied;
            ResetReproduce();
        }

        public Prey()
        {
            ResetReproduce();
        }

        #endregion

        protected void ResetReproduce()
        {
            _timeToReproduce = General.OceanRandom.Next(_defaultTimeToReproduce) + _defaultTimeToReproduce / 2;
        }

        public override void Process(int x, int y, IOceanCell ocean)
        {
            DirectionAppliances.DirectionRandomIterate((nx, ny) =>
            {
                nx += x;
                ny += y;

                Cell cell = ocean.GetCell(nx, ny);

                if (cell.Image == DefaultSettings.DEFAULT_OCEAN_IMAGE)
                {
                    if (_timeToReproduce <= 0)
                    {
                        ResetReproduce();
                        ocean.TrySetCell(nx, ny, new Prey(_settings));
                        ocean.PreyMultiplied();
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
        }
    }
}
