namespace VisualPlus.Managers
{
    #region Namespace

    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.IO;
    using System.Runtime.InteropServices;

    #endregion

    public sealed class FontManager
    {
        #region Events

        /// <summary>Construct a font from a bytes array.</summary>
        /// <param name="bytes">The bytes array.</param>
        /// <param name="size">The size.</param>
        /// <param name="fontStyle">The font style.</param>
        /// <returns>The <see cref="Font" />.</returns>
        public static Font ConstructFont(byte[] bytes, float size, FontStyle fontStyle = FontStyle.Regular)
        {
            var _font = bytes;
            IntPtr _buffer = Marshal.AllocCoTaskMem(_font.Length);
            Marshal.Copy(_font, 0, _buffer, _font.Length);

            using (PrivateFontCollection _privateFontCollection = new PrivateFontCollection())
            {
                _privateFontCollection.AddMemoryFont(_buffer, _font.Length);
                return new Font(_privateFontCollection.Families[0].Name, size, fontStyle);
            }
        }

        /// <summary>Construct a font from a font file.</summary>
        /// <param name="fontPath">The font path.</param>
        /// <param name="size">The size.</param>
        /// <param name="fontStyle">The font style.</param>
        /// <returns>The <see cref="Font" />.</returns>
        public static Font ConstructFont(string fontPath, float size, FontStyle fontStyle = FontStyle.Regular)
        {
            var _font = File.ReadAllBytes(fontPath);
            IntPtr _buffer = Marshal.AllocCoTaskMem(_font.Length);
            Marshal.Copy(_font, 0, _buffer, _font.Length);

            using (PrivateFontCollection _privateFontCollection = new PrivateFontCollection())
            {
                _privateFontCollection.AddMemoryFont(_buffer, _font.Length);
                return new Font(_privateFontCollection.Families[0].Name, size, fontStyle);
            }
        }

        #endregion
    }
}