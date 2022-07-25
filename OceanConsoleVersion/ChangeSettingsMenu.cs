using OceanLogic;

namespace OceanConsoleVersion
{
    public class ChangeSettingsMenu : ISettingsController
    {
        #region =====----- PRIVATE DATA -----=====

        private int _positionY = 0;
        private int _currentHigh = 0;

        private List<(string entry, double val, double min, double max)> _entries = new List<(string, double, double, double)>();

        #endregion

        #region =====----- EVENTS -----=====

        public event MenuEndEventHandler MenuExit;

        #endregion

        #region =====----- CTOR -----=====

        public ChangeSettingsMenu()
        {
        }

        #endregion

        #region =====----- PUBLIC METHODS -----=====

        public void AddEntry(string name, double defaultValue, double min, double max)
        {
            _entries.Add((name, defaultValue, min, max));
        }

        public void Make()
        {
            Console.Clear();
            Console.WriteLine("Press Q to exit the settings menu.");
            Console.WriteLine("Use the arrow keys to navigate, Press Enter to edit or confirm\n\n");
            Console.CursorVisible = false;
            _positionY = Console.GetCursorPosition().Top;

            for (int i = 0; i < _entries.Count; i++)
            {
                DisplayEntry(i, false);
            }

            while (true)
            {

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey().Key;

                    if (key == ConsoleKey.Q)
                    {
                        InvertColor(false);
                        Console.CursorVisible = true;

                        if (MenuExit != null)
                        {
                            var entries = _entries.Select((x, i) => (x.entry, x.val)).ToList();
                            MenuExit(this, new MenuEndEventArgs(entries));
                        }

                        return;
                    }

                    else if (key == ConsoleKey.UpArrow)
                    {
                        PrevItem();
                    }

                    else if (key == ConsoleKey.DownArrow)
                    {
                        NextItem();
                    }

                    else if (key == ConsoleKey.Enter)
                    {
                        UpdateEntry(_currentHigh);
                    }

                    else
                    {
                        // Clear symbol
                        var pos = Console.GetCursorPosition();
                        Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        InvertColor(false);
                        Console.Write(" ");
                    }
                }
            }
        }

        #endregion

        #region =====----- PRIVATE METHODS -----=====

        private void NextItem()
        {
            DisplayEntry(_currentHigh, false);
            if (++_currentHigh >= _entries.Count)
            {
                _currentHigh = 0;
            }
            DisplayEntry(_currentHigh, true);
        }

        private void PrevItem()
        {
            DisplayEntry(_currentHigh, false);
            if (--_currentHigh < 0)
            {
                _currentHigh = _entries.Count - 1;
            }
            DisplayEntry(_currentHigh, true);
        }

        private void InvertColor(bool inv)
        {
            if (inv)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private void DisplayEntry(int index, bool inv)
        {
            InvertColor(false);
            Console.SetCursorPosition(0, index + _positionY);
            Console.Write($"{_entries[index].entry}: ");
            InvertColor(inv);
            Console.Write($"{_entries[index].val}");
        }

        private void ClearEntry(int index)
        {
            InvertColor(false);
            var posX = _entries[_currentHigh].entry.Length + 2;
            Console.SetCursorPosition(posX, index + _positionY);

            Console.Write("                               ");

            Console.SetCursorPosition(posX, index + _positionY);
        }

        private void UpdateEntry(int index)
        {
            ClearEntry(index);
            Console.CursorVisible = true;
            while (true)
            {
                bool success = double.TryParse(Console.ReadLine(), out var val);
                var entry = _entries[_currentHigh];
                if (success && val > entry.min && val < entry.max)
                {
                    _entries[_currentHigh] = (entry.entry, val, entry.min, entry.max);
                    DisplayEntry(index, true);
                    Console.CursorVisible = false;
                    return;
                }
                ClearEntry(index);
            }
        }

        #endregion

    }
}
