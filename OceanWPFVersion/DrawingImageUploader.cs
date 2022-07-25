using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OceanWPFVersion
{
    public static class DrawingImageUploader
    {
        #region =====----- PUBLIC METHODS -----=====

        public static ImageBrush Load(string path)
        {
            var _bitmapImage = new BitmapImage(new Uri(path));
            var _img = new ImageBrush();
            _img.ImageSource = _bitmapImage;
            return _img;
        }

        #endregion

    }
}
