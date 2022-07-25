using System.Runtime.InteropServices;
using OceanLogic;

namespace OceanConsoleVersion
{
    internal class Program
    {
        #region =====----- PRIVATE DATA -----=====

        private static bool[] _key = new bool[255];
        private static bool[] _push = new bool[255];

        #endregion

        [DllImport("user32.dll")]
        private static extern ushort GetAsyncKeyState(int key);

        static void Main(string[] args)
        {
            var settings = new DefaultSettings();


            Console.WriteLine("Push any key to start...");
            Console.WriteLine("Push P key to pause | | Push C key to change game settings");

            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.C)
            {
                var menu = new ChangeSettingsMenu();
                var editor = new GameSettingsController(menu);

                editor.SettingsEdited += (s, e) =>
                {
                    settings = e.settings;
                };

                editor.Edit(settings);
            }

            Console.Clear();

            var oceanView = new ConsoleOceanViewer(settings);
            var oceanController = new Controller(oceanView);

            oceanView.Pause();
            while (oceanController.IsOceanAlive())
            {
                UpdateKeyStates();
                if (WasKeyUp((int)ConsoleKey.P))
                {
                    oceanView.Pause();
                }
                // Esc
                if (WasKeyUp((int)ConsoleKey.Escape))
                {
                    oceanView.ForceEnd();
                    break;
                }
            }
            Console.ReadKey();
        }

        #region =====----- PRIVATE METHODS -----=====

        private static bool WasKeyUp(int key)
        {
            // Get KeyUp state and clear
            bool b = Program._key[key];
            Program._key[key] = false;
            return b;
        }

        private static void UpdateKeyStates()
        {
            for (int i = 0; i < _key.Length; i++)
            {
                bool state = GetAsyncKeyState(i) != 0;

                // if wasn`t pressed
                if (state && !_push[i])
                {
                    _push[i] = true;
                }
                // if was pressed
                else if (!state && _push[i])
                {
                    _key[i] = true; // set KeyUp
                    _push[i] = false; // clear press
                }
            }
        }

        #endregion

    }
}
