namespace VisualPlus.Extensibility
{
    #region Namespace

    using System.Drawing;

    #endregion

    public static class ColorExtension
    {
        #region Events

        /// <summary>Converts the string HTML to a <see cref="Color" />.</summary>
        /// <param name="color">The color.</param>
        /// <param name="withoutHash">The HTML color. (Don't include hash '#')</param>
        /// <returns>The <see cref="Color" />.</returns>
        public static Color FromHTML(this Color color, string withoutHash)
        {
            return ColorTranslator.FromHtml("#" + withoutHash);
        }

        /// <summary>Converts the color mix to a color.</summary>
        /// <param name="colors">The colors.</param>
        /// <returns>The <see cref="Color" />.</returns>
        public static Color MixColors(this Color[] colors)
        {
            int r = default(int);
            int g = default(int);
            int b = default(int);

            foreach (Color _color in colors)
            {
                r += _color.R;
                g += _color.B;
                b += _color.B;
            }

            return Color.FromArgb(r / colors.Length, g / colors.Length, b / colors.Length);
        }

        /// <summary>Converts the <see cref="Color" /> to an ARGB <see cref="string" />.</summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string ToARGB(this Color color)
        {
            return $"ARGB:({color.A}, {color.R}, {color.G}, {color.B})";
        }

        /// <summary>Converts the <see cref="Color" /> to HTML string.</summary>
        /// <param name="color">The color to convert to HTML.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string ToHTML(this Color color)
        {
            if (color == Color.Transparent)
            {
                return "#00FFFFFF";
            }
            else
            {
                return ColorTranslator.ToHtml(color);
            }
        }

        /// <summary>Converts the <see cref="Color" /> to a <see cref="Pen" />.</summary>
        /// <param name="color">The color.</param>
        /// <param name="size">The size.</param>
        /// <returns>The <see cref="Pen" />.</returns>
        public static Pen ToPen(this Color color, float size = 1)
        {
            return new Pen(color, size);
        }

        /// <summary>Converts the <see cref="Color" /> to an RGB <see cref="string" />.</summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string ToRGB(this Color color)
        {
            return $"RGB:({color.R}, {color.G}, {color.B})";
        }

        /// <summary>Converts the <see cref="Color" /> to an RGBA <see cref="string" />.</summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string ToRGBA(this Color color)
        {
            return $"RGBA:({color.R}, {color.G}, {color.B}, {color.A})";
        }

        /// <summary>Converts the <see cref="Color" /> to a <see cref="SolidBrush" />.</summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="SolidBrush" />.</returns>
        public static SolidBrush ToSolidBrush(this Color color)
        {
            return new SolidBrush(color);
        }

        #endregion
    }
}