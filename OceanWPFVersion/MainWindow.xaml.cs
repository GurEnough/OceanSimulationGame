using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using OceanLogic;

namespace OceanWPFVersion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IOceanViewer
    {
        #region =====----- PRIVATE DATA -----=====

        private const int SPRITE_WIDTH = 16;
        private const int SPRITE_HEIGHT = 16;

        private Controller _controller;

        private readonly ImageBrush _preyImg = DrawingImageUploader.Load(@"pack://application:,,/img/fish.png");
        private readonly ImageBrush _predatorImg = DrawingImageUploader.Load(@"pack://application:,,/img/shark.png");
        private readonly ImageBrush _obstacleImg = DrawingImageUploader.Load(@"pack://application:,,/img/oceanweed.png");

        private readonly DispatcherTimer _oceanTimer = new DispatcherTimer();
        private MediaPlayer mediaPlayer = new MediaPlayer();


        private bool _gameStarted = false;

        #endregion

        #region =====----- PROPERTIES -----=====

        public DefaultSettings _viewGameSettings { get; private set; }

        #endregion

        #region =====----- CTOR -----=====

        public MainWindow()
        {
            InitializeComponent();

            _viewGameSettings = new DefaultSettings();

            helpText.TextWrapping = TextWrapping.Wrap;
            helpText.Text =
                "Press P to pause/unpause game";

            mediaPlayer.Open(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\music\gamesound.mp3"));
            mediaPlayer.Play();

        }

        #endregion

        #region =====----- PUBLIC METHODS -----=====

        public void Pause()
        {
            if (!_oceanTimer.IsEnabled)
            {
                _oceanTimer.Start();
            }
            else
            {
                _oceanTimer.Stop();
            }
        }

        public void Step(object sender, EventArgs e)
        {
            if (GetStep != null)
            {
                GetStep(this, null);
            }
        }

        public void WatchAlive(object sender, EventArgs e)
        {
            if (!_controller.IsOceanAlive())
            {
                Application.Current.Shutdown();
            }
        }

        public void Display(in Cell[,] field, in OceanData stats)
        {
            canvas.Children.Clear();
            for (int i = 0; i < _viewGameSettings.OceanHeight; i++)
            {
                for (int j = 0; j < _viewGameSettings.OceanWidth; j++)
                {
                    DrawCell(field[i, j], j, i);
                }
            }
            statsText.Text =
                $"INFO:\n" +
                $"Cycle:     {stats.numCycles}\n" +
                $"Predators: {stats.numPredators}\n" +
                $"Prey:      {stats.numPreys}\n" +
                $"Obstacles: {stats.numObstacles}\n";
        }

        public void DisplayMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void InitializePropertyValues()
        {

            mediaPlayer.Volume = (double)volumeSlider.Value;
        }

        #endregion

        #region =====----- PUBLIC METHODS -----=====

        private void StartSettings(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            var settingsWindow = new SettingsEditWindow(_viewGameSettings);
            var editor = new GameSettingsController(settingsWindow);

            editor.SettingsEdited +=
                (object sender, SettingsSaveEventArgs e) =>
                {
                    _viewGameSettings = e.settings;
                    IsEnabled = true;
                };

            editor.Edit(_viewGameSettings);
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            CanvasProportion();

            _controller = new Controller(this);

            _oceanTimer.Tick += Step;
            _oceanTimer.Tick += WatchAlive;
            _oceanTimer.Interval = TimeSpan.FromMilliseconds(1000 / _viewGameSettings.IntervalOfIterations);

            _gameStarted = true;

            Pause(); // Start game

            startButton.IsEnabled = false;
            settingsButton.IsEnabled = false;
        }

        private void CanvasProportion()
        {
            canvas.Height = (SPRITE_HEIGHT + 2) * _viewGameSettings.OceanHeight;
            canvas.Width = (SPRITE_WIDTH + 2) * _viewGameSettings.OceanWidth;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P && _gameStarted)
            {
                Pause();
            }
            if (e.Key == Key.Escape)
            {
                mediaPlayer.Stop();
                _oceanTimer.Stop();
                DisplayMessage("♥ Game extremely stopped ♥");
                Application.Current.Shutdown();
            }
        }

        private void DrawCell(Cell cell, int x, int y)
        {
            ImageBrush img;

            switch (cell.Image)
            {
                case DefaultSettings.DEFAULT_OBSTACLE_IMAGE:
                    img = _obstacleImg;
                    break;
                case DefaultSettings.DEFAULT_PREDATOR_IMAGE:
                    img = _predatorImg;
                    break;
                case DefaultSettings.DEFAULT_PREY_IMAGE:
                    img = _preyImg;
                    break;
                default:
                    return;
            }

            var sprite = new Rectangle
            {
                Tag = "cell",
                Height = SPRITE_HEIGHT,
                Width = SPRITE_WIDTH,
                Fill = img
            };

            Canvas.SetLeft(sprite, x * (SPRITE_WIDTH + 2));
            Canvas.SetTop(sprite, y * (SPRITE_HEIGHT + 2));

            canvas.Children.Add(sprite);
        }

        private void ChangeMusicVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaPlayer.Volume = (double)volumeSlider.Value;
        }
        private void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
        {
            // The Stop method stops and resets the media to be played from
            // the beginning.

            mediaPlayer.Stop();
        }

        private void Element_MediaEnded(object sender, EventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void StopMusic(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            startMusicButton.Content = "Play Music";
        }



        private void StartMusic(object sender, RoutedEventArgs e)
        {
            InitializePropertyValues();

            if (startMusicButton.Content.ToString() == "Play Music")
            {
                mediaPlayer.Play();
                startMusicButton.Content = "Pause Music";
            }
            else
            {
                mediaPlayer.Pause();
                startMusicButton.Content = "Play Music";
            }

        }

        #endregion


        #region =====----- EVENTS -----=====

        public event EventHandler GetPause;
        public event EventHandler GetStep;
        public event EventHandler GetEmergencyExit;

        #endregion

    }
}
