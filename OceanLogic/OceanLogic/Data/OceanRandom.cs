namespace OceanLogic
{
    public class OceanRandom : Random
    {
        private List<(char img, double ratio)> ratios;

        public OceanRandom(DefaultSettings settings)
        {
            ratios = new List<(char, double)>
            {
                new (DefaultSettings.DEFAULT_OBSTACLE_IMAGE, settings.ObstacleCountProportion),
                new (DefaultSettings.DEFAULT_PREDATOR_IMAGE, settings.PredatorCountProportion),
                new (DefaultSettings.DEFAULT_PREY_IMAGE, settings.PreyCountProportion),
                new (DefaultSettings.DEFAULT_OCEAN_IMAGE, 1)
            };
        }

        public char NextCellImage()
        {
            var rands = ratios.Select(r => (r.img, NextDouble() * r.ratio));

            return rands.OrderBy(rand => rand.Item2).Last().img;
        }
    }
}
