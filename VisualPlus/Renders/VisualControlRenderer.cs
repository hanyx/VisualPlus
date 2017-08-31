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

        /// <summary>Draws a hatch component on the specified path.</summary>
        /// <param name="graphics">The specified graphics to draw on.</param>
        /// <param name="hatch">The hatch type.</param>
        /// <param name="hatchGraphicsPath">The hatch path to fill.</param>
        public static void DrawHatch(Graphics graphics, Hatch hatch, GraphicsPath hatchGraphicsPath)
        {
            if (hatch.Visible)
            {
                HatchBrush _hatchBrush = new HatchBrush(hatch.Style, hatch.ForeColor, hatch.BackColor);
                using (TextureBrush _textureBrush = GDI.DrawTextureUsingHatch(_hatchBrush))
                {
                    _textureBrush.ScaleTransform(hatch.Size.Width, hatch.Size.Height);
                    graphics.FillPath(_textureBrush, hatchGraphicsPath);
                }

            }
        }

        /// <summary>Draws a button control.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="backColor">The BackColor of the button.</param>
        /// <param name="backgroundImage">The background image for the button.</param>
        /// <param name="border">The border.</param>
        /// <param name="mouseState">The mouse State.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="font">The font to use in the string.</param>
        /// <param name="foreColor">The color of the string.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="textImageRelation">The text image relation.</param>
        public static void DrawButton(Graphics graphics, Rectangle rectangle, Color backColor, Image backgroundImage, Border border, MouseStates mouseState, string text, Font font, Color foreColor, VisualBitmap image, TextImageRelation textImageRelation)
        {
            VisualBackgroundRenderer.DrawBackground(graphics, rectangle, backColor, backgroundImage, border, mouseState);
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