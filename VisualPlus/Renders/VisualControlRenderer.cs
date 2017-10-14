namespace VisualPlus.Renders
{
    #region Namespace

    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Structure;

    #endregion

    public sealed class VisualControlRenderer
    {
        #region Events

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
            GraphicsPath _controlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(rectangle, border);

            VisualBackgroundRenderer.DrawBackground(graphics, backColor, backgroundImage, mouseState, rectangle, border);
            DrawInternalContent(graphics, rectangle, text, font, foreColor, image, textImageRelation);
            VisualBorderRenderer.DrawBorderStyle(graphics, border, _controlGraphicsPath, mouseState);
        }

        /// <summary>Draws the text and image content.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="font">The font to use in the string.</param>
        /// <param name="foreColor">The color of the string.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="imageSize">The image Size.</param>
        /// <param name="textImageRelation">The text image relation.</param>
        public static void DrawContent(Graphics graphics, Rectangle rectangle, string text, Font font, Color foreColor, Image image, Size imageSize, TextImageRelation textImageRelation)
        {
            Rectangle _imageRectangle = new Rectangle(new Point(), imageSize);
            Point _imagePoint = GDI.ApplyTextImageRelation(graphics, textImageRelation, _imageRectangle, text, font, rectangle, true);
            Point _textPoint = GDI.ApplyTextImageRelation(graphics, textImageRelation, _imageRectangle, text, font, rectangle, false);

            graphics.DrawImage(image, new Rectangle(_imagePoint, imageSize));
            graphics.DrawString(text, font, new SolidBrush(foreColor), _textPoint);
        }

        /// <summary>Draws the text content.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The coordinates of the rectangle to draw.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="font">The font to use in the string.</param>
        /// <param name="foreColor">The color of the string.</param>
        /// <param name="stringAlignment">The string Alignment.</param>
        public static void DrawContentText(Graphics graphics, Rectangle rectangle, string text, Font font, Color foreColor, StringAlignment stringAlignment)
        {
            const int Padding = 0;

            Size _textSize = GDI.MeasureText(graphics, text, font);
            int yPos = (rectangle.Height / 2) - (_textSize.Height / 2);
            Point _textPoint;

            switch (stringAlignment)
            {
                case StringAlignment.Near:
                    {
                        _textPoint = new Point(rectangle.X + Padding, yPos);
                        break;
                    }

                case StringAlignment.Center:
                    {
                        _textPoint = new Point((rectangle.Width / 2) - (_textSize.Width / 2), yPos);
                        break;
                    }

                case StringAlignment.Far:
                    {
                        _textPoint = new Point(rectangle.Width - Padding - _textSize.Width, yPos);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(stringAlignment), stringAlignment, null);
                    }
            }

            graphics.DrawString(text, font, new SolidBrush(foreColor), _textPoint);
        }

        /// <summary>Draws a hatch component on the specified path.</summary>
        /// <param name="graphics">The specified graphics to draw on.</param>
        /// <param name="hatch">The hatch type.</param>
        /// <param name="hatchGraphicsPath">The hatch path to fill.</param>
        public static void DrawHatch(Graphics graphics, Hatch hatch, GraphicsPath hatchGraphicsPath)
        {
            if (!hatch.Visible)
            {
                return;
            }

            HatchBrush _hatchBrush = new HatchBrush(hatch.Style, hatch.ForeColor, hatch.BackColor);
            using (TextureBrush _textureBrush = GDI.DrawTextureUsingHatch(_hatchBrush))
            {
                _textureBrush.ScaleTransform(hatch.Size.Width, hatch.Size.Height);
                graphics.FillPath(_textureBrush, hatchGraphicsPath);
            }
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