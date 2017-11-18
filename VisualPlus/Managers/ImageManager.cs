namespace VisualPlus.Managers
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Windows.Forms;

    #endregion

    [Description("The image manager.")]
    public sealed class ImageManager
    {
        #region Events

        /// <summary>Creates a gradient bitmap.</summary>
        /// <param name="size">The size of the gradient.</param>
        /// <param name="topLeft">The color for top-left.</param>
        /// <param name="topRight">The color for top-right.</param>
        /// <param name="bottomLeft">The color for bottom-left.</param>
        /// <param name="bottomRight">The color for bottom-right.</param>
        /// <returns>The <see cref="Bitmap" />.</returns>
        public static Image CreateGradientBitmap(Size size, Color topLeft, Color topRight, Color bottomLeft, Color bottomRight)
        {
            Bitmap _bitmap = new Bitmap(size.Width, size.Height);

            for (var i = 0; i < _bitmap.Width; i++)
            {
                Color _xColor = ColorManager.TransitionColor(int.Parse(Math.Round((i / (double)_bitmap.Width) * 100.0, 0).ToString(CultureInfo.CurrentCulture)), topLeft, topRight);
                for (var j = 0; j < _bitmap.Height; j++)
                {
                    Color _yColor = ColorManager.TransitionColor(int.Parse(Math.Round((j / (double)_bitmap.Height) * 100.0, 0).ToString(CultureInfo.CurrentCulture)), bottomLeft, bottomRight);
                    _bitmap.SetPixel(i, j, ColorManager.InsertColor(_xColor, _yColor));
                }
            }

            return _bitmap;
        }

        /// <summary>Sets the image opacity.</summary>
        /// <param name="image">The image.</param>
        /// <param name="opacity">The opacity value.</param>
        /// <returns>The <see cref="Image" />.</returns>
        public static Image SetOpacity(Image image, float opacity)
        {
            try
            {
                // create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                // create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {
                    // create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    // set the opacity  
                    matrix.Matrix33 = opacity;

                    // create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    // set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    // now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }

                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        #endregion
    }
}