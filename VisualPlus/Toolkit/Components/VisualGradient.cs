namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Localization.Category;

    #endregion

    [Description("The VisualPlus gradient component can be used to apply gradient backgrounds on controls.")]
    public class VisualGradient : Component
    {
        #region Variables

        private Color _bottomLeft;
        private Color _bottomRight;
        private Control _control;
        private int _quality;
        private Color _topLeft;
        private Color _topRight;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualGradient" /> class.</summary>
        /// <param name="container">The container.</param>
        public VisualGradient(IContainer container) : this()
        {
            container.Add(this);
            ConstructVisualGradient(_quality, _bottomLeft, _bottomRight, _topLeft, _topRight);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualGradient" /> class.</summary>
        /// <param name="control">The control.</param>
        public VisualGradient(Control control) : this()
        {
            _control = control;
            ConstructVisualGradient(_quality, _bottomLeft, _bottomRight, _topLeft, _topRight);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualGradient" /> class.</summary>
        /// <param name="control">The control.</param>
        /// <param name="quality">The quality.</param>
        public VisualGradient(Control control, int quality) : this()
        {
            _control = control;
            ConstructVisualGradient(quality, _bottomLeft, _bottomRight, _topLeft, _topRight);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualGradient" /> class.</summary>
        /// <param name="control">The control.</param>
        /// <param name="bottomLeft">The bottom Left.</param>
        /// <param name="bottomRight">The bottom Right.</param>
        /// <param name="topLeft">The top Left.</param>
        /// <param name="topRight">The top Right.</param>
        public VisualGradient(Control control, Color bottomLeft, Color bottomRight, Color topLeft, Color topRight) : this()
        {
            _control = control;
            ConstructVisualGradient(_quality, bottomLeft, bottomRight, topLeft, topRight);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualGradient" /> class.</summary>
        /// <param name="control">The control.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="bottomLeft">The bottom Left.</param>
        /// <param name="bottomRight">The bottom Right.</param>
        /// <param name="topLeft">The top Left.</param>
        /// <param name="topRight">The top Right.</param>
        public VisualGradient(Control control, int quality, Color bottomLeft, Color bottomRight, Color topLeft, Color topRight) : this()
        {
            _control = control;
            ConstructVisualGradient(quality, bottomLeft, bottomRight, topLeft, topRight);
        }

        /// <summary>Prevents a default instance of the <see cref="VisualGradient" /> class from being created.</summary>
        private VisualGradient()
        {
            _bottomLeft = Color.Blue;
            _bottomRight = Color.Red;
            _topLeft = Color.Green;
            _topRight = Color.Yellow;

            _quality = 10;
        }

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.Color)]
        public Color BottomLeft
        {
            get
            {
                return _bottomLeft;
            }

            set
            {
                _bottomLeft = value;
                ConstructVisualGradient(_quality, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.Color)]
        public Color BottomRight
        {
            get
            {
                return _bottomRight;
            }

            set
            {
                _bottomRight = value;
                ConstructVisualGradient(_quality, _bottomLeft, _bottomRight, _topLeft, _topRight);
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
                ConstructVisualGradient(_quality, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(Property.Appearance)]
        [Description("The quality of the gradient.")]
        public int Quality
        {
            get
            {
                return _quality;
            }

            set
            {
                _quality = value;
                ConstructVisualGradient(_quality, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.Color)]
        public Color TopLeft
        {
            get
            {
                return _topLeft;
            }

            set
            {
                _topLeft = value;
                ConstructVisualGradient(_quality, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.Color)]
        public Color TopRight
        {
            get
            {
                return _topRight;
            }

            set
            {
                _topRight = value;
                ConstructVisualGradient(_quality, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        #endregion

        #region Events

        /// <summary>Construct visual gradient.</summary>
        /// <param name="quality">The quality.</param>
        /// <param name="bottomLeft">The bottom Left.</param>
        /// <param name="bottomRight">The bottom Right.</param>
        /// <param name="topLeft">The top Left.</param>
        /// <param name="topRight">The top Right.</param>
        private void ConstructVisualGradient(int quality, Color bottomLeft, Color bottomRight, Color topLeft, Color topRight)
        {
            _quality = quality;
            _bottomLeft = bottomLeft;
            _bottomRight = bottomRight;
            _topLeft = topLeft;
            _topRight = topRight;

            UpdateGradient();
        }

        private void UpdateGradient()
        {
            if (_control == null)
            {
                return;
            }

            GDI.ApplyGradientBackground(_control, _topLeft, _topRight, _bottomLeft, _bottomRight, _quality);
        }

        #endregion
    }
}