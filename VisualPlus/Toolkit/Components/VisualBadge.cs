namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Localization;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Component))]
    [Description("The VisualPlus badge component enables controls to have a badge with text displayed.")]
    public class VisualBadge : Component
    {
        #region Variables

        private BadgeLabel _badgeLabel;
        private Control _control;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualBadge" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        public VisualBadge(Control control) : this()
        {
            _control = control;

            if (Enabled)
            {
                Attach();
            }

            ConstructVisualBadge(_badgeLabel.Text, _badgeLabel.Font, _badgeLabel.ForeColor, _badgeLabel.Background, new Rectangle(_badgeLabel.Location, _badgeLabel.Size), new Shape());
        }

        /// <summary>Initializes a new instance of the <see cref="VisualBadge" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="text">The text.</param>
        public VisualBadge(Control control, string text) : this()
        {
            _control = control;

            if (Enabled)
            {
                Attach();
            }

            ConstructVisualBadge(text, _badgeLabel.Font, _badgeLabel.ForeColor, _badgeLabel.Background, new Rectangle(_badgeLabel.Location, _badgeLabel.Size), new Shape());
        }

        /// <summary>Initializes a new instance of the <see cref="VisualBadge" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        public VisualBadge(Control control, string text, Font font) : this()
        {
            _control = control;

            if (Enabled)
            {
                Attach();
            }

            ConstructVisualBadge(text, font, _badgeLabel.ForeColor, _badgeLabel.Background, new Rectangle(_badgeLabel.Location, _badgeLabel.Size), new Shape());
        }

        /// <summary>Initializes a new instance of the <see cref="VisualBadge" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="foreColor">The fore Color.</param>
        public VisualBadge(Control control, string text, Font font, Color foreColor) : this()
        {
            _control = control;

            if (Enabled)
            {
                Attach();
            }

            ConstructVisualBadge(text, font, foreColor, _badgeLabel.Background, new Rectangle(_badgeLabel.Location, _badgeLabel.Size), new Shape());
        }

        /// <summary>Initializes a new instance of the <see cref="VisualBadge" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="foreColor">The fore Color.</param>
        /// <param name="background">The background.</param>
        public VisualBadge(Control control, string text, Font font, Color foreColor, Color background) : this()
        {
            _control = control;

            if (Enabled)
            {
                Attach();
            }

            ConstructVisualBadge(text, font, foreColor, background, new Rectangle(_badgeLabel.Location, _badgeLabel.Size), new Shape());
        }

        /// <summary>Initializes a new instance of the <see cref="VisualBadge" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="foreColor">The fore Color.</param>
        /// <param name="background">The background.</param>
        /// <param name="rectangle">The rectangle.</param>
        public VisualBadge(Control control, string text, Font font, Color foreColor, Color background, Rectangle rectangle) : this()
        {
            _control = control;

            if (Enabled)
            {
                Attach();
            }

            ConstructVisualBadge(text, font, foreColor, background, rectangle, new Shape());
        }

        /// <summary>Initializes a new instance of the <see cref="VisualBadge" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="foreColor">The fore Color.</param>
        /// <param name="background">The background.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="shape">The shape.</param>
        public VisualBadge(Control control, string text, Font font, Color foreColor, Color background, Rectangle rectangle, Shape shape) : this()
        {
            _control = control;

            if (Enabled)
            {
                Attach();
            }

            ConstructVisualBadge(text, font, foreColor, background, rectangle, shape);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualBadge" /> class.</summary>
        /// <param name="container">The container.</param>
        public VisualBadge(IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>Prevents a default instance of the <see cref="VisualBadge" /> class from being created.</summary>
        private VisualBadge()
        {
            _badgeLabel = new BadgeLabel();
        }

        #endregion

        #region Properties

        [Description(PropertyDescription.Color)]
        [Category(PropertyCategory.Appearance)]
        public Color Background
        {
            get
            {
                return _badgeLabel.Background;
            }

            set
            {
                _badgeLabel.Background = value;
                _badgeLabel.Invalidate();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Behavior)]
        [Description("The control to attach this component.")]
        public Control Control
        {
            get
            {
                return _control;
            }

            set
            {
                _control = value;

                if (_control == null)
                {
                    return;
                }

                if (Enabled)
                {
                    Attach();
                }
                else
                {
                    Remove();
                }
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Toggle)]
        public bool Enabled
        {
            get
            {
                return _badgeLabel.Visible;
            }

            set
            {
                _badgeLabel.Visible = value;

                if (_control == null)
                {
                    return;
                }

                if (Enabled)
                {
                    Attach();
                }
                else
                {
                    Remove();
                }
            }
        }

        [Description(PropertyDescription.Font)]
        [Category(PropertyCategory.Appearance)]
        public Font Font
        {
            get
            {
                return _badgeLabel.Font;
            }

            set
            {
                _badgeLabel.Font = value;
                _badgeLabel.Invalidate();
            }
        }

        [Description(PropertyDescription.Color)]
        [Category(PropertyCategory.Appearance)]
        public Color ForeColor
        {
            get
            {
                return _badgeLabel.ForeColor;
            }

            set
            {
                _badgeLabel.ForeColor = value;
                _badgeLabel.Invalidate();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Color)]
        [Category(PropertyCategory.Appearance)]
        public Point Location
        {
            get
            {
                return _badgeLabel.Location;
            }

            set
            {
                _badgeLabel.Location = value;
                _badgeLabel.Invalidate();
            }
        }

        [TypeConverter(typeof(ShapeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(PropertyCategory.Appearance)]
        public Shape Shape
        {
            get
            {
                return _badgeLabel.Shape;
            }

            set
            {
                _badgeLabel.Shape = value;
                _badgeLabel.Invalidate();
            }
        }

        [Description(PropertyDescription.Size)]
        [Category(PropertyCategory.Appearance)]
        public Size Size
        {
            get
            {
                return _badgeLabel.Size;
            }

            set
            {
                _badgeLabel.Size = value;
                _badgeLabel.Invalidate();
            }
        }

        [Description(PropertyDescription.Text)]
        [Category(PropertyCategory.Appearance)]
        public string Text
        {
            get
            {
                return _badgeLabel.Text;
            }

            set
            {
                _badgeLabel.Text = value;
                _badgeLabel.Invalidate();
            }
        }

        #endregion

        #region Events

        /// <summary>Sets the click action for the badge label.</summary>
        /// <param name="action">The click action to set.</param>
        public void SetClickAction(Action<Control> action)
        {
            if (_badgeLabel != null)
            {
                _badgeLabel.ClickEvent = action;
            }
        }

        /// <summary>Attach the badge label to the control.</summary>
        private void Attach()
        {
            _control.Controls.Add(_badgeLabel);
        }

        /// <summary>Creates the visual badge.</summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="foreColor">The fore Color.</param>
        /// <param name="background">The background.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="shape">The shape.</param>
        private void ConstructVisualBadge(string text, Font font, Color foreColor, Color background, Rectangle rectangle, Shape shape)
        {
            Text = text;
            Font = font;
            ForeColor = foreColor;
            Background = background;
            Size = rectangle.Size;
            Location = rectangle.Location;
            Shape = shape;
        }

        /// <summary>Remove the badge label from the control.</summary>
        private void Remove()
        {
            if (_badgeLabel != null)
            {
                _control.Controls.Remove(_badgeLabel);
            }
        }

        #endregion
    }
}