namespace VisualPlus.Renders
{
    #region Namespace

    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;

    using VisualPlus.Enumerators;
    using VisualPlus.Extensibility;
    using VisualPlus.Structure;

    #endregion

    public sealed class VisualToggleRenderer
    {
        #region Events

        /// <summary>Draws a check box control in the specified state and location.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="border">The border type.</param>
        /// <param name="checkmark">The check mark type.</param>
        /// <param name="rectangle">The rectangle that represents the dimensions of the check box.</param>
        /// <param name="state">The toggle state of the check mark.</param>
        /// <param name="enabled">The state to draw the check mark in.</param>
        /// <param name="linearGradientBrush">The brush used to fill the background.</param>
        /// <param name="mouseState">The state of the mouse on the control.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="foreColor">The fore Color.</param>
        /// <param name="textPoint">The text Point.</param>
        public static void DrawCheckBox(Graphics graphics, Border border, Checkmark checkmark, Rectangle rectangle, bool state, bool enabled, LinearGradientBrush linearGradientBrush, MouseStates mouseState, string text, Font font, Color foreColor, Point textPoint)
        {
            DrawCheckBox(graphics, border, checkmark, rectangle, state, enabled, linearGradientBrush, mouseState);
            graphics.DrawString(text, font, new SolidBrush(foreColor), textPoint);
        }

        /// <summary>Draws a check box control in the specified state and location.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="border">The border type.</param>
        /// <param name="checkmark">The check mark type.</param>
        /// <param name="rectangle">The rectangle that represents the dimensions of the check box.</param>
        /// <param name="state">The toggle state of the check mark.</param>
        /// <param name="enabled">The state to draw the check mark in.</param>
        /// <param name="linearGradientBrush">The brush used to fill the background.</param>
        /// <param name="mouseState">The state of the mouse on the control.</param>
        public static void DrawCheckBox(Graphics graphics, Border border, Checkmark checkmark, Rectangle rectangle, bool state, bool enabled, LinearGradientBrush linearGradientBrush, MouseStates mouseState)
        {
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.GammaCorrected;

            Rectangle _box = rectangle;
            GraphicsPath _boxPath = VisualBorderRenderer.GetBorderShape(_box, border);

            GDI.FillBackground(graphics, _boxPath, linearGradientBrush);

            if (state)
            {
                graphics.SetClip(_boxPath);
                DrawCheckMark(graphics, checkmark, _box, enabled);
                graphics.ResetClip();
            }

            VisualBorderRenderer.DrawBorderStyle(graphics, border, mouseState, _boxPath);
        }

        /// <summary>
        ///     Draws a check mark control in the specified state, on the specified graphics surface, and within the specified
        ///     bounds.
        /// </summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="checkmark">The check mark type.</param>
        /// <param name="rectangle">The rectangle that represents the dimensions of the check box.</param>
        /// <param name="enabled">The state to draw the check mark in.</param>
        public static void DrawCheckMark(Graphics graphics, Checkmark checkmark, Rectangle rectangle, bool enabled)
        {
            Gradient checkGradient = enabled ? checkmark.EnabledGradient : checkmark.DisabledGradient;
            Bitmap checkImage = enabled ? checkmark.EnabledImage : checkmark.DisabledImage;

            var boxGradientPoints = GDI.GetGradientPoints(rectangle);
            LinearGradientBrush checkmarkBrush = Gradient.CreateGradientBrush(checkGradient.Colors, boxGradientPoints, checkGradient.Angle, checkGradient.Positions);

            Size characterSize = GDI.MeasureText(graphics, checkmark.Character.ToString(), checkmark.Font);

            int stylesCount = checkmark.Style.Count();
            var autoLocations = new Point[stylesCount];
            autoLocations[0] = new Point((rectangle.X + (rectangle.Width / 2)) - (characterSize.Width / 2), (rectangle.Y + (rectangle.Height / 2)) - (characterSize.Height / 2));
            autoLocations[1] = new Point((rectangle.X + (rectangle.Width / 2)) - (checkmark.ImageSize.Width / 2), (rectangle.Y + (rectangle.Height / 2)) - (checkmark.ImageSize.Height / 2));
            autoLocations[2] = new Point((rectangle.X + (rectangle.Width / 2)) - (checkmark.ShapeSize.Width / 2), (rectangle.Y + (rectangle.Height / 2)) - (checkmark.ShapeSize.Height / 2));

            Point tempPoint;
            if (checkmark.AutoSize)
            {
                int styleIndex = checkmark.Style.GetIndexByValue(checkmark.Style.ToString());
                tempPoint = autoLocations[styleIndex];
            }
            else
            {
                tempPoint = checkmark.Location;
            }

            switch (checkmark.Style)
            {
                case Checkmark.CheckType.Character:
                    {
                        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                        graphics.DrawString(checkmark.Character.ToString(), checkmark.Font, checkmarkBrush, tempPoint);
                        graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
                        break;
                    }

                case Checkmark.CheckType.Image:
                    {
                        Rectangle checkImageRectangle = new Rectangle(tempPoint, checkmark.ImageSize);
                        graphics.DrawImage(checkImage, checkImageRectangle);
                        break;
                    }

                case Checkmark.CheckType.Shape:
                    {
                        Rectangle shapeRectangle = new Rectangle(tempPoint, checkmark.ShapeSize);
                        GraphicsPath shapePath = VisualBorderRenderer.GetBorderShape(shapeRectangle, checkmark.ShapeType, checkmark.ShapeRounding);
                        graphics.FillPath(checkmarkBrush, shapePath);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetBase64CheckImage()
        {
            return
                "iVBORw0KGgoAAAANSUhEUgAAABMAAAAQCAYAAAD0xERiAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEySURBVDhPY/hPRUBdw/79+/efVHz77bf/X37+wRAn2bDff/7+91l+83/YmtsYBpJs2ITjz/8rTbrwP2Dlrf9XXn5FkSPJsD13P/y3nHsVbNjyy28w5Ik27NWXX//TNt8DG1S19zFWNRiGvfzy8//ccy9RxEB4wvFnYIMMZl7+//brLwx5EEYx7MP33/9dF18Ha1py8RVcHBR7mlMvgsVXX8X0Hgwz/P379z8yLtz5AKxJdcpFcBj9+v3nf/CqW2Cx5E13UdSiYwzDvv36/d9/BUSzzvRL/0t2PQSzQd57+vEHilp0jGEYCJ9+8hnuGhiee+4Vhjp0jNUwEN566/1/m/mQZJC/48H/zz9+YVWHjHEaBsKgwAZ59eH771jl0TFew0D48osvWMWxYYKGEY///gcAqiuA6kEmfEMAAAAASUVORK5CYII=";
        }

        #endregion
    }
}