namespace VisualPlus.Managers
{
    #region Namespace

    using System.Drawing;
    using System.Drawing.Drawing2D;

    #endregion

    public sealed class BrushManager
    {
        #region Events

        /// <summary>Creates a glow brush from the specified graphics path.</summary>
        /// <param name="centerColor">The center color of path gradient.</param>
        /// <param name="surroundColor">The array of colors correspond to the points in the path.</param>
        /// <param name="point">The focus point for the gradient offset.</param>
        /// <param name="graphicsPath">The graphics path.</param>
        /// <param name="wrapMode">The wrap mode.</param>
        /// <returns>The <see cref="Brush" />.</returns>
        public Brush GlowBrush(Color centerColor, Color[] surroundColor, PointF point, GraphicsPath graphicsPath, WrapMode wrapMode = WrapMode.Clamp)
        {
            return new PathGradientBrush(graphicsPath) { CenterColor = centerColor, SurroundColors = surroundColor, FocusScales = point, WrapMode = wrapMode };
        }

        /// <summary>Creates a glow brush from the specified the specified points.</summary>
        /// <param name="centerColor">The center color of path gradient.</param>
        /// <param name="surroundColor">The array of colors correspond to the points in the path.</param>
        /// <param name="point">The focus point for the gradient offset.</param>
        /// <param name="wrapMode">The wrap mode.</param>
        /// <returns>The <see cref="Brush" />.</returns>
        public Brush GlowBrush(Color centerColor, Color[] surroundColor, PointF[] point, WrapMode wrapMode = WrapMode.Clamp)
        {
            return new PathGradientBrush(point) { CenterColor = centerColor, SurroundColors = surroundColor, WrapMode = wrapMode };
        }

        #endregion
    }
}