namespace VisualPlus.Extensibility
{
    #region Namespace

    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;

    #endregion

    public static class ImageExtensions
    {
        #region Events

        /// <summary>Creates a Base64 string value from the image.</summary>
        /// <param name="image">The image.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string ToBase64(this Image image)
        {
            using (MemoryStream _base64 = new MemoryStream())
            {
                image.Save(_base64, image.RawFormat);
                image.Dispose();
                return Convert.ToBase64String(_base64.ToArray());
            }
        }

        /// <summary>Creates a pen from the image.</summary>
        /// <param name="image">The image.</param>
        /// <param name="width">The width.</param>
        /// <param name="startCap">The start cap.</param>
        /// <param name="endCap">The end cap.</param>
        /// <returns>The <see cref="Pen" />.</returns>
        public static Pen ToPen(this Image image, float width = 1, LineCap startCap = LineCap.Custom, LineCap endCap = LineCap.Custom)
        {
            using (TextureBrush _textureBrush = new TextureBrush(image))
            {
                return new Pen(_textureBrush, width) { StartCap = startCap, EndCap = endCap };
            }
        }

        /// <summary>Creates a texture brush from the image.</summary>
        /// <param name="image">The image.</param>
        /// <returns>The <see cref="TextureBrush" />.</returns>
        public static TextureBrush ToTextureBrush(this Image image)
        {
            return new TextureBrush(image);
        }

        /// <summary>Creates a texture brush from the image.</summary>
        /// <param name="image">The image.</param>
        /// <param name="rectangle">The rectangle boundaries.</param>
        /// <returns>The <see cref="TextureBrush" />.</returns>
        public static TextureBrush ToTextureBrush(this Image image, Rectangle rectangle)
        {
            return new TextureBrush(image, rectangle);
        }

        #endregion
    }
}