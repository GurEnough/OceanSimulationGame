using OceanLogic;

namespace OceanConsoleVersion
{
    public class ConsoleOceanViewer : IOceanViewer
    {
        #region =====----- PRIVATE DATA -----=====

        private int currentCursorY = 0;

        private Cell[,] prevFieldState = null;
        private bool isBordersPrinted = false;

        #endregion

        #region =====----- EVENTS -----=====

        public event EventHandler GetPause;
        public event EventHandler GetStep;
        public event EventHandler GetEmergencyExit;

        #endregion

        #region =====----- PROPERTIES -----=====

        public DefaultSettings _viewGameSettings { get; }

        #endregion

        #region =====----- CTOR -----=====

        public ConsoleOceanViewer(DefaultSettings settings)
        {
            Console.CursorVisible = false;
            _viewGameSettings = settings;

            prevFieldState = new Cell[_viewGameSettings.OceanHeight, _viewGameSettings.OceanWidth];
        }

        #endregion

        #region =====----- PRIVATE METHODS -----=====

        private void DisplayUpperHorizontalBorders(int length)
        {
            Console.Write("╔");

            for (int i = 0; i < length; i++)
            {
                Console.Write("═");
            }

            Console.Write("╗\n");
        }

        private void DisplayDownHorizontalBorders(int length)
        {
            Console.Write("╚");

            for (int i = 0; i < length; i++)
            {
                Console.Write("═");
            }

            Console.Write("╝\n");
        }

        private void PrintBorder()
        {
            DisplayUpperHorizontalBorders(_viewGameSettings.OceanWidth);

            for (int i = 0; i < _viewGameSettings.OceanHeight; i++)
            {
                Console.Write('║');
                for (int j = 0; j < _viewGameSettings.OceanWidth; j++)
                {
                    Console.Write(' ');
                }
                Console.Write("║\n");
            }

            DisplayDownHorizontalBorders(_viewGameSettings.OceanWidth);
        }
        private void SetCell(int x, int y, Cell cell)
        {
            Console.SetCursorPosition(x + 1, currentCursorY + y + 1);
            Console.Write(cell.Image);
        }
        private void DisplayField(in Cell[,] field)
        {
            currentCursorY = Console.GetCursorPosition().Top;

            if (!isBordersPrinted)
            {
                PrintBorder();
                isBordersPrinted = true;
            }

            for (int i = 0; i < _viewGameSettings.OceanHeight; i++)
            {
                for (int j = 0; j < _viewGameSettings.OceanWidth; j++)
                {
                    if (prevFieldState[i, j] == null || prevFieldState[i, j] != field[i, j])
                    {
                        SetCell(j, i, field[i, j]);
                    }
                    prevFieldState[i, j] = field[i, j];
                }
            }

            Console.SetCursorPosition(0, currentCursorY + _viewGameSettings.OceanHeight + 2);
        }

        private void DisplayInfo(in OceanData stats)
        {
            Console.WriteLine("\nINFO:\n");
            Console.WriteLine($"Cycle:     {stats.numCycles}    ");
            Console.WriteLine($"Predators: {stats.numPredators}    ");
            Console.WriteLine($"Prey:      {stats.numPreys}    ");
            Console.WriteLine($"Obstacles: {stats.numObstacles}    ");
        }

        #endregion


        #region =====----- PUBLIC METHODS -----=====

        public void Display(in Cell[,] field, in OceanData data)
        {
            Console.SetCursorPosition(0, 0);
            DisplayField(field);
            DisplayInfo(data);
        }

        public void Pause()
        {
            if (GetPause != null)
            {
                GetPause(this, null);
            }
        }

        public void ForceEnd()
        {
            if (GetEmergencyExit != null)
            {
                GetEmergencyExit(this, null);
            }
        }

        public void DisplayMessage(string msg)
        {
            Console.WriteLine();

            for (int i = 0; i < msg.Length + 4; i++)
            {
                Console.Write("-");
            }
            Console.Write($"\n♥ {msg} ♥\n");
            for (int i = 0; i < msg.Length + 4; i++)
            {
                Console.Write("-");
            }
            Console.Write("\n");
        }

        #endregion

    }
}
