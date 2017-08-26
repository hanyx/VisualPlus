#region Namespace

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using VisualPlus.Enumerators;
using VisualPlus.Structure;

#endregion

namespace VisualPlus.Renders
{
    public sealed class VisualBorderRenderer
    {
        #region Events

        /// <summary>Draws a border around the rectangle.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="borderRectangle">The rectangle.</param>
        /// <param name="borderThickness">The thickness.</param>
        /// <param name="color">The color.</param>
        public static void DrawBorder(Graphics graphics, Rectangle borderRectangle, float borderThickness, Color color)
        {
            using (GraphicsPath borderPath = new GraphicsPath())
            {
                borderPath.AddRectangle(borderRectangle);
                DrawBorder(graphics, borderPath, borderThickness, color);
            }
        }

        /// <summary>Draws a border around the path.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="borderPath">The path.</param>
        /// <param name="borderThickness">The thickness.</param>
        /// <param name="color">The color.</param>
        public static void DrawBorder(Graphics graphics, GraphicsPath borderPath, float borderThickness, Color color)
        {
            Pen borderPen = new Pen(color, borderThickness);
            graphics.DrawPath(borderPen, borderPath);
        }

        /// <summary>Draws a border around the path.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="borderPath">The path.</param>
        /// <param name="shape">The shape type.</param>
        public static void DrawBorder(Graphics graphics, GraphicsPath borderPath, Shape shape)
        {
            Pen borderPen = new Pen(shape.Color, shape.Thickness);
            graphics.DrawPath(borderPen, borderPath);
        }

        /// <summary>Draws the border style.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="border">The border type.</param>
        /// <param name="mouseState">The mouse state.</param>
        /// <param name="borderPath">The border path.</param>
        public static void DrawBorderStyle(Graphics graphics, Border border, MouseStates mouseState, GraphicsPath borderPath)
        {
            if (border.Visible)
            {
                if ((mouseState == MouseStates.Hover) && border.HoverVisible)
                {
                    DrawBorder(graphics, borderPath, border.Thickness, border.HoverColor);
                }
                else if ((mouseState == MouseStates.Down) && border.HoverVisible)
                {
                    // TODO: Create 'Down' border color.
                    DrawBorder(graphics, borderPath, border.Thickness, border.HoverVisible ? border.HoverColor : border.Color);
                }
                else
                {
                    DrawBorder(graphics, borderPath, border.Thickness, border.Color);
                }
            }
        }

        /// <summary>Draws the border style.</summary>
        /// <param name="graphics">Graphics controller.</param>
        /// <param name="border">The border type.</param>
        /// <param name="mouseState">The mouse state.</param>
        /// <param name="borderRectangle">The border Rectangle.</param>
        public static void DrawBorderStyle(Graphics graphics, Border border, MouseStates mouseState, Rectangle borderRectangle)
        {
            GraphicsPath borderPath = new GraphicsPath();
            borderPath.AddRectangle(borderRectangle);

            DrawBorderStyle(graphics, border, mouseState, borderPath);
        }

        /// <summary>Gets the distance from the border.</summary>
        /// <param name="shape">The shape of the container control.</param>
        /// <returns>The internal control distance.</returns>
        public static int GetBorderDistance(Shape shape)
        {
            return (shape.Rounding / 2) + shape.Thickness + 1;
        }

        /// <summary>Get the border shape.</summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="border">The border.</param>
        /// <returns>Border graphics path.</returns>
        public static GraphicsPath GetBorderShape(Rectangle rectangle, Border border)
        {
            return GetBorderShape(rectangle, border.Type, border.Rounding);
        }

        /// <summary>Get the border shape.</summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="borderType">The shape.</param>
        /// <param name="borderRounding">The rounding.</param>
        /// <returns>The <see cref="GraphicsPath" />.</returns>
        public static GraphicsPath GetBorderShape(Rectangle rectangle, ShapeType borderType, int borderRounding)
        {
            Rectangle borderRectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);

            GraphicsPath borderShape = new GraphicsPath();

            switch (borderType)
            {
                case ShapeType.Rectangle:
                {
                    borderShape.AddRectangle(borderRectangle);
                    break;
                }

                case ShapeType.Rounded:
                {
                    borderShape.AddArc(borderRectangle.X, borderRectangle.Y, borderRounding, borderRounding, 180.0F, 90.0F);
                    borderShape.AddArc(borderRectangle.Right - borderRounding, borderRectangle.Y, borderRounding, borderRounding, 270.0F, 90.0F);
                    borderShape.AddArc(borderRectangle.Right - borderRounding, borderRectangle.Bottom - borderRounding, borderRounding, borderRounding, 0.0F, 90.0F);
                    borderShape.AddArc(borderRectangle.X, borderRectangle.Bottom - borderRounding, borderRounding, borderRounding, 90.0F, 90.0F);
                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(borderType), borderType, null);
                }
            }

            borderShape.CloseAllFigures();
            return borderShape;
        }

        #endregion
    }
}