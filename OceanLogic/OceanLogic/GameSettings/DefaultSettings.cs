namespace OceanLogic
{
    public class DefaultSettings
    {

        #region =====----- PUBLIC DATA -----=====

        public const char DEFAULT_OBSTACLE_IMAGE = '#';
        public const char DEFAULT_OCEAN_IMAGE = '-';
        public const char DEFAULT_PREY_IMAGE = 'f';
        public const char DEFAULT_PREDATOR_IMAGE = 'S';

        public double PredatorCountProportion = 0.14;
        public double ObstacleCountProportion = 0.20;
        public double PreyCountProportion = 0.28;

        public int PreyTimeToMultiplied = 25;
        public int PredatorTimeToMultiplied = 25;
        public int PredatorTimeToEat = 15;

        public int IntervalOfIterations = 20;

        public int CountCycles = 300;
        public int OceanWidth = 60;
        public int OceanHeight = 40;

        #endregion

    }
}
