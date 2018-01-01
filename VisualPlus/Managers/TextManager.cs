namespace VisualPlus.Managers
{
    #region Namespace

    using System.Drawing;

    #endregion

    public sealed class TextManager
    {
        #region Events

        /// <summary>Set the string format alignments.</summary>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <param name="verticalAlignment">The vertical alignment.</param>
        /// <returns>The <see cref="StringFormat" />.</returns>
        public static StringFormat SetStringFormat(StringAlignment horizontalAlignment = StringAlignment.Center, StringAlignment verticalAlignment = StringAlignment.Center)
        {
            return new StringFormat { Alignment = horizontalAlignment, LineAlignment = verticalAlignment };
        }

        #endregion
    }
}