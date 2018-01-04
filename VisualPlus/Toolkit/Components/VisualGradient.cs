﻿namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Localization;
    using VisualPlus.Managers;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Component))]
    [Description("The VisualPlus gradient component can be used to apply gradient backgrounds on controls.")]
    public class VisualGradient : Component
    {
        #region Variables

        private bool _autoSize;
        private Color _bottomLeft;
        private Color _bottomRight;
        private Control _control;
        private Size _size;
        private Color _topLeft;
        private Color _topRight;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualGradient" /> class.</summary>
        /// <param name="container">The container.</param>
        public VisualGradient(IContainer container) : this()
        {
            container.Add(this);
            ConstructVisualGradient(new Size(), _bottomLeft, _bottomRight, _topLeft, _topRight);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualGradient" /> class.</summary>
        /// <param name="control">The control.</param>
        public VisualGradient(Control control) : this()
        {
            _control = control;
            Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
            ConstructVisualGradient(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualGradient" /> class.</summary>
        /// <param name="control">The control.</param>
        /// <param name="size">The size.</param>
        /// <param name="bottomLeft">The bottom Left.</param>
        /// <param name="bottomRight">The bottom Right.</param>
        /// <param name="topLeft">The top Left.</param>
        /// <param name="topRight">The top Right.</param>
        public VisualGradient(Control control, Size size, Color bottomLeft, Color bottomRight, Color topLeft, Color topRight) : this()
        {
            _control = control;
            ConstructVisualGradient(size, bottomLeft, bottomRight, topLeft, topRight);
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
            Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
            ConstructVisualGradient(_gradientSize, bottomLeft, bottomRight, topLeft, topRight);
        }

        /// <summary>Prevents a default instance of the <see cref="VisualGradient" /> class from being created.</summary>
        private VisualGradient()
        {
            _bottomLeft = Color.Blue;
            _bottomRight = Color.Red;
            _topLeft = Color.Green;
            _topRight = Color.Yellow;
            _size = new Size(25, 25);
            _autoSize = true;
        }

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.AutoSize)]
        public bool AutoSize
        {
            get
            {
                return _autoSize;
            }

            set
            {
                if (value == _autoSize)
                {
                    return;
                }

                _autoSize = value;
                Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
                ConstructVisualGradient(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color BottomLeft
        {
            get
            {
                return _bottomLeft;
            }

            set
            {
                _bottomLeft = value;
                Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
                ConstructVisualGradient(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color BottomRight
        {
            get
            {
                return _bottomRight;
            }

            set
            {
                _bottomRight = value;
                Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
                ConstructVisualGradient(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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
                if (value == null)
                {
                    return;
                }

                _control = value;

                _control.Resize += Control_Resize;
                Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
                ConstructVisualGradient(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        /// <summary>Gets the <see cref="VisualGradient" /> as a bitmap.</summary>
        [Browsable(true)]
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Image)]
        public Image Image
        {
            get
            {
                Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
                return ImageManager.CreateGradientBitmap(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        /// <summary>Gets a value indicating whether this <see cref="VisualGradient" /> is empty.</summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return _bottomLeft.IsEmpty && _bottomRight.IsEmpty && _topLeft.IsEmpty && _topRight.IsEmpty;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Size Size
        {
            get
            {
                return _size;
            }

            set
            {
                if (value == _size)
                {
                    return;
                }

                _size = value;
                Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
                ConstructVisualGradient(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TopLeft
        {
            get
            {
                return _topLeft;
            }

            set
            {
                _topLeft = value;
                Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
                ConstructVisualGradient(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TopRight
        {
            get
            {
                return _topRight;
            }

            set
            {
                _topRight = value;
                Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
                ConstructVisualGradient(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
            }
        }

        #endregion

        #region Events

        /// <summary>Construct visual gradient.</summary>
        /// <param name="gradientSize">The size of the gradient.</param>
        /// <param name="bottomLeft">The bottom Left.</param>
        /// <param name="bottomRight">The bottom Right.</param>
        /// <param name="topLeft">The top Left.</param>
        /// <param name="topRight">The top Right.</param>
        private void ConstructVisualGradient(Size gradientSize, Color bottomLeft, Color bottomRight, Color topLeft, Color topRight)
        {
            _bottomLeft = bottomLeft;
            _bottomRight = bottomRight;
            _topLeft = topLeft;
            _topRight = topRight;
            Size _gradientSize = gradientSize;

            if (_control == null)
            {
                return;
            }

            GraphicsManager.ApplyGradientBackground(_control, _gradientSize, _topLeft, _topRight, _bottomLeft, _bottomRight);
        }

        private void Control_Resize(object sender, EventArgs e)
        {
            Size _gradientSize = GetGradientSize(_autoSize, _control, _size);
            ConstructVisualGradient(_gradientSize, _bottomLeft, _bottomRight, _topLeft, _topRight);
        }

        /// <summary>Gets the gradient size.</summary>
        /// <param name="autoSize">The auto size toggle.</param>
        /// <param name="control">The control.</param>
        /// <param name="custom">The custom size.</param>
        /// <returns>The <see cref="Size" />.</returns>
        private Size GetGradientSize(bool autoSize, Control control, Size custom)
        {
            Size _newSize;

            if (autoSize)
            {
                if (control == null)
                {
                    _newSize = custom;
                }
                else
                {
                    _newSize = control.Size;
                    _size = _newSize;
                }
            }
            else
            {
                _newSize = custom;
            }

            return _newSize;
        }

        #endregion
    }
}