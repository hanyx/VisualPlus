namespace VisualPlus.Renders
{
    #region Namespace

    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    using VisualPlus.Enumerators;
    using VisualPlus.Managers;
    using VisualPlus.Structure;

    #endregion

    public sealed class VisualBorderRenderer
    {
        #region Events

        /// <summary>Gets the distance from the border.</summary>
        /// <param name="shape">The shape of the container control.</param>
        /// <returns>The <see cref="int" />.</returns>
        public static int CalculateBorderCurve(Shape shape)
        {
            return (shape.Rounding / 2) + shape.Thickness + 1;
        }

        /// <summary>Creates a border type path.</summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        public static GraphicsPath CreateBorderTypePath(Rectangle rectangle, Shape shape)
        {
            return CreateBorderTypePath(rectangle, shape.Rounding, shape.Thickness, shape.Type);
        }

        /// <summary>Creates a border type path.</summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="rounding">The rounding.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="type">The shape.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        public static GraphicsPath CreateBorderTypePath(Rectangle rectangle, int rounding, int thickness, ShapeType type)
        {
            Rectangle _borderRectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - thickness, rectangle.Height - thickness);
            GraphicsPath _borderShape = new GraphicsPath();

            switch (type)
            {
                case ShapeType.Rectangle:
                    {
                        _borderShape.AddRectangle(_borderRectangle);
                        break;
                    }

                case ShapeType.Rounded:
                    {
                        _borderShape.AddArc(rectangle.X, rectangle.Y, rounding, rounding, 180.0F, 90.0F);
                        _borderShape.AddArc(rectangle.Right - rounding, rectangle.Y, rounding, rounding, 270.0F, 90.0F);
                        _borderShape.AddArc(rectangle.Right - rounding, rectangle.Bottom - rounding, rounding, rounding, 0.0F, 90.0F);
                        _borderShape.AddArc(rectangle.X, rectangle.Bottom - rounding, rounding, rounding, 90.0F, 90.0F);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                    }
            }

            _borderShape.CloseAllFigures();
            return _borderShape;
        }

        /// <summary>Draws a border around the rectangle, with the specified thickness.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="color">The color.</param>
        /// <param name="thickness">The thickness.</param>
        public static void DrawBorder(Graphics graphics, Rectangle rectangle, Color color, float thickness)
        {
            GraphicsPath _borderGraphicsPath = new GraphicsPath();
            _borderGraphicsPath.AddRectangle(rectangle);
            Pen _borderPen = new Pen(color, thickness);
            graphics.DrawPath(_borderPen, _borderGraphicsPath);
        }

        /// <summary>Draws a border around the custom graphics path, with the specified thickness.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="customPath">The custom Path.</param>
        /// <param name="color">The color.</param>
        /// <param name="thickness">The thickness.</param>
        public static void DrawBorder(Graphics graphics, GraphicsPath customPath, Color color, float thickness)
        {
            Pen _borderPen = new Pen(color, thickness);
            graphics.DrawPath(_borderPen, customPath);
        }

        /// <summary>Draws a border around the rounded rectangle, with the specified rounding and thickness.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="color">The color.</param>
        /// <param name="rounding">The amount of rounding.</param>
        /// <param name="thickness">The thickness.</param>
        public static void DrawBorder(Graphics graphics, Rectangle rectangle, Color color, int rounding, float thickness)
        {
            GraphicsPath _borderGraphicsPath = GraphicsManager.DrawRoundedRectangle(rectangle, rounding);
            Pen _borderPen = new Pen(color, thickness);
            graphics.DrawPath(_borderPen, _borderGraphicsPath);
        }

        /// <summary>Draws a border with the specified shape settings.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="color">The color.</param>
        /// <param name="rounding">The rounding.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="shape">The shape.</param>
        public static void DrawBorder(Graphics graphics, Rectangle rectangle, Color color, int rounding, float thickness, ShapeType shape)
        {
            switch (shape)
            {
                case ShapeType.Rectangle:
                    {
                        DrawBorder(graphics, rectangle, color, thickness);
                        break;
                    }

                case ShapeType.Rounded:
                    {
                        DrawBorder(graphics, rectangle, color, rounding, thickness);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(shape), shape, null);
                    }
            }
        }

        /// <summary>Draws a border with the specified shape.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="shape">The shape.</param>
        public static void DrawBorder(Graphics graphics, Rectangle rectangle, Shape shape)
        {
            DrawBorder(graphics, rectangle, shape.Color, shape.Rounding, shape.Thickness, shape.Type);
        }

        /// <summary>Draws a border around the rectangle, with the specified mouse state.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="border">The border type.</param>
        /// <param name="mouseState">The mouse state.</param>
        /// <param name="rectangle">The rectangle.</param>
        public static void DrawBorder(Graphics graphics, Border border, MouseStates mouseState, Rectangle rectangle)
        {
            if (!border.Visible)
            {
                return;
            }

            switch (mouseState)
            {
                case MouseStates.Normal:
                    {
                        DrawBorder(graphics, rectangle, border.Color, border.Rounding, border.Thickness, border.Type);
                        break;
                    }

                case MouseStates.Hover:
                    {
                        DrawBorder(graphics, rectangle, border.HoverVisible ? border.HoverColor : border.Color, border.Rounding, border.Thickness, border.Type);
                        break;
                    }

                case MouseStates.Down:
                    {
                        DrawBorder(graphics, rectangle, border.HoverVisible ? border.HoverColor : border.Color, border.Rounding, border.Thickness, border.Type);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(mouseState), mouseState, null);
                    }
            }
        }

        /// <summary>Draws a border around the custom path, with the specified mouse state.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="border">The border type.</param>
        /// <param name="customPath">The custom Path.</param>
        /// <param name="mouseState">The mouse state.</param>
        public static void DrawBorderStyle(Graphics graphics, Border border, GraphicsPath customPath, MouseStates mouseState)
        {
            if (!border.Visible)
            {
                return;
            }

            switch (mouseState)
            {
                case MouseStates.Normal:
                    {
                        DrawBorder(graphics, customPath, border.Color, border.Thickness);
                        break;
                    }

                case MouseStates.Hover:
                    {
                        DrawBorder(graphics, customPath, border.HoverVisible ? border.HoverColor : border.Color, border.Thickness);
                        break;
                    }

                case MouseStates.Down:
                    {
                        DrawBorder(graphics, customPath, border.HoverVisible ? border.HoverColor : border.Color, border.Thickness);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(mouseState), mouseState, null);
                    }
            }
        }

        #endregion
    }
}