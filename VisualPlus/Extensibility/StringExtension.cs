namespace VisualPlus.Extensibility
{
    #region Namespace

    using System.Drawing;
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

        /// <summary>Provides the size, in pixels, of the specified text when drawn with the specified font.</summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to apply to the measured text.</param>
        /// <returns>The <see cref="Size" />.</returns>
        public static Size MeasureText(this string text, Font font)
        {
            return TextRenderer.MeasureText(text, font);
        }

        #endregion
    }
}