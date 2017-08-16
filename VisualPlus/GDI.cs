namespace VisualPlus
{
    #region Namespace

    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Controls;

    #endregion

    public sealed class GDI
    {
        #region Events

        /// <summary>Anchors the rectangle to an anchored alignment of the base rectangle.</summary>
        /// <param name="anchorStyle">Alignment style.</param>
        /// <param name="baseRectangle">Base rectangle.</param>
        /// <param name="anchorWidth">Anchor width.</param>
        /// <returns>Anchored rectangle.</returns>
        public static Rectangle ApplyAnchor(TabAlignment anchorStyle, Rectangle baseRectangle, int anchorWidth)
        {
            Point anchoredLocation;
            Size anchoredSize;

            switch (anchorStyle)
            {
                case TabAlignment.Top:
                    {
                        anchoredLocation = new Point(baseRectangle.X, baseRectangle.Y);
                        anchoredSize = new Size(baseRectangle.Width, anchorWidth);
                        break;
                    }

                case TabAlignment.Bottom:
                    {
                        anchoredLocation = new Point(baseRectangle.X, baseRectangle.Bottom - anchorWidth);
                        anchoredSize = new Size(baseRectangle.Width, anchorWidth);
                        break;
                    }

                case TabAlignment.Left:
                    {
                        anchoredLocation = new Point(baseRectangle.X, baseRectangle.Y);
                        anchoredSize = new Size(anchorWidth, baseRectangle.Height);
                        break;
                    }

                case TabAlignment.Right:
                    {
                        anchoredLocation = new Point(baseRectangle.Right - anchorWidth, baseRectangle.Y);
                        anchoredSize = new Size(anchorWidth, baseRectangle.Height);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(anchorStyle), anchorStyle, null);
            }

            Rectangle anchoredRectangle = new Rectangle(anchoredLocation, anchoredSize);
            return anchoredRectangle;
        }

        /// <summary>Apply BackColor change on the container and it's child controls.</summary>
        /// <param name="container">The container control.</param>
        /// <param name="backgroundColor">The container backgroundColor.</param>
        public static void ApplyContainerBackColorChange(Control container, Color backgroundColor)
        {
            foreach (object control in container.Controls)
            {
                if (control != null)
                {
                    ((Control)control).BackColor = backgroundColor;
                }
            }
        }

        /// <summary>Draws the text image relation.</summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="relation">The relation type.</param>
        /// <param name="imageRectangle">The image rectangle.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="outerBounds">The outer bounds.</param>
        /// <param name="imagePoint">Return image point.</param>
        /// <returns>The return point.</returns>
        public static Point ApplyTextImageRelation(Graphics graphics, TextImageRelation relation, Rectangle imageRectangle, string text, Font font, Rectangle outerBounds, bool imagePoint)
        {
            Point newPosition = new Point(0, 0);
            Point newImagePoint = new Point(0, 0);
            Point newTextPoint = new Point(0, 0);
            Size textSize = MeasureText(graphics, text, font);

            switch (relation)
            {
                case TextImageRelation.Overlay:
                    {
                        // Set center
                        newPosition.X = outerBounds.Width / 2;
                        newPosition.Y = outerBounds.Height / 2;

                        // Set image
                        newImagePoint.X = newPosition.X - (imageRectangle.Width / 2);
                        newImagePoint.Y = newPosition.Y - (imageRectangle.Height / 2);

                        // Set text
                        newTextPoint.X = newPosition.X - (textSize.Width / 2);
                        newTextPoint.Y = newPosition.Y - (textSize.Height / 2);
                        break;
                    }

                case TextImageRelation.ImageBeforeText:
                    {
                        // Set center
                        newPosition.Y = outerBounds.Height / 2;

                        // Set image
                        newImagePoint.X = newPosition.X + 4;
                        newImagePoint.Y = newPosition.Y - (imageRectangle.Height / 2);

                        // Set text
                        newTextPoint.X = newImagePoint.X + imageRectangle.Width;
                        newTextPoint.Y = newPosition.Y - (textSize.Height / 2);
                        break;
                    }

                case TextImageRelation.TextBeforeImage:
                    {
                        // Set center
                        newPosition.Y = outerBounds.Height / 2;

                        // Set text
                        newTextPoint.X = newPosition.X + 4;
                        newTextPoint.Y = newPosition.Y - (textSize.Height / 2);

                        // Set image
                        newImagePoint.X = newTextPoint.X + textSize.Width;
                        newImagePoint.Y = newPosition.Y - (imageRectangle.Height / 2);
                        break;
                    }

                case TextImageRelation.ImageAboveText:
                    {
                        // Set center
                        newPosition.X = outerBounds.Width / 2;

                        // Set image
                        newImagePoint.X = newPosition.X - (imageRectangle.Width / 2);
                        newImagePoint.Y = newPosition.Y + 4;

                        // Set text
                        newTextPoint.X = newPosition.X - (textSize.Width / 2);
                        newTextPoint.Y = newImagePoint.Y + imageRectangle.Height;
                        break;
                    }

                case TextImageRelation.TextAboveImage:
                    {
                        // Set center
                        newPosition.X = outerBounds.Width / 2;

                        // Set text
                        newTextPoint.X = newPosition.X - (textSize.Width / 2);
                        newTextPoint.Y = newImagePoint.Y + 4;

                        // Set image
                        newImagePoint.X = newPosition.X - (imageRectangle.Width / 2);
                        newImagePoint.Y = newPosition.Y + textSize.Height + 4;
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(relation), relation, null);
            }

            if (imagePoint)
            {
                return newImagePoint;
            }
            else
            {
                return newTextPoint;
            }
        }

        /// <summary>Calculates a 5 point star.</summary>
        /// <param name="originF"> The originF is the middle of the star.</param>
        /// <param name="outerRadius">Radius of the surrounding circle.</param>
        /// <param name="innerRadius">Radius of the circle for the "inner" points</param>
        /// <returns>10 PointF array.</returns>
        public static PointF[] Calculate5PointStar(PointF originF, float outerRadius, float innerRadius)
        {
            // Define some variables to avoid as much calculations as possible
            // conversions to radians
            const double Ang36 = Math.PI / 5.0; // 36Â° x PI/180
            const double Ang72 = 2.0 * Ang36; // 72Â° x PI/180

            // some sine and cosine values we need
            var sin36 = (float)Math.Sin(Ang36);
            var sin72 = (float)Math.Sin(Ang72);
            var cos36 = (float)Math.Cos(Ang36);
            var cos72 = (float)Math.Cos(Ang72);

            // Fill array with 10 originF points
            PointF[] pointsArray = { originF, originF, originF, originF, originF, originF, originF, originF, originF, originF };
            pointsArray[0].Y -= outerRadius; // top off the star, or on a clock this is 12:00 or 0:00 hours
            pointsArray[1].X += innerRadius * sin36;
            pointsArray[1].Y -= innerRadius * cos36; // 0:06 hours
            pointsArray[2].X += outerRadius * sin72;
            pointsArray[2].Y -= outerRadius * cos72; // 0:12 hours
            pointsArray[3].X += innerRadius * sin72;
            pointsArray[3].Y += innerRadius * cos72; // 0:18
            pointsArray[4].X += outerRadius * sin36;
            pointsArray[4].Y += outerRadius * cos36; // 0:24 

            // Phew! Glad I got that trig working.
            pointsArray[5].Y += innerRadius;

            // I use the symmetry of the star figure here
            pointsArray[6].X += pointsArray[6].X - pointsArray[4].X;
            pointsArray[6].Y = pointsArray[4].Y; // mirror point
            pointsArray[7].X += pointsArray[7].X - pointsArray[3].X;
            pointsArray[7].Y = pointsArray[3].Y; // mirror point
            pointsArray[8].X += pointsArray[8].X - pointsArray[2].X;
            pointsArray[8].Y = pointsArray[2].Y; // mirror point
            pointsArray[9].X += pointsArray[9].X - pointsArray[1].X;
            pointsArray[9].Y = pointsArray[1].Y; // mirror point

            return pointsArray;
        }

        /// <summary>Draws the control.</summary>
        /// <param name="control">The control to draw.</param>
        /// <param name="point">The point.</param>
        public static void DrawControl(Control control, Point point)
        {
            Bitmap _bitmap = new Bitmap(control.Size.Width, control.Size.Height);
            control.DrawToBitmap(_bitmap, new Rectangle(point.X, point.Y, _bitmap.Width, _bitmap.Height));
        }

        /// <summary>Draws the rounded rectangle from specific values.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="curve">The curve.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        public static GraphicsPath DrawRoundedRectangle(int x, int y, int width, int height, int curve)
        {
            Rectangle rectangleShape = new Rectangle(x, y, width, height);
            GraphicsPath graphicsPath = DrawRoundedRectangle(rectangleShape, curve);
            return graphicsPath;
        }

        /// <summary> Draws the rounded rectangle from a rectangle shape.</summary>
        /// <param name="rectangleShape">The rectangle shape.</param>
        /// <param name="curve">The curve.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        public static GraphicsPath DrawRoundedRectangle(Rectangle rectangleShape, int curve)
        {
            GraphicsPath graphicPath = new GraphicsPath(FillMode.Winding);
            graphicPath.AddArc(rectangleShape.X, rectangleShape.Y, curve, curve, 180.0F, 90.0F);
            graphicPath.AddArc(rectangleShape.Right - curve, rectangleShape.Y, curve, curve, 270.0F, 90.0F);
            graphicPath.AddArc(rectangleShape.Right - curve, rectangleShape.Bottom - curve, curve, curve, 0.0F, 90.0F);
            graphicPath.AddArc(rectangleShape.X, rectangleShape.Bottom - curve, curve, curve, 90.0F, 90.0F);
            graphicPath.CloseFigure();
            return graphicPath;
        }

        /// <summary>Draws the hatch brush as an image and then converts it to a texture brush for scaling.</summary>
        /// <param name="hatchBrush">Hatch brush pattern.</param>
        /// <returns>Texture brush.</returns>
        public static TextureBrush DrawTextureUsingHatch(HatchBrush hatchBrush)
        {
            using (Bitmap bitmap = new Bitmap(8, 8))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(hatchBrush, 0, 0, 8, 8);
                return new TextureBrush(bitmap);
            }
        }

        /// <summary>Draws the tick line.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="drawRect">The rectangle</param>
        /// <param name="tickFrequency">Tick frequency.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="tickColor">The tick Color.</param>
        /// <param name="orientation">The orientation.</param>
        public static void DrawTickLine(Graphics graphics, RectangleF drawRect, int tickFrequency, int minimum, int maximum, Color tickColor, Orientation orientation)
        {
            // Check input value
            if (maximum == minimum)
            {
                return;
            }

            // Create the Pen for drawing Ticks
            Pen pen = new Pen(tickColor, 1);
            float tickFrequencySize;

            // Calculate tick number
            int tickCount = (maximum - minimum) / tickFrequency;
            if ((maximum - minimum) % tickFrequency == 0)
            {
                tickCount -= 1;
            }

            if (orientation == Orientation.Horizontal)
            {
                // Calculate tick's setting
                tickFrequencySize = (drawRect.Width * tickFrequency) / (maximum - minimum);

                // Draw each tick
                for (var i = 0; i <= tickCount; i++)
                {
                    graphics.DrawLine(pen, drawRect.Left + (tickFrequencySize * i), drawRect.Top, drawRect.Left + (tickFrequencySize * i), drawRect.Bottom);
                }

                // Draw last tick at Maximum
                graphics.DrawLine(pen, drawRect.Right, drawRect.Top, drawRect.Right, drawRect.Bottom);
            }
            else
            {
                // Calculate tick's setting
                tickFrequencySize = (drawRect.Height * tickFrequency) / (maximum - minimum);

                // Draw each tick
                for (var i = 0; i <= tickCount; i++)
                {
                    graphics.DrawLine(pen, drawRect.Left, drawRect.Bottom - (tickFrequencySize * i), drawRect.Right, drawRect.Bottom - (tickFrequencySize * i));
                }

                // Draw last tick at Maximum
                graphics.DrawLine(pen, drawRect.Left, drawRect.Top, drawRect.Right, drawRect.Top);
            }
        }

        /// <summary>Draws the tick text.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="drawRect">The rectangle</param>
        /// <param name="tickFrequency">Tick frequency.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="foreColor">Fore color.</param>
        /// <param name="font">The font.</param>
        /// <param name="orientation">The orientation.</param>
        public static void DrawTickTextLine(Graphics graphics, RectangleF drawRect, int tickFrequency, int minimum, int maximum, Color foreColor, Font font, Orientation orientation)
        {
            // Check input value
            if (maximum == minimum)
            {
                return;
            }

            // Calculate tick number
            int tickCount = (maximum - minimum) / tickFrequency;
            if ((maximum - minimum) % tickFrequency == 0)
            {
                tickCount -= 1;
            }

            // Prepare for drawing Text
            StringFormat stringFormat = new StringFormat
                {
                    FormatFlags = StringFormatFlags.NoWrap,
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter,
                    HotkeyPrefix = HotkeyPrefix.Show
                };

            Brush brush = new SolidBrush(foreColor);
            string text;
            float tickFrequencySize;

            if (orientation == Orientation.Horizontal)
            {
                // Calculate tick's setting
                tickFrequencySize = (drawRect.Width * tickFrequency) / (maximum - minimum);

                // Draw each tick text
                for (var i = 0; i <= tickCount; i++)
                {
                    text = Convert.ToString(minimum + (tickFrequency * i), 10);
                    graphics.DrawString(text, font, brush, drawRect.Left + (tickFrequencySize * i), drawRect.Top + (drawRect.Height / 2), stringFormat);
                }

                // Draw last tick text at Maximum
                text = Convert.ToString(maximum, 10);
                graphics.DrawString(text, font, brush, drawRect.Right, drawRect.Top + (drawRect.Height / 2), stringFormat);
            }
            else
            {
                // Calculate tick's setting
                tickFrequencySize = (drawRect.Height * tickFrequency) / (maximum - minimum);

                // Draw each tick text
                for (var i = 0; i <= tickCount; i++)
                {
                    text = Convert.ToString(minimum + (tickFrequency * i), 10);
                    graphics.DrawString(text, font, brush, drawRect.Left + (drawRect.Width / 2), drawRect.Bottom - (tickFrequencySize * i), stringFormat);
                }

                // Draw last tick text at Maximum
                text = Convert.ToString(maximum, 10);
                graphics.DrawString(text, font, brush, drawRect.Left + (drawRect.Width / 2), drawRect.Top, stringFormat);
            }
        }

        /// <summary>Draw a triangle.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The button rectangle.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="state">The expanded toggle.</param>
        /// TODO: Add angle
        public static void DrawTriangle(Graphics graphics, Rectangle rectangle, Brush brush, bool state)
        {
            var points = new Point[3];
            if (state)
            {
                points[0].X = rectangle.X + (rectangle.Width / 2);
                points[0].Y = rectangle.Y;

                points[1].X = rectangle.X;
                points[1].Y = rectangle.Y + rectangle.Height;

                points[2].X = rectangle.X + rectangle.Width;
                points[2].Y = rectangle.Y + rectangle.Height;
            }
            else
            {
                points[0].X = rectangle.X;
                points[0].Y = rectangle.Y;

                points[1].X = rectangle.X + rectangle.Width;
                points[1].Y = rectangle.Y;

                points[2].X = rectangle.X + (rectangle.Width / 2);
                points[2].Y = rectangle.Y + rectangle.Height;
            }

            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.FillPolygon(brush, points);
        }

        /// <summary>Flip the size by orientation.</summary>
        /// <param name="orientation">The orientation.</param>
        /// <param name="size">Current size.</param>
        /// <returns>New size.</returns>
        public static Size FlipOrientationSize(Orientation orientation, Size size)
        {
            Size newSize = new Size(0, 0);

            // Resize
            if (orientation == Orientation.Vertical)
            {
                if (size.Width > size.Height)
                {
                    newSize = new Size(size.Height, size.Width);
                }
            }
            else
            {
                if (size.Width < size.Height)
                {
                    newSize = new Size(size.Height, size.Width);
                }
            }

            return newSize;
        }

        /// <summary>Gets the gradients points from the rectangle.</summary>
        /// <param name="rectangle">Rectangle points to set.</param>
        /// <returns>Gradient points.</returns>
        public static Point[] GetGradientPoints(Rectangle rectangle)
        {
            return new[] { new Point { X = rectangle.Width, Y = 0 }, new Point { X = rectangle.Width, Y = rectangle.Height } };
        }

        /// <summary>Checks whether the mouse is inside the bounds.</summary>
        /// <param name="mousePoint">Mouse location.</param>
        /// <param name="bounds">The rectangle.</param>
        /// <returns>Returns value.</returns>
        public static bool IsMouseInBounds(Point mousePoint, Rectangle bounds)
        {
            return bounds.Contains(mousePoint);
        }

        /// <summary>Measures the specified string when draw with the specified font.</summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to apply to the measured text.</param>
        /// <returns>Measured text size.</returns>
        public static Size MeasureText(string text, Font font)
        {
            return TextRenderer.MeasureText(text, font);
        }

        /// <summary>Measures the specified string when draw with the specified font.</summary>
        /// <param name="graphics">Graphics input.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to apply to the measured text.</param>
        /// <returns>Measured text size.</returns>
        public static Size MeasureText(Graphics graphics, string text, Font font)
        {
            int width = Convert.ToInt32(graphics.MeasureString(text, font).Width);
            int height = Convert.ToInt32(graphics.MeasureString(text, font).Height);
            Size textSize = new Size(width, height);

            return textSize;
        }

        /// <summary>Set's the container controls BackColor.</summary>
        /// <param name="control">Current control.</param>
        /// <param name="backgroundColor">Container background color.</param>
        /// <param name="onControlRemoved">Control removed?</param>
        public static void SetControlBackColor(Control control, Color backgroundColor, bool onControlRemoved)
        {
            Color backColor;

            if (onControlRemoved)
            {
                backColor = Color.Transparent;

                // Bug: The Control doesn't support transparent background
                if (control is VisualProgressIndicator)
                {
                    backColor = SystemColors.Control;
                }
            }
            else
            {
                backColor = backgroundColor;
            }

            control.BackColor = backColor;
        }

        /// <summary>Checks if the text is larger than the rectangle.</summary>
        /// <param name="text">The text.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>Returns result.</returns>
        public static bool TextLargerThanRectangle(Size text, Rectangle rectangle)
        {
            return text.Height > rectangle.Size.Height;
        }

        /// <summary>Sets the graphics using picture box size mode.</summary>
        /// <param name="pictureBoxSizeMode">The picture box size mode.</param>
        /// <returns>Graphics controller.</returns>
        public Graphics SetPictureBoxSizeMode(PictureBoxSizeMode pictureBoxSizeMode)
        {
            Bitmap drawArea = new Bitmap(new PictureBox { SizeMode = pictureBoxSizeMode }.Size.Width, new PictureBox { SizeMode = pictureBoxSizeMode }.Size.Height);
            new PictureBox { SizeMode = pictureBoxSizeMode }.Image = drawArea;
            return Graphics.FromImage(drawArea);
        }

        /// <summary>Draw background image.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="image">The image.</param>
        /// <param name="centered">Center image.</param>
        internal static void DrawBackgroundImage(Graphics graphics, Rectangle rectangle, Bitmap image, bool centered = true)
        {
            if (image != null)
            {
                Point imageLocation = centered ? new Point((rectangle.X + (rectangle.Width / 2)) - (image.Size.Width / 2), (rectangle.Y + (rectangle.Height / 2)) - (image.Size.Height / 2)) : new Point(0, 0);
                graphics.DrawImage(image, new Rectangle(imageLocation, image.Size));
            }
        }

        /// <summary>Fills the background.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="graphicsPath">The graphics path.</param>
        /// <param name="gradientBrush">The gradient brush.</param>
        internal static void FillBackground(Graphics graphics, GraphicsPath graphicsPath, Brush gradientBrush)
        {
            graphics.FillPath(gradientBrush, graphicsPath);
        }

        /// <summary>Gets the control brush.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="enabled">Enabled state.</param>
        /// <param name="mouseState">Mouse state.</param>
        /// <param name="controlStates">The gradient color states.</param>
        /// <param name="rectangle">The rectangle</param>
        /// <returns>Control brush state.</returns>
        internal static LinearGradientBrush GetControlBrush(Graphics graphics, bool enabled, MouseStates mouseState, Gradient[] controlStates, Rectangle rectangle)
        {
            Gradient tempGradient;
            if (enabled)
            {
                switch (mouseState)
                {
                    case MouseStates.Normal:
                        {
                            tempGradient = controlStates[0];
                            break;
                        }

                    case MouseStates.Hover:
                        {
                            tempGradient = controlStates[1];
                            break;
                        }

                    case MouseStates.Down:
                        {
                            tempGradient = controlStates[2];
                            break;
                        }

                    default:
                        {
                            tempGradient = controlStates[0];
                            break;
                        }
                }
            }
            else
            {
                tempGradient = controlStates[3];
            }

            var gradientPoints = GetGradientPoints(rectangle);
            return Gradient.CreateGradientBrush(tempGradient.Colors, gradientPoints, tempGradient.Angle, tempGradient.Positions);
        }

        /// <summary>Draws the rounded rectangle from a rectangle shape.</summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="curve">The curve.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        internal static GraphicsPath RoundForm(Rectangle rectangle, int curve)
        {
            GraphicsPath _graphicsPath = new GraphicsPath();
            _graphicsPath.StartFigure();
            _graphicsPath.AddArc(rectangle.X, rectangle.Y, curve, curve, 180F, 90F);
            _graphicsPath.AddLine(curve, rectangle.Y, rectangle.Width - curve, 90F);
            _graphicsPath.AddArc(rectangle.Width - curve, rectangle.Y, curve, curve, 270F, 90F);
            _graphicsPath.AddLine(rectangle.Width, curve, rectangle.Width, rectangle.Height - curve);
            _graphicsPath.AddArc(rectangle.Width - curve, rectangle.Height - curve, curve, curve, 90F, 90F);
            _graphicsPath.AddLine(rectangle.Width - curve, rectangle.Height, curve, rectangle.Height);
            _graphicsPath.AddArc(rectangle.X, rectangle.Height - curve, curve, curve, 90F, 90F);
            return _graphicsPath;
        }

        #endregion
    }
}