namespace VisualPlus.Extensibility
{
    #region Namespace

    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;

    #endregion

    public static class StringExtension
    {
        #region Events

        /// <summary>Converts the string HTML to a color.</summary>
        /// <param name="withoutHash">The HTML color. (Don't include hash '#')</param>
        /// <returns>The <see cref="Color" />.</returns>
        public static Color FromHtml(this string withoutHash)
        {
            return ColorTranslator.FromHtml("#" + withoutHash);
        }

        /// <summary>Returns the text as a <see cref="string" /><see cref="Array" />.</summary>
        /// <param name="text">The text.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string[] Lines(this string text)
        {
            return text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        /// <summary>Provides the size, in pixels, of the specified text when drawn with the specified font.</summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to apply to the measured text.</param>
        /// <returns>The <see cref="Size" />.</returns>
        public static Size MeasureText(this string text, Font font)
        {
            return TextRenderer.MeasureText(text, font);
        }

        /// <summary>Converts the html <see cref="string" /> by the alpha value.</summary>
        /// <param name="htmlColor">The html color.</param>
        /// <param name="alpha">The alpha value.</param>
        /// <returns>The <see cref="Color" />.</returns>
        public static Color ToColor(this string htmlColor, int alpha = 255)
        {
            return Color.FromArgb(alpha > 255 ? 255 : alpha, ColorTranslator.FromHtml(htmlColor));
        }

        /// <summary>Converts the <see cref="string" /> to a <see cref="Font" />.</summary>
        /// <param name="fontName">The font name.</param>
        /// <param name="size">The size.</param>
        /// <param name="fontStyle">The font style.</param>
        /// <param name="unit">The unit.</param>
        /// <returns>The <see cref="Font" />.</returns>
        public static Font ToFont(this string fontName, float size, FontStyle fontStyle = FontStyle.Regular, GraphicsUnit unit = GraphicsUnit.Pixel)
        {
            return new Font(fontName, size, fontStyle, unit);
        }

        /// <summary>Converts the Base64 <see cref="string" /> to an <see cref="Image" />.</summary>
        /// <param name="base64Image">The Base64 value.</param>
        /// <returns>The <see cref="Image" />.</returns>
        public static Image ToImage(this string base64Image)
        {
            using (MemoryStream _image = new MemoryStream(Convert.FromBase64String(base64Image)))
            {
                return Image.FromStream(_image);
            }
        }

        /// <summary>Converts the html <see cref="string" /> by the alpha value.</summary>
        /// <param name="htmlColor">The html color.</param>
        /// <param name="alpha">The alpha value.</param>
        /// <param name="size">The size.</param>
        /// <param name="startCap">The start cap.</param>
        /// <param name="endCap">The end cap.</param>
        /// <returns>The <see cref="SolidBrush" />.</returns>
        public static Pen ToPen(this string htmlColor, int alpha = 255, float size = 1, LineCap startCap = LineCap.Custom, LineCap endCap = LineCap.Custom)
        {
            return new Pen(Color.FromArgb(alpha > 255 ? 255 : alpha, ColorTranslator.FromHtml(htmlColor)), size) { StartCap = startCap, EndCap = endCap };
        }

        /// <summary>Converts the html <see cref="string" /> by the alpha value.</summary>
        /// <param name="htmlColor">The html color.</param>
        /// <param name="alpha">The alpha value.</param>
        /// <returns>The <see cref="SolidBrush" />.</returns>
        public static SolidBrush ToSolidBrush(this string htmlColor, int alpha = 255)
        {
            return new SolidBrush(Color.FromArgb(alpha > 255 ? 255 : alpha, ColorTranslator.FromHtml(htmlColor)));
        }

        #endregion
    }
}