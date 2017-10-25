namespace VisualPlus.Managers
{
    #region Namespace

    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    #endregion

    internal class ImageManager
    {
        #region Events

        /// <summary>Sets the image opacity.</summary>
        /// <param name="image">The image.</param>
        /// <param name="opacity">The opacity value.</param>
        /// <returns>The <see cref="Image"/>.</returns>
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