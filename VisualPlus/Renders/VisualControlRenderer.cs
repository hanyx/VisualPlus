namespace VisualPlus.Renders
{
    #region Namespace

    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Structure;

    #endregion

    public sealed class VisualControlRenderer
    {
        #region Events

        /// <summary>Draws a button control, with a colorGradientToggle, and the specified mouse state.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="font">The font to use in the string.</param>
        /// <param name="foreColor">The color of the string.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="border">The border type.</param>
        /// <param name="textImageRelation">The text image relation.</param>
        /// <param name="backgroundColor">The background color.</param>
        /// <param name="backgroundGradient">The background Gradient.</param>
        /// <param name="colorGradientToggle">The color Gradient Toggle.</param>
        /// <param name="mouseState">The mouse state.</param>
        public static void DrawButton(Graphics graphics, Rectangle rectangle, string text, Font font, Color foreColor, VisualBitmap image, Border border, TextImageRelation textImageRelation, Color backgroundColor, LinearGradientBrush backgroundGradient, bool colorGradientToggle, MouseStates mouseState)
        {
            if (colorGradientToggle)
            {
                VisualBackgroundRenderer.DrawBackground(graphics, border, rectangle, backgroundGradient, mouseState);
            }
            else
            {
                VisualBackgroundRenderer.DrawBackground(graphics, border, rectangle, backgroundColor, mouseState);
            }

            DrawInternalContent(graphics, rectangle, text, font, foreColor, image, textImageRelation);
        }

        /// <summary>Draws a button control, with a background color, and the specified mouse state.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="font">The font to use in the string.</param>
        /// <param name="foreColor">The color of the string.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="border">The border type.</param>
        /// <param name="textImageRelation">The text image relation.</param>
        /// <param name="background">The background color.</param>
        /// <param name="mouseState">The mouse state.</param>
        public static void DrawButton(Graphics graphics, Rectangle rectangle, string text, Font font, Color foreColor, VisualBitmap image, Border border, TextImageRelation textImageRelation, Color background, MouseStates mouseState)
        {
            VisualBackgroundRenderer.DrawBackground(graphics, border, rectangle, background, mouseState);
            DrawInternalContent(graphics, rectangle, text, font, foreColor, image, textImageRelation);
        }

        /// <summary>Draws a button control, with a background color.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="font">The font to use in the string.</param>
        /// <param name="foreColor">The color of the string.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="border">The border type.</param>
        /// <param name="textImageRelation">The text image relation.</param>
        /// <param name="background">The background color.</param>
        public static void DrawButton(Graphics graphics, Rectangle rectangle, string text, Font font, Color foreColor, VisualBitmap image, Border border, TextImageRelation textImageRelation, Color background)
        {
            VisualBackgroundRenderer.DrawBackground(graphics, border, rectangle, background);
            DrawInternalContent(graphics, rectangle, text, font, foreColor, image, textImageRelation);
        }

        /// <summary>Draws a button control, with a background gradient, and the specified mouse state.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="font">The font to use in the string.</param>
        /// <param name="foreColor">The color of the string.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="border">The border type.</param>
        /// <param name="textImageRelation">The text image relation.</param>
        /// <param name="background">The background gradient.</param>
        /// <param name="mouseState">The mouse state.</param>
        public static void DrawButton(Graphics graphics, Rectangle rectangle, string text, Font font, Color foreColor, VisualBitmap image, Border border, TextImageRelation textImageRelation, LinearGradientBrush background, MouseStates mouseState)
        {
            VisualBackgroundRenderer.DrawBackground(graphics, border, rectangle, background, mouseState);
            DrawInternalContent(graphics, rectangle, text, font, foreColor, image, textImageRelation);
        }

        /// <summary>Draws a button control, with a background gradient, and the specified mouse state.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="font">The font to use in the string.</param>
        /// <param name="foreColor">The color of the string.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="border">The border type.</param>
        /// <param name="textImageRelation">The text image relation.</param>
        /// <param name="background">The background gradient.</param>
        public static void DrawButton(Graphics graphics, Rectangle rectangle, string text, Font font, Color foreColor, VisualBitmap image, Border border, TextImageRelation textImageRelation, LinearGradientBrush background)
        {
            VisualBackgroundRenderer.DrawBackground(graphics, border, rectangle, background);
            DrawInternalContent(graphics, rectangle, text, font, foreColor, image, textImageRelation);
        }

        /// <summary>Draws the internal text and image content.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="font">The font to use in the string.</param>
        /// <param name="foreColor">The color of the string.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="textImageRelation">The text image relation.</param>
        public static void DrawInternalContent(Graphics graphics, Rectangle rectangle, string text, Font font, Color foreColor, VisualBitmap image, TextImageRelation textImageRelation)
        {
            image.Point = GDI.ApplyTextImageRelation(graphics, textImageRelation, new Rectangle(image.Point, image.Size), text, font, rectangle, true);
            Point textPoint = GDI.ApplyTextImageRelation(graphics, textImageRelation, new Rectangle(image.Point, image.Size), text, font, rectangle, false);
            VisualBitmap.DrawImage(graphics, image.Border, image.Point, image.Image, image.Size, image.Visible);
            graphics.DrawString(text, font, new SolidBrush(foreColor), textPoint);
        }

        #endregion
    }
}