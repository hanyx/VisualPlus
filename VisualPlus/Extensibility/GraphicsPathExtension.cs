namespace VisualPlus.Extensibility
{
    #region Namespace

    using System.Drawing;
    using System.Drawing.Drawing2D;

    using VisualPlus.Renders;
    using VisualPlus.Structure;

    #endregion

    public static class GraphicsPathExtension
    {
        #region Events

        /// <summary>Converts the GraphicsPath to a border path.</summary>
        /// <param name="borderPath">The border path.</param>
        /// <param name="border">The border.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        public static GraphicsPath ToBorderPath(this GraphicsPath borderPath, Border border)
        {
            return VisualBorderRenderer.CreateBorderTypePath(borderPath.GetBounds().ToRectangle(), border);
        }

        /// <summary>Converts the Rectangle to a GraphicsPath.</summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        public static GraphicsPath ToGraphicsPath(this Rectangle rectangle)
        {
            GraphicsPath convertedPath = new GraphicsPath();
            convertedPath.AddRectangle(rectangle);
            return convertedPath;
        }

        #endregion
    }
}