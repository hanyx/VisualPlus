namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using VisualPlus.Localization.Category;
    using VisualPlus.Renders;
    using VisualPlus.Structure;

    #endregion

    [ToolboxItem(false)]
    [Description("The VisualPlus badge component enables controls to have a badge with text displayed.")]
    public class Badge : Label
    {
        #region Variables

        public Action<Control> ClickEvent;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Badge" /> class.</summary>
        public Badge()
        {
            Background = Color.Red;
            Shape = new Shape();
        }

        #endregion

        #region Properties

        [DefaultValue(typeof(Color), "Red")]
        [Category(Property.Appearance)]
        public Color Background { get; set; }

        [TypeConverter(typeof(ShapeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Appearance)]
        public Shape Shape { get; set; }

        #endregion

        #region Events

        /// <summary>Add a badge control to the control, with the specified text and position.</summary>
        /// <param name="control">The control to draw the badge on.</param>
        /// <param name="text">The text for the badge to display.</param>
        /// <returns>Badge control added to control.</returns>
        public static bool Add(Control control, string text)
        {
            if (controlsList.Contains(control))
            {
                return false;
            }

            ConstructBadge(control, text, DefaultFont, DefaultBackColor, DefaultForeColor, new Point(), new Shape());
            return true;
        }

        /// <summary>Add a badge control to the control, with the specified text and position.</summary>
        /// <param name="control">The control to draw the badge on.</param>
        /// <param name="text">The text for the badge to display.</param>
        /// <param name="position">The position of the badge.</param>
        /// <returns>Badge control added to control.</returns>
        public static bool Add(Control control, string text, Point position)
        {
            if (controlsList.Contains(control))
            {
                return false;
            }

            ConstructBadge(control, text, DefaultFont, DefaultBackColor, DefaultForeColor, position, new Shape());
            return true;
        }

        /// <summary>Add a badge control to the control, with the specified text font and position.</summary>
        /// <param name="control">The control to draw the badge on.</param>
        /// <param name="text">The text for the badge to display.</param>
        /// <param name="font">The font to use for the text.</param>
        /// <param name="position">The position of the badge.</param>
        /// <returns>Badge control added to control.</returns>
        public static bool Add(Control control, string text, Font font, Point position)
        {
            if (controlsList.Contains(control))
            {
                return false;
            }

            ConstructBadge(control, text, font, DefaultBackColor, DefaultForeColor, position, new Shape());
            return true;
        }

        /// <summary>Add a badge control to the control, with the specified text, style and position.</summary>
        /// <param name="control">The control to draw the badge on.</param>
        /// <param name="text">The text for the badge to display.</param>
        /// <param name="font">The font to use for the text.</param>
        /// <param name="backColor">The back Color of the badge.</param>
        /// <param name="foreColor">The fore Color of the badge.</param>
        /// <param name="position">The position of the badge.</param>
        /// <returns>Badge control added to control.</returns>
        public static bool Add(Control control, string text, Font font, Color backColor, Color foreColor, Point position)
        {
            if (controlsList.Contains(control))
            {
                return false;
            }

            ConstructBadge(control, text, font, backColor, foreColor, position, new Shape());
            return true;
        }

        /// <summary>Add a badge control to the control, with the specified settings.</summary>
        /// <param name="control">The control to draw the badge on.</param>
        /// <param name="text">The text for the badge to display.</param>
        /// <param name="font">The font to use for the text.</param>
        /// <param name="backColor">The back Color of the badge.</param>
        /// <param name="foreColor">The fore Color of the badge.</param>
        /// <param name="position">The position of the badge.</param>
        /// <param name="shape">The shape of the badge.</param>
        /// <returns>Badge control added to control.</returns>
        public static bool Add(Control control, string text, Font font, Color backColor, Color foreColor, Point position, Shape shape)
        {
            if (controlsList.Contains(control))
            {
                return false;
            }

            ConstructBadge(control, text, font, backColor, foreColor, position, shape);
            return true;
        }

        /// <summary>Draws the badge.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="position">The position to draw.</param>
        /// <param name="backColor">The back color.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="foreColor">The fore color.</param>
        /// <param name="shape">The shape type.</param>
        public static void Draw(Graphics graphics, Point position, Color backColor, string text, Font font, Color foreColor, Shape shape)
        {
            Size textSize = GDI.MeasureText(graphics, text, font);
            Rectangle shapeRectangle = new Rectangle(position, new Size(textSize.Width + 1, textSize.Height));
            Point textPoint = new Point(shapeRectangle.X, shapeRectangle.Y);
            GraphicsPath shapePath = VisualBorderRenderer.GetBorderShape(shapeRectangle, shape.Type, shape.Rounding);

            graphics.FillPath(new SolidBrush(backColor), shapePath);
            VisualBorderRenderer.DrawBorder(graphics, shapePath, shape);
            graphics.DrawString(text, font, new SolidBrush(foreColor), textPoint);
        }

        /// <summary>Gets the back color from the badge.</summary>
        /// <param name="control">The control.</param>
        /// <returns>Returns back color.</returns>
        public static Color GetBackColor(Control control)
        {
            Badge badge = GetBadge(control);
            return badge?.Background ?? DefaultBackColor;
        }

        /// <summary>Gets the badge from the control.</summary>
        /// <param name="control">The control.</param>
        /// <returns>The badge.</returns>
        public static Badge GetBadge(Control control)
        {
            for (var i = 0; i < control.Controls.Count; i++)
            {
                if (control.Controls[i] is Badge)
                {
                    return control.Controls[i] as Badge;
                }
            }

            return null;
        }

        /// <summary>Gets the text from the badge.</summary>
        /// <param name="control">The control.</param>
        /// <returns>Returns text.</returns>
        public static Font GetFont(Control control)
        {
            Badge badge = GetBadge(control);
            return badge?.Font;
        }

        /// <summary>Gets the fore color from the badge.</summary>
        /// <param name="control">The control.</param>
        /// <returns>Returns fore color.</returns>
        public static Color GetForeColor(Control control)
        {
            Badge badge = GetBadge(control);
            return badge?.ForeColor ?? DefaultForeColor;
        }

        /// <summary>Gets the position for the badge.</summary>
        /// <param name="control">The control.</param>
        /// <returns>Returns position.</returns>
        public static Point GetPosition(Control control)
        {
            Badge badge = GetBadge(control);
            return badge?.Location ?? new Point(0, 0);
        }

        /// <summary>Gets the shape from the badge.</summary>
        /// <param name="control">The control.</param>
        /// <returns>Returns shape.</returns>
        public static Shape GetShape(Control control)
        {
            Badge badge = GetBadge(control);
            return badge?.Shape;
        }

        /// <summary>Gets the size from the badge.</summary>
        /// <param name="control">The control.</param>
        /// <returns>Returns size.</returns>
        public static Size GetSize(Control control)
        {
            Badge badge = GetBadge(control);
            return badge?.Size ?? new Size(0, 0);
        }

        /// <summary>Gets the text from the badge.</summary>
        /// <param name="control">The control.</param>
        /// <returns>Returns text.</returns>
        public static string GetText(Control control)
        {
            Badge badge = GetBadge(control);
            return badge != null ? badge.Text : string.Empty;
        }

        /// <summary>Remove the badge from the control.</summary>
        /// <param name="control">The control to remove the badge from.</param>
        /// <returns>Removed badge from control.</returns>
        public static bool Remove(Control control)
        {
            Badge _badge = GetBadge(control);
            if (_badge != null)
            {
                control.Controls.Remove(_badge);
                controlsList.Remove(control);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Sets the back color for the badge.</summary>
        /// <param name="control">The control.</param>
        /// <param name="color">The color.</param>
        public static void SetBackColor(Control control, Color color)
        {
            Badge badge = GetBadge(control);
            if (badge != null)
            {
                badge.Background = color;
            }
        }

        /// <summary>Sets the click action for the badge.</summary>
        /// <param name="control">The control.</param>
        /// <param name="action">The action.</param>
        public static void SetClickAction(Control control, Action<Control> action)
        {
            Badge _badge = GetBadge(control);
            if (_badge != null)
            {
                _badge.ClickEvent = action;
            }
        }

        /// <summary>Sets the font for the badge.</summary>
        /// <param name="control">The control.</param>
        /// <param name="font">The font.</param>
        public static void SetFont(Control control, Font font)
        {
            Badge badge = GetBadge(control);
            if (badge != null)
            {
                badge.Font = font;
            }
        }

        /// <summary>Sets the fore color for the badge.</summary>
        /// <param name="control">The control.</param>
        /// <param name="color">The color.</param>
        public static void SetForeColor(Control control, Color color)
        {
            Badge badge = GetBadge(control);
            if (badge != null)
            {
                badge.ForeColor = color;
            }
        }

        /// <summary>Sets the position for the badge.</summary>
        /// <param name="control">The control.</param>
        /// <param name="position">The position.</param>
        public static void SetPosition(Control control, Point position)
        {
            Badge badge = GetBadge(control);
            if (badge != null)
            {
                badge.Location = position;
            }
        }

        /// <summary>Sets the shape for the badge.</summary>
        /// <param name="control">The control.</param>
        /// <param name="shape">The shape.</param>
        public static void SetShape(Control control, Shape shape)
        {
            Badge badge = GetBadge(control);
            if (badge != null)
            {
                badge.Shape = shape;
            }
        }

        /// <summary>Sets the text for the badge.</summary>
        /// <param name="control">The control.</param>
        /// <param name="text">The text.</param>
        public static void SetText(Control control, string text)
        {
            Badge _badge = GetBadge(control);
            if (_badge != null)
            {
                _badge.Text = text;
                _badge.Size = ConfigureSize(text, _badge.Font);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            ClickEvent(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics, new Point(0, 0), Background, Text, Font, ForeColor, Shape);
        }

        /// <summary>Attach the badge control.</summary>
        /// <param name="control">The control.</param>
        /// <param name="badge">The badge control.</param>
        private static void Attach(Control control, Badge badge)
        {
            controlsList.Add(control);
            control.Controls.Add(badge);
        }

        /// <summary>Configures the size of the text field.</summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <returns>The text field size.</returns>
        private static Size ConfigureSize(string text, Font font)
        {
            Size textSize = GDI.MeasureText(text, font);
            textSize = new Size(textSize.Width + 1, textSize.Height + 1);
            return textSize;
        }

        /// <summary>Construct a badge control for the control, with the specified settings.</summary>
        /// <param name="control">The control to draw the badge on.</param>
        /// <param name="text">The text for the badge to display.</param>
        /// <param name="font">The font to use for the text.</param>
        /// <param name="backColor">The back Color of the badge.</param>
        /// <param name="foreColor">The fore Color of the badge.</param>
        /// <param name="position">The position of the badge.</param>
        /// <param name="shape">The shape of the badge.</param>
        private static void ConstructBadge(Control control, string text, Font font, Color backColor, Color foreColor, Point position, Shape shape)
        {
            Size textSize = ConfigureSize(text, font);

            Badge _badge = new Badge
                {
                    AutoSize = false,
                    Font = font,
                    Text = text,
                    BackColor = Color.Transparent,
                    Background = backColor,
                    ForeColor = foreColor,
                    Location = position,
                    Shape = shape,
                    Size = textSize
                };

            Attach(control, _badge);
            SetPosition(control, position);
        }

        private static List<Control> controlsList = new List<Control>();

        #endregion
    }
}