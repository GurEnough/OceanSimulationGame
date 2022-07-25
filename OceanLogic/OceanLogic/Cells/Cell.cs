namespace OceanLogic
{
    public class Cell
    {
        public virtual char Image { get; } = DefaultSettings.DEFAULT_OCEAN_IMAGE;
        public virtual void Process(int x, int y, IOceanCell ocean)
        {

        }
    }
}

