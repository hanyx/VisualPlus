namespace VisualPlus.Renders
{
    #region Namespace

    using System.Drawing;
    using System.Drawing.Drawing2D;

    using VisualPlus.Enumerators;
    using VisualPlus.Structure;

    #endregion

    public sealed class VisualBackgroundRenderer
    {
        #region Events

        /// <summary>Draws a background with a color filled rectangle and the specified background image.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="backColor">The back Color.</param>
        /// <param name="backgroundImage">The background Image.</param>
        /// <param name="rounding">The rounding.</param>
        public static void DrawBackground(Graphics graphics, Rectangle rectangle, Color backColor, Image backgroundImage, int rounding)
        {
            GraphicsPath _controlGraphicsPath = FillBackgroundPath(graphics, rounding, rectangle, backColor);

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
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="backColor">The back Color.</param>
        public static void DrawBackground(Graphics graphics, Rectangle rectangle, Color backColor)
        {
            GraphicsPath _graphicsPath = new GraphicsPath();
            _graphicsPath.AddRectangle(rectangle);
            graphics.FillPath(new SolidBrush(backColor), _graphicsPath);
        }

        /// <summary>Draws the control background, with a BackColor and the specified BackgroundImage.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="backColor">The color to use for the background.</param>
        /// <param name="backgroundImage">The background image to use for the background.</param>
        /// <param name="shape">The shape settings.</param>
        /// <param name="mouseState">The mouse state.</param>
        public static void DrawBackground(Graphics graphics, Rectangle rectangle, Color backColor, Image backgroundImage, Border shape, MouseStates mouseState)
        {
            GraphicsPath _controlGraphicsPath = FillBackgroundPath(graphics, shape, rectangle, backColor);
            VisualBorderRenderer.DrawBorderStyle(graphics, shape, mouseState, _controlGraphicsPath);

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
        private static GraphicsPath FillBackgroundPath(Graphics graphics, Shape border, Rectangle rectangle, Color background)
        {
            GraphicsPath backgroundPath = VisualBorderRenderer.GetBorderShape(rectangle, border);
            graphics.SetClip(backgroundPath);
            graphics.FillRectangle(new SolidBrush(background), rectangle);
            graphics.ResetClip();
            return backgroundPath;
        }

        /// <summary>Fills the background graphics path.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rounding">The amount of rounding.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="background">The background color.</param>
        /// <returns>The background path filled.</returns>
        private static GraphicsPath FillBackgroundPath(Graphics graphics, int rounding, Rectangle rectangle, Color background)
        {
            GraphicsPath backgroundPath = GDI.DrawRoundedRectangle(rectangle, rounding);
            graphics.SetClip(backgroundPath);
            graphics.FillRectangle(new SolidBrush(background), rectangle);
            graphics.ResetClip();
            return backgroundPath;
        }

        #endregion
    }
}