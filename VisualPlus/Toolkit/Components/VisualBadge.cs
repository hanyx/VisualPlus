namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Localization.Category;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

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

        [Category(Property.Appearance)]
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
        [Category(Property.Behavior)]
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
        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.Description.Common.Toggle)]
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

        [Category(Property.Appearance)]
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

        [Category(Property.Appearance)]
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
        [Category(Property.Appearance)]
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