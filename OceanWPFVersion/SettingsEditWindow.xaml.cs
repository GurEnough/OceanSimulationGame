using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OceanLogic;

namespace OceanWPFVersion
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SettingsEditWindow : Window, ISettingsController
    {
        #region =====----- PRIVATE DATA -----=====

        private DefaultSettings _settings;

        private int _currCol = 0;
        private int _currRow = 0;

        #endregion

        #region =====----- EVENTS -----=====

        public event MenuEndEventHandler MenuExit;

        #endregion

        #region =====----- CTOR -----=====

        public SettingsEditWindow(DefaultSettings settings)
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            _settings = settings;

        }

        #endregion

        #region =====----- PRIVATE METHODS -----=====

        private void OnSave(object sender, RoutedEventArgs e)
        {
            var entries = new List<(string entry, double val)>();
            foreach (var c in mainGrid.Children)
            {
                if (c is TextBox)
                {
                    var box = c as TextBox;
                    Double.TryParse(box.Text, out var val);
                    entries.Add((box.Name, val));
                }
            }

            if (MenuExit != null)
            {
                MenuExit(this, new MenuEndEventArgs(entries));
            }

            Close();
        }

        #endregion


        #region =====----- PUBLIC METHODS -----=====

        public void AddEntry(string name, double defaultValue, double min, double max)
        {
            var label = new Label();

            label.ToolTip = $"{min} - {max}";
            label.Content = name;
            label.FontSize = 11;
            label.FontFamily = new FontFamily("Algerian");
            label.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
            label.HorizontalAlignment = HorizontalAlignment.Center;

            Grid.SetColumn(label, _currCol);
            Grid.SetRow(label, _currRow);
            mainGrid.Children.Add(label);

            var box = new NumericBox();

            box.Text = defaultValue.ToString();
            box.MinValue = min;
            box.MaxValue = max;
            box.Width = 100;
            box.Height = 25;
            box.TextAlignment = TextAlignment.Center;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.Margin = new Thickness(5, 0, 0, 0);
            box.Name = name;
            Grid.SetColumn(box, _currCol);
            Grid.SetRow(box, _currRow);
            mainGrid.Children.Add(box);

            if (++_currRow >= mainGrid.RowDefinitions.Count)
            {
                _currCol++;
                _currRow = 0;
            }

            if (_currCol >= mainGrid.ColumnDefinitions.Count)
            {
                throw new IndexOutOfRangeException("Too Many Fields!");
            }
        }

        public void Make()
        {
            Show();
        }

        #endregion

    }

}
