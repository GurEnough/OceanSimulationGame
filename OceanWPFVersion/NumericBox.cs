using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;


namespace OceanWPFVersion
{
    public class NumericBox : TextBox
    {
        #region =====----- PRIVATE DATA -----=====

        private bool _maxRelated = false;
        private bool _minRelated = false;
        private double _minValue = -1;
        private double _maxValue = -1;

        #endregion

        #region =====----- PROPERTIES -----=====

        public double MinValue
        {
            get { return _minValue; }
            set
            {
                _minRelated = true;
                _minValue = value;
            }
        }
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxRelated = true;
                _maxValue = value;
            }
        }

        #endregion

        #region =====----- CTOR -----=====

        public NumericBox()
        {
            PreviewTextInput += NumberInputValidate;
        }

        #endregion

        #region =====----- PRIVATE METHODS -----=====

        private void NumberInputValidate(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9,]+");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }

            var text = Text + e.Text;

            bool success = Double.TryParse(text, out var val);
            if (!success
                || (_minRelated && val < _minValue)
                || (_maxRelated && val > _maxValue))
            {
                e.Handled = true;
            }
        }

        #endregion

    }


}
