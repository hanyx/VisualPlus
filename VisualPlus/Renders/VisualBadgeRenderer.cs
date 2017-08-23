namespace VisualPlus.Renders
{
    #region Namespace

    using System.Drawing;
    using System.Drawing.Drawing2D;

    using VisualPlus.Structure;

    #endregion

    public sealed class VisualBadgeRenderer
    {
        #region Events

        /// <summary>Draws the badge.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="backColor">The back color.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="foreColor">The fore color.</param>
        /// <param name="shape">The shape type.</param>
        /// <param name="textLocation">The _text Location.</param>
        public static void DrawBadge(Graphics graphics, Rectangle rectangle, Color backColor, string text, Font font, Color foreColor, Shape shape, Point textLocation)
        {
            GraphicsPath _badgePath = VisualBorderRenderer.GetBorderShape(rectangle, shape.Type, shape.Rounding);
            graphics.FillPath(new SolidBrush(backColor), _badgePath);
            VisualBorderRenderer.DrawBorder(graphics, _badgePath, shape);
            graphics.DrawString(text, font, new SolidBrush(foreColor), textLocation);
        }

        #endregion
    }
}