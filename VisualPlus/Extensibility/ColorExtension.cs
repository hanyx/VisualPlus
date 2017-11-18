namespace VisualPlus.Extensibility
{
    #region Namespace

    using System.Drawing;

    #endregion

    public static class ColorExtension
    {
        #region Events

        /// <summary>Converts the string HTML to a color.</summary>
        /// <param name="color">The color.</param>
        /// <param name="withoutHash">The HTML color. (Don't include hash '#')</param>
        /// <returns>The <see cref="Color" />.</returns>
        public static Color FromHTML(this Color color, string withoutHash)
        {
            return ColorTranslator.FromHtml("#" + withoutHash);
        }

        /// <summary>Converts the color to HTML string.</summary>
        /// <param name="color">The color to convert to HTML.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string ToHTML(this Color color)
        {
            return ColorTranslator.ToHtml(color);
        }

        #endregion
    }
}