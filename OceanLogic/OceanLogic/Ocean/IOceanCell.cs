namespace OceanLogic
{
    public interface IOceanCell
    {
        #region =====----- PUBLIC METHODS -----=====

        public Cell GetCell(int x, int y);
        public void TrySetCell(int x, int y, Cell cell);
        public void PreyEated();
        public void PreyMultiplied();
        public void PredatorDied();
        public void PredatorMultiplied();

        #endregion

    }
}
