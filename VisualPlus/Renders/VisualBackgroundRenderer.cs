namespace VisualPlus.Renders
{
    #region Namespace

    using System.Drawing;
    using System.Drawing.Drawing2D;

    using VisualPlus.Enumerators;
    using VisualPlus.Managers;
    using VisualPlus.Structure;

    #endregion

    public sealed class VisualBackgroundRenderer
    {
        #region Events

        /// <summary>Draws a background with a color filled rectangle and the specified background image.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="backColor">The back Color.</param>
        /// <param name="backgroundImage">The background Image.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="rounding">The rounding.</param>
        public static void DrawBackground(Graphics graphics, Color backColor, Image backgroundImage, Rectangle rectangle, int rounding)
        {
            GraphicsPath _controlGraphicsPath = FillBackgroundPath(graphics, backColor, rectangle, rounding);

            if (backgroundImage == null)
            {
                return;
            }

            Point _location = new Point(rectangle.Width - backgroundImage.Width, rectangle.Height - backgroundImage.Height);
            Size _size = new Size(backgroundImage.Width, backgroundImage.Height);
            graphics.SetClip(_controlGraphicsPath);
            graphics.DrawImage(backgroundImage, new Rectangle(_location, _size));
            graphics.ResetClip();
        }

        /// <summary>Draws a background with a color filled rectangle.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="backColor">The back Color.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        public static void DrawBackground(Graphics graphics, Color backColor, Rectangle rectangle)
        {
            GraphicsPath _graphicsPath = new GraphicsPath();
            _graphicsPath.AddRectangle(rectangle);
            graphics.FillPath(new SolidBrush(backColor), _graphicsPath);
        }

        /// <summary>Draws the control background, with a BackColor and the specified BackgroundImage.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="backColor">The color to use for the background.</param>
        /// <param name="backgroundImage">The background image to use for the background.</param>
        /// <param name="mouseState">The mouse state.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="border">The shape settings.</param>
        public static void DrawBackground(Graphics graphics, Color backColor, Image backgroundImage, MouseStates mouseState, Rectangle rectangle, Border border)
        {
            GraphicsPath _controlGraphicsPath = FillBackgroundPath(graphics, backColor, rectangle, border);

            if (backgroundImage != null)
            {
                Point _location = new Point(rectangle.Width - backgroundImage.Width, rectangle.Height - backgroundImage.Height);
                Size _size = new Size(backgroundImage.Width, backgroundImage.Height);
                graphics.SetClip(_controlGraphicsPath);
                graphics.DrawImage(backgroundImage, new Rectangle(_location, _size));
                graphics.ResetClip();
            }
        }

        /// <summary>Draws the control background, with a BackColor and the specified BackgroundImage.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="backColor">The color to use for the background.</param>
        /// <param name="mouseState">The mouse state.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="shape">The shape settings.</param>
        public static void DrawBackground(Graphics graphics, Color backColor, MouseStates mouseState, Rectangle rectangle, Border shape)
        {
            GraphicsPath _controlGraphicsPath = FillBackgroundPath(graphics, backColor, rectangle, shape);
            VisualBorderRenderer.DrawBorderStyle(graphics, shape, _controlGraphicsPath, mouseState);
        }

        /// <summary>Draws a background with a border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="background">The background color.</param>
        /// <param name="border">The border type.</param>
        /// <param name="mouseState">The control mouse state.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        public static void DrawBackground(Graphics graphics, Color background, Border border, MouseStates mouseState, Rectangle rectangle)
        {
            GraphicsPath backgroundPath = FillBackgroundPath(graphics, background, rectangle, border);
            VisualBorderRenderer.DrawBorderStyle(graphics, border, backgroundPath, mouseState);
        }

        /// <summary>Draws a background with a still border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="border">The border type.</param>
        /// <param name="background">The background color.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        public static void DrawBackground(Graphics graphics, Border border, Color background, Rectangle rectangle)
        {
            GraphicsPath backgroundPath = FillBackgroundPath(graphics, background, rectangle, border);
            VisualBorderRenderer.DrawBorder(graphics, backgroundPath, border.Color, thickness: border.Thickness);
        }

        /// <summary>Draws a background with a still border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="background">The background color.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="shape">The shape.</param>
        public static void DrawBackground(Graphics graphics, Color background, Rectangle rectangle, Shape shape)
        {
            GraphicsPath backgroundPath = VisualBorderRenderer.CreateBorderTypePath(rectangle, shape);
            graphics.SetClip(backgroundPath);
            graphics.FillRectangle(new SolidBrush(background), rectangle);
            graphics.ResetClip();
            VisualBorderRenderer.DrawBorder(graphics, backgroundPath, shape.Color, thickness: shape.Thickness);
        }

        /// <summary>Draws a background with a linear gradient still border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="background">The background linear gradient.</param>
        /// <param name="border">The border type.</param>
        /// <param name="mouseState">The control mouse state.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        public static void DrawBackground(Graphics graphics, LinearGradientBrush background, Border border, MouseStates mouseState, Rectangle rectangle)
        {
            GraphicsPath backgroundPath = VisualBorderRenderer.CreateBorderTypePath(rectangle, border);
            FillBackground(graphics, backgroundPath, background);
            VisualBorderRenderer.DrawBorderStyle(graphics, border, backgroundPath, mouseState);
        }

        /// <summary>Draws a background with a linear gradient still border style.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="background">The background linear gradient.</param>
        /// <param name="border">The border type.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        public static void DrawBackground(Graphics graphics, LinearGradientBrush background, Border border, Rectangle rectangle)
        {
            GraphicsPath backgroundPath = VisualBorderRenderer.CreateBorderTypePath(rectangle, border);
            FillBackground(graphics, backgroundPath, background);
            VisualBorderRenderer.DrawBorder(graphics, backgroundPath, border.Color, border.Thickness);
        }

        /// <summary>Fills the background.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="graphicsPath">The graphics path.</param>
        /// <param name="brush">The gradient brush.</param>
        public static void FillBackground(Graphics graphics, GraphicsPath graphicsPath, Brush brush)
        {
            graphics.FillPath(brush, graphicsPath);
        }

        /// <summary>Fills the background graphics path.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="background">The background color.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="border">The border type.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        private static GraphicsPath FillBackgroundPath(Graphics graphics, Color background, Rectangle rectangle, Shape border)
        {
            GraphicsPath backgroundPath = VisualBorderRenderer.CreateBorderTypePath(rectangle, border);
            graphics.SetClip(backgroundPath);
            graphics.FillRectangle(new SolidBrush(background), rectangle);
            graphics.ResetClip();
            return backgroundPath;
        }

        /// <summary>Fills the background graphics path.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="background">The background color.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="rounding">The amount of rounding.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        private static GraphicsPath FillBackgroundPath(Graphics graphics, Color background, Rectangle rectangle, int rounding)
        {
            GraphicsPath backgroundPath = GraphicsManager.DrawRoundedRectangle(rectangle, rounding);
            graphics.SetClip(backgroundPath);
            graphics.FillRectangle(new SolidBrush(background), rectangle);
            graphics.ResetClip();
            return backgroundPath;
        }

        #endregion
    }
}