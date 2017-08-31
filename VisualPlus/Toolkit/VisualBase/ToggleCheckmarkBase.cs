namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Extensibility;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public abstract class ToggleCheckmarkBase : ToggleBase, IAnimationSupport
    {
        #region Variables

        private bool _animation;
        private Border _border;
        private Rectangle _box;
        private int _boxSpacing;
        private CheckStyle _checkStyle;
        private ControlColorState _colorState;
        private Point _mouseLocation;
        private VFXManager _rippleEffectsManager;
        private Size _textSize;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ToggleCheckmarkBase" /> class.</summary>
        protected ToggleCheckmarkBase()
        {
            _boxSpacing = 2;
            _box = new Rectangle(0, 0, 14, 14);
            _animation = Settings.DefaultValue.Animation;
            _checkStyle = new CheckStyle(ClientRectangle);
            _border = new Border();

            _colorState = new ControlColorState();
            ConfigureAnimation();
        }

        #endregion

        #region Properties

        [DefaultValue(Settings.DefaultValue.Animation)]
        [Category(Propertys.Behavior)]
        [Description(Property.Animation)]
        public bool Animation
        {
            get
            {
                return _animation;
            }

            set
            {
                _animation = value;

                // Make AutoSize directly set the bounds.
                AutoSize = AutoSize;

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public Border Border
        {
            get
            {
                return _border;
            }

            set
            {
                _border = value;
                Invalidate();
            }
        }

        [Description(Property.Size)]
        [Category(Propertys.Layout)]
        public Size Box
        {
            get
            {
                return _box.Size;
            }

            set
            {
                _box.Size = value;
                if (AutoSize)
                {
                    AutoFit(Text.MeasureText(Font));
                }

                Invalidate();
            }
        }

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState BoxColorState
        {
            get
            {
                return _colorState;
            }

            set
            {
                _colorState = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Spacing)]
        public int BoxSpacing
        {
            get
            {
                return _boxSpacing;
            }

            set
            {
                _boxSpacing = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        [Category(Propertys.Behavior)]
        [Description(Property.Checked)]
        public bool Checked
        {
            get
            {
                return Toggle;
            }

            set
            {
                if (Toggle != value)
                {
                    // Store new values
                    Toggle = value;

                    // Generate events
                    OnToggleChanged(new ToggleEventArgs(Toggle));

                    // Repaint
                    Invalidate();
                }
            }
        }

        [TypeConverter(typeof(CheckStyleConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public CheckStyle CheckStyle
        {
            get
            {
                return _checkStyle;
            }

            set
            {
                _checkStyle = value;
                Invalidate();
            }
        }

        /// <summary>Gets the <see cref="GlyphSize" /> of the control.</summary>
        [Browsable(false)]
        [Description(Property.Size)]
        public Size GlyphSize
        {
            get
            {
                return _box.Size;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsBoxLarger { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Size TextSize
        {
            get
            {
                return _textSize;
            }

            set
            {
                _textSize = value;
            }
        }

        #endregion

        #region Events

        public void ConfigureAnimation()
        {
            VFXManager effectsManager = new VFXManager
                {
                    Increment = 0.05,
                    EffectType = EffectType.EaseInOut
                };
            _rippleEffectsManager = new VFXManager(false)
                {
                    Increment = 0.10,
                    SecondaryIncrement = 0.08,
                    EffectType = EffectType.Linear
                };

            effectsManager.OnAnimationProgress += sender => Invalidate();
            _rippleEffectsManager.OnAnimationProgress += sender => Invalidate();
            effectsManager.StartNewAnimation(Toggle ? AnimationDirection.In : AnimationDirection.Out);
        }

        public void DrawAnimation(Graphics graphics)
        {
            if (_animation && _rippleEffectsManager.IsAnimating())
            {
                for (var i = 0; i < _rippleEffectsManager.GetAnimationCount(); i++)
                {
                    double animationValue = _rippleEffectsManager.GetProgress(i);

                    Point animationSource = new Point(_box.X + (_box.Width / 2), _box.Y + (_box.Height / 2));
                    SolidBrush animationBrush = new SolidBrush(Color.FromArgb((int)(animationValue * 40), (bool)_rippleEffectsManager.GetData(i)[0] ? Color.Black : _checkStyle.CheckColor));

                    int height = _box.Height;
                    int size = _rippleEffectsManager.GetDirection(i) == AnimationDirection.InOutIn ? (int)(height * (0.8d + (0.2d * animationValue))) : height;

                    Rectangle _animationBox = new Rectangle(animationSource.X - (size / 2), animationSource.Y - (size / 2), size, size);
                    GraphicsPath _path = VisualBorderRenderer.CreateBorderTypePath(_animationBox, _border);

                    graphics.FillPath(animationBrush, _path);
                    animationBrush.Dispose();
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (DesignMode)
            {
                return;
            }

            MouseState = MouseStates.Normal;
            MouseEnter += (sender, args) =>
                {
                    MouseState = MouseStates.Hover;
                };
            MouseLeave += (sender, args) =>
                {
                    _mouseLocation = new Point(-1, -1);
                    MouseState = MouseStates.Normal;
                };
            MouseDown += (sender, args) =>
                {
                    MouseState = MouseStates.Down;

                    if (_animation && (args.Button == MouseButtons.Left) && GDI.IsMouseInBounds(_mouseLocation, _box))
                    {
                        _rippleEffectsManager.SecondaryIncrement = 0;
                        _rippleEffectsManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { Toggle });
                    }
                };
            MouseUp += (sender, args) =>
                {
                    MouseState = MouseStates.Hover;
                    _rippleEffectsManager.SecondaryIncrement = 0.08;
                };
            MouseMove += (sender, args) =>
                {
                    _mouseLocation = args.Location;
                    Cursor = GDI.IsMouseInBounds(_mouseLocation, _box) ? Cursors.Hand : Cursors.Default;
                };
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MouseState = MouseStates.Down;
            Invalidate();
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (AutoSize)
            {
                _box = new Rectangle(new Point(Padding.Left, (ClientRectangle.Height / 2) - (_box.Height / 2)), _box.Size);
                AutoFit(Text.MeasureText(Font));
            }
            else
            {
                _box = new Rectangle(new Point(Padding.Left, (ClientRectangle.Height / 2) - (_box.Height / 2)), _box.Size);
            }

            Color _backColor = GDI.GetBackColorState(Enabled, BoxColorState.Enabled, BoxColorState.Hover, BoxColorState.Pressed, BoxColorState.Disabled, MouseState);

            Graphics _graphics = e.Graphics;
            _graphics.Clear(Parent.BackColor);
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.TextRenderingHint = TextRenderingHint;

            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            ControlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);

            _graphics.FillRectangle(new SolidBrush(BackColor), _clientRectangle);

            _textSize = GDI.MeasureText(_graphics, Text, Font);
            Point _textLocation = new Point(_box.Right + _boxSpacing, (ClientRectangle.Height / 2) - (_textSize.Height / 2));
            Color _textColor = Enabled ? ForeColor : ForeColorDisabled;

            VisualToggleRenderer.DrawCheckBox(_graphics, Border, _checkStyle, _box, Checked, Enabled, _backColor, BackgroundImage, MouseState, Text, Font, _textColor, _textLocation);
            DrawAnimation(_graphics);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(Parent.BackColor);
        }

        private void AutoFit(Size textSize)
        {
            if (GDI.TextLargerThanRectangle(textSize, _box))
            {
                IsBoxLarger = false;
                Size = new Size(_box.X + _box.Width + _boxSpacing + textSize.Width, textSize.Height);
            }
            else
            {
                IsBoxLarger = true;
                Size = new Size(_box.X + _box.Width + _boxSpacing + textSize.Width, _box.Height);
            }
        }

        #endregion
    }
}