#region Namespace

using System.Drawing;
using System.Drawing.Drawing2D;
using VisualPlus.Enumerators;
using VisualPlus.Structure;

#endregion

namespace VisualPlus.Renders
{
    public sealed class VisualBackgroundRenderer
    {
        #region Events

        /// <summary>Draws a background with a border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="border">The border type.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="background">The background color.</param>
        /// <param name="mouseState">The control mouse state.</param>
        public static void DrawBackground(Graphics graphics, Border border, Rectangle rectangle, Color background, MouseStates mouseState)
        {
            GraphicsPath backgroundPath = FillBackgroundPath(graphics, border, rectangle, background);
            VisualBorderRenderer.DrawBorderStyle(graphics, border, mouseState, backgroundPath);
        }

        /// <summary>Draws a background with a still border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="border">The border type.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="background">The background color.</param>
        public static void DrawBackground(Graphics graphics, Border border, Rectangle rectangle, Color background)
        {
            GraphicsPath backgroundPath = FillBackgroundPath(graphics, border, rectangle, background);
            VisualBorderRenderer.DrawBorder(graphics, backgroundPath, border.Thickness, border.Color);
        }

        /// <summary>Draws a background with a still border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="background">The background color.</param>
        /// <param name="shape">The shape.</param>
        public static void DrawBackground(Graphics graphics, Rectangle rectangle, Color background, Shape shape)
        {
            GraphicsPath backgroundPath = VisualBorderRenderer.GetBorderShape(rectangle, shape.Type, shape.Rounding);
            graphics.SetClip(backgroundPath);
            graphics.FillRectangle(new SolidBrush(background), rectangle);
            graphics.ResetClip();
            VisualBorderRenderer.DrawBorder(graphics, backgroundPath, shape.Thickness, shape.Color);
        }

        /// <summary>Draws a background with a linear gradient still border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="border">The border type.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="background">The background linear gradient.</param>
        /// <param name="mouseState">The control mouse state.</param>
        public static void DrawBackground(Graphics graphics, Border border, Rectangle rectangle, LinearGradientBrush background, MouseStates mouseState)
        {
            GraphicsPath backgroundPath = VisualBorderRenderer.GetBorderShape(rectangle, border);
            GDI.FillBackground(graphics, backgroundPath, background);
            VisualBorderRenderer.DrawBorderStyle(graphics, border, mouseState, backgroundPath);
        }

        /// <summary>Draws a background with a linear gradient still border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="border">The border type.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="background">The background linear gradient.</param>
        public static void DrawBackground(Graphics graphics, Border border, Rectangle rectangle, LinearGradientBrush background)
        {
            GraphicsPath backgroundPath = VisualBorderRenderer.GetBorderShape(rectangle, border);
            GDI.FillBackground(graphics, backgroundPath, background);
            VisualBorderRenderer.DrawBorder(graphics, backgroundPath, border.Thickness, border.Color);
        }

        /// <summary>Fills the background graphics path.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="border">The border type.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="background">The background color.</param>
        /// <returns>The background path filled.</returns>
        private static GraphicsPath FillBackgroundPath(Graphics graphics, Border border, Rectangle rectangle, Color background)
        {
            GraphicsPath backgroundPath = VisualBorderRenderer.GetBorderShape(rectangle, border);
            graphics.SetClip(backgroundPath);
            graphics.FillRectangle(new SolidBrush(background), rectangle);
            graphics.ResetClip();
            return backgroundPath;
        }

        #endregion
    }
}