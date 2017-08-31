namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    [Description("The Visual TrackBar")]
    [Designer(ControlManager.FilterProperties.VisualTrackBar)]
    public class VisualTrackBar : TrackBar
    {
        #region Variables

        protected Orientation _orientation = Orientation.Horizontal;

        #endregion

        #region Variables

        private ControlColorState _buttonControlColorState;
        private Color _progressColor;
        private VisualStyleManager _styleManager;
        private ColorState _trackBarColor;
        private int _barThickness;
        private int _barTickSpacing;
        private bool _buttonAutoSize;
        private Border _buttonBorder;
        private GraphicsPath _buttonPath;
        private Rectangle _buttonRectangle;
        private Size _buttonSize;
        private Color _buttonTextColor;
        private bool _buttonVisible;
        private int _currentUsedPos;
        private ValueDivisor _dividedValue;
        private int _fillingValue;
        private Color _foreColor;
        private Color _hatchForeColor;
        private float _hatchSize;
        private HatchStyle _hatchStyle;
        private bool _hatchVisible;
        private int _indentHeight;
        private int _indentWidth;
        private bool _leftButtonDown;
        private bool _lineTicksVisible;
        private float _mouseStartPos;
        private MouseStates _mouseState;
        private string _prefix;
        private bool _progressFilling;
        private bool _progressValueVisible;
        private bool _progressVisible;
        private string _suffix;
        private Size _textAreaSize;
        private Color _textDisabledColor;
        private Font _textFont;
        private TextRenderingHint _textRendererHint;
        private Color _tickColor;
        private int _tickHeight;
        private Border _trackBarBorder;
        private GraphicsPath _trackBarPath;
        private Rectangle _trackBarRectangle;
        private bool _valueTicksVisible;
        private Rectangle _workingRectangle;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.Controls.Interactivity.VisualTrackBar" /> class.</summary>
        public VisualTrackBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor,
                true);

            UpdateStyles();
            _styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);
            _buttonPath = new GraphicsPath();
            _buttonAutoSize = true;
            _buttonSize = new Size(27, 20);
            _barThickness = 10;
            _buttonVisible = true;
            _dividedValue = ValueDivisor.By1;
            _barTickSpacing = 8;
            _fillingValue = 25;
            _hatchForeColor = Color.FromArgb(40, hatchBackColor);
            _hatchSize = 2F;
            _hatchStyle = HatchStyle.DarkDownwardDiagonal;
            _hatchVisible = Settings.DefaultValue.HatchVisible;
            _lineTicksVisible = Settings.DefaultValue.TextVisible;
            _mouseStartPos = -1;
            _progressVisible = Settings.DefaultValue.TextVisible;
            _textRendererHint = Settings.DefaultValue.TextRenderingHint;
            _tickHeight = 4;
            _valueTicksVisible = Settings.DefaultValue.TextVisible;

            _buttonControlColorState = new ControlColorState();
            _trackBarColor = new ColorState();

            BackColor = Color.Transparent;
            DoubleBuffered = true;
            UpdateStyles();
            AutoSize = false;
            Size = new Size(200, 50);
            MinimumSize = new Size(0, 0);

            _trackBarBorder = new Border();
            _buttonBorder = new Border();

            _textRendererHint = Settings.DefaultValue.TextRenderingHint;

            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        public enum ValueDivisor
        {
            /// <summary>The by 1.</summary>
            By1 = 1,

            /// <summary>The by 10.</summary>
            By10 = 10,

            /// <summary>The by 100.</summary>
            By100 = 100,

            /// <summary>The by 1000.</summary>
            By1000 = 1000
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public ControlColorState BackColorState
        {
            get
            {
                return _buttonControlColorState;
            }

            set
            {
                if (value == _buttonControlColorState)
                {
                    return;
                }

                _buttonControlColorState = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public int BarThickness
        {
            get
            {
                return _barThickness;
            }

            set
            {
                _barThickness = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public int BarTickSpacing
        {
            get
            {
                return _barTickSpacing;
            }

            set
            {
                _barTickSpacing = value;
                Invalidate();
            }
        }

        [DefaultValue(true)]
        [Category(Propertys.Behavior)]
        [Description(Property.AutoSize)]
        public bool ButtonAutoSize
        {
            get
            {
                return _buttonAutoSize;
            }

            set
            {
                _buttonAutoSize = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public Border ButtonBorder
        {
            get
            {
                return _buttonBorder;
            }

            set
            {
                _buttonBorder = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public Size ButtonSize
        {
            get
            {
                return _buttonSize;
            }

            set
            {
                _buttonSize = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ButtonTextColor
        {
            get
            {
                return _buttonTextColor;
            }

            set
            {
                _buttonTextColor = value;
                Invalidate();
            }
        }

        [DefaultValue(true)]
        [Category(Propertys.Behavior)]
        [Description(Property.Visible)]
        public bool ButtonVisible
        {
            get
            {
                return _buttonVisible;
            }

            set
            {
                _buttonVisible = value;
                Invalidate();
            }
        }

        [Category(Propertys.Data)]
        [Description("Experiemental: Filling Value.")]
        public int FillingValue
        {
            get
            {
                return _fillingValue;
            }

            set
            {
                _fillingValue = value;
                Invalidate();
            }
        }

        public new Font Font
        {
            get
            {
                return _textFont;
            }

            set
            {
                base.Font = value;
                _textFont = value;
                Invalidate();
            }
        }

        public new Color ForeColor
        {
            get
            {
                return _foreColor;
            }

            set
            {
                base.ForeColor = value;
                _foreColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color HatchBackColor
        {
            get
            {
                return hatchBackColor;
            }

            set
            {
                hatchBackColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color HatchForeColor
        {
            get
            {
                return _hatchForeColor;
            }

            set
            {
                _hatchForeColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]

        // [DefaultValue(Settings.DefaultValue.HatchSize)]
        [Description(Property.Size)]
        public float HatchSize
        {
            get
            {
                return _hatchSize;
            }

            set
            {
                _hatchSize = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Type)]
        public HatchStyle HatchStyle
        {
            get
            {
                return _hatchStyle;
            }

            set
            {
                _hatchStyle = value;
                Invalidate();
            }
        }

        [DefaultValue(Settings.DefaultValue.HatchVisible)]
        [Category(Propertys.Behavior)]
        [Description(Property.Visible)]
        public bool HatchVisible
        {
            get
            {
                return _hatchVisible;
            }

            set
            {
                _hatchVisible = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public int IndentHeight
        {
            get
            {
                return _indentHeight;
            }

            set
            {
                _indentHeight = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public int IndentWidth
        {
            get
            {
                return _indentWidth;
            }

            set
            {
                _indentWidth = value;
                Invalidate();
            }
        }

        [DefaultValue(Settings.DefaultValue.TextVisible)]
        [Category(Propertys.Appearance)]
        [Description(Property.Visible)]
        public bool LineTicksVisible
        {
            get
            {
                return _lineTicksVisible;
            }

            set
            {
                _lineTicksVisible = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Orientation)]
        public new Orientation Orientation
        {
            get
            {
                return _orientation;
            }

            set
            {
                _orientation = value;
                Size = GDI.FlipOrientationSize(_orientation, Size);
                Invalidate();
            }
        }

        [Category(Propertys.Data)]
        [Description(Property.Visible)]
        public string Prefix
        {
            get
            {
                return _prefix;
            }

            set
            {
                _prefix = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ProgressColor
        {
            get
            {
                return _progressColor;
            }

            set
            {
                _progressColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Visible)]
        public bool ProgressFilling
        {
            get
            {
                return _progressFilling;
            }

            set
            {
                _progressFilling = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        [Category(Propertys.Behavior)]
        [Description(Property.Visible)]
        public bool ProgressValueVisible
        {
            get
            {
                return _progressValueVisible;
            }

            set
            {
                _progressValueVisible = value;
                Invalidate();
            }
        }

        [DefaultValue(true)]
        [Category(Propertys.Behavior)]
        [Description(Property.Visible)]
        public bool ProgressVisible
        {
            get
            {
                return _progressVisible;
            }

            set
            {
                _progressVisible = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.MouseState)]
        public MouseStates State
        {
            get
            {
                return _mouseState;
            }

            set
            {
                _mouseState = value;
                Invalidate();
            }
        }

        [Category(Propertys.Data)]
        [Description(Property.Visible)]
        public string Suffix
        {
            get
            {
                return _suffix;
            }

            set
            {
                _suffix = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color TextDisabledColor
        {
            get
            {
                return _textDisabledColor;
            }

            set
            {
                _textDisabledColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.TextRenderingHint)]
        public TextRenderingHint TextRendering
        {
            get
            {
                return _textRendererHint;
            }

            set
            {
                _textRendererHint = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color TickColor
        {
            get
            {
                return _tickColor;
            }

            set
            {
                _tickColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public int TickHeight
        {
            get
            {
                return _tickHeight;
            }

            set
            {
                _tickHeight = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public Border TrackBar
        {
            get
            {
                return _trackBarBorder;
            }

            set
            {
                _trackBarBorder = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(ColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public ColorState TrackBarState
        {
            get
            {
                return _trackBarColor;
            }

            set
            {
                if (value == _trackBarColor)
                {
                    return;
                }

                _trackBarColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Behavior)]
        [Description(Property.ValueDivisor)]
        public ValueDivisor ValueDivision
        {
            get
            {
                return _dividedValue;
            }

            set
            {
                _dividedValue = value;
                Invalidate();
            }
        }

        [DefaultValue(Settings.DefaultValue.TextVisible)]
        [Category(Propertys.Appearance)]
        [Description(Property.Visible)]
        public bool ValueTicksVisible
        {
            get
            {
                return _valueTicksVisible;
            }

            set
            {
                _valueTicksVisible = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        /// <summary>Call the Decrement() method to decrease the value displayed by an integer you specify.</summary>
        /// <param name="value">The value to decrement.</param>
        public void Decrement(int value)
        {
            if (Value > Minimum)
            {
                Value -= value;
                if (Value < Minimum)
                {
                    Value = Minimum;
                }
            }
            else
            {
                Value = Minimum;
            }

            Invalidate();
        }

        /// <summary>Get's the formatted progress value.</summary>
        /// <returns>Formatted progress value.</returns>
        public string GetFormattedProgressValue()
        {
            var value = (float)(Value / (double)_dividedValue);
            string formattedString = $"{Prefix}{value}{Suffix}";

            return formattedString;
        }

        /// <summary>Call the Increment() method to increase the value displayed by an integer you specify.</summary>
        /// <param name="value">The value to increment.</param>
        public void Increment(int value)
        {
            if (Value < Maximum)
            {
                Value += value;
                if (Value > Maximum)
                {
                    Value = Maximum;
                }
            }
            else
            {
                Value = Maximum;
            }

            Invalidate();
        }

        /// <summary>Sets a new range value.</summary>
        /// <param name="minimumValue">The minimum.</param>
        /// <param name="maximumValue">The maximum.</param>
        public new void SetRange(int minimumValue, int maximumValue)
        {
            Minimum = minimumValue;

            if (Minimum > Value)
            {
                Value = Minimum;
            }

            Maximum = maximumValue;

            if (Maximum < Value)
            {
                Value = Maximum;
            }

            if (Maximum < Minimum)
            {
                Minimum = Maximum;
            }

            Invalidate();
        }

        public void UpdateTheme(Styles style)
        {
            VisualStyleManager _styleManager = new VisualStyleManager(style);

            ForeColor = _styleManager.FontStyle.ForeColor;

            Font = _styleManager.Font;
            _textFont = Font;
            _foreColor = _styleManager.FontStyle.ForeColor;
            _buttonTextColor = ForeColor;
            _textDisabledColor = _styleManager.FontStyle.ForeColorDisabled;

            _progressColor = _styleManager.ProgressStyle.Progress.Colors[0];

            _buttonControlColorState.Enabled = _styleManager.ControlStyle.Background(0);
            _buttonControlColorState.Disabled = Color.FromArgb(224, 224, 224);
            _buttonControlColorState.Hover = Color.FromArgb(224, 224, 224);
            _buttonControlColorState.Pressed = Color.Silver;

            _trackBarColor.Enabled = _styleManager.ProgressStyle.BackProgress.Colors[0];
            _trackBarColor.Disabled = _styleManager.ProgressStyle.BackProgress.Colors[0];

            hatchBackColor = _styleManager.ProgressStyle.Hatch;
            _tickColor = _styleManager.ControlStyle.Line;

            _buttonBorder.Color = _styleManager.BorderStyle.Color;
            _buttonBorder.HoverColor = _styleManager.BorderStyle.HoverColor;

            _trackBarBorder.Color = _styleManager.BorderStyle.Color;
            _trackBarBorder.HoverColor = _styleManager.BorderStyle.HoverColor;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var offsetValue = 0;
            Point currentPoint = new Point(e.X, e.Y);

            // TODO: Improve location accuracy
            if (trackerRectangle.Contains(currentPoint))
            {
                if (!_leftButtonDown)
                {
                    _mouseState = MouseStates.Down;
                    _leftButtonDown = true;
                    Capture = true;
                    switch (_orientation)
                    {
                        case Orientation.Horizontal:
                            {
                                _mouseStartPos = currentPoint.X - trackerRectangle.X;
                                Invalidate();
                                break;
                            }

                        case Orientation.Vertical:
                            {
                                _mouseStartPos = currentPoint.Y - trackerRectangle.Y;
                                Invalidate();
                                break;
                            }
                    }
                }
            }
            else
            {
                switch (_orientation)
                {
                    case Orientation.Horizontal:
                        {
                            if ((currentPoint.X + _buttonSize.Width) / 2 >= Width - _indentWidth)
                            {
                                offsetValue = Maximum - Minimum;
                            }
                            else if ((currentPoint.X - _buttonSize.Width) / 2 <= _indentWidth)
                            {
                                offsetValue = 0;
                            }
                            else
                            {
                                offsetValue = (int)(((((currentPoint.X - _indentWidth - _buttonSize.Width) / 2) * (Maximum - Minimum)) / (Width - (2 * _indentWidth) - _buttonSize.Width)) + 0.5);
                            }

                            break;
                        }

                    case Orientation.Vertical:
                        {
                            if ((currentPoint.Y + _buttonSize.Width) / 2 >= Height - _indentHeight)
                            {
                                offsetValue = 0;
                            }
                            else if ((currentPoint.Y - _buttonSize.Width) / 2 <= _indentHeight)
                            {
                                offsetValue = Maximum - Minimum;
                            }
                            else
                            {
                                offsetValue = (int)(((((Height - currentPoint.Y - _indentHeight - _buttonSize.Width) / 2) * (Maximum - Minimum)) / (Height - (2 * _indentHeight) - _buttonSize.Width)) + 0.5);
                            }

                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                int oldValue = Value;
                Value = Minimum + offsetValue;

                Invalidate();

                if (oldValue != Value)
                {
                    OnScroll(e);
                    OnValueChanged(e);
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            OnEnter(e);
            State = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            Cursor = _orientation == Orientation.Vertical ? Cursors.SizeNS : Cursors.SizeWE;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            OnLeave(e);
            State = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var offsetValue = 0;
            PointF currentPoint = new PointF(e.X, e.Y);

            if (_leftButtonDown)
            {
                try
                {
                    // TODO: Improve location accuracy
                    switch (_orientation)
                    {
                        case Orientation.Horizontal:
                            {
                                if ((currentPoint.X + _buttonSize.Width) - _mouseStartPos >= Width - _indentWidth)
                                {
                                    offsetValue = Maximum - Minimum;
                                }
                                else if (currentPoint.X - _mouseStartPos <= _indentWidth)
                                {
                                    offsetValue = 0;
                                }
                                else
                                {
                                    offsetValue = (int)((((currentPoint.X - _mouseStartPos - _indentWidth) * (Maximum - Minimum)) / (Width - (2 * _indentWidth) - _buttonSize.Width)) + 0.5);
                                }

                                break;
                            }

                        case Orientation.Vertical:
                            {
                                if ((currentPoint.Y + _buttonSize.Height) / 2 >= Height - _indentHeight)
                                {
                                    offsetValue = 0;
                                }
                                else if ((currentPoint.Y + _buttonSize.Height) / 2 <= _indentHeight)
                                {
                                    offsetValue = Maximum - Minimum;
                                }
                                else
                                {
                                    offsetValue = (int)(((((((Height - currentPoint.Y) + _buttonSize.Height) / 2) - _mouseStartPos - _indentHeight) * (Maximum - Minimum)) / (Height - (2 * _indentHeight))) + 0.5);
                                }

                                break;
                            }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    int oldValue = Value;

                    // TODO: Vertical exception is caused when trying to scroll passed the bottom
                    Value = Minimum + offsetValue;
                    Invalidate();

                    if (oldValue != Value)
                    {
                        OnScroll(e);
                        OnValueChanged(e);
                    }
                }
            }

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _leftButtonDown = false;
            _mouseState = MouseStates.Normal;
            Capture = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(Parent.BackColor);
            graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = _textRendererHint;

            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            GraphicsPath controlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _trackBarBorder);

            _workingRectangle = Rectangle.Inflate(_clientRectangle, -_indentWidth, -_indentHeight);

            // Set control state color
            _foreColor = Enabled ? _foreColor : _textDisabledColor;

            // Step 1 - Configure tick style
            ConfigureTickStyle(graphics);

            // Step 2 - Draw the progress
            if (_progressVisible)
            {
                DrawProgress(graphics);
            }

            Size formattedProgressValue = GDI.MeasureText(graphics, Maximum.ToString(), _textFont);

            // Step 3 - Draw the Tracker
            DrawButton(graphics, formattedProgressValue);

            // Step 4 - Draw progress value
            if (_progressValueVisible)
            {
                string value = GetFormattedProgressValue();

                // Position
                Point progressValueLocation = new Point();
                if (_buttonVisible)
                {
                    progressValueLocation = new Point((_buttonRectangle.X + (_buttonRectangle.Width / 2)) - (formattedProgressValue.Width / 2), (_buttonRectangle.Y + (_buttonRectangle.Height / 2)) - (formattedProgressValue.Height / 2));
                }
                else
                {
                    switch (Orientation)
                    {
                        case Orientation.Horizontal:
                            {
                                progressValueLocation = new Point(trackerRectangle.X, (_trackBarRectangle.Y + (_trackBarRectangle.Height / 2)) - (formattedProgressValue.Height / 2));
                                break;
                            }

                        case Orientation.Vertical:
                            {
                                progressValueLocation = new Point(_trackBarRectangle.X, trackerRectangle.Y);
                                break;
                            }
                    }
                }

                // Draw the formatted progress value
                graphics.DrawString(value, _textFont, new SolidBrush(_buttonTextColor), progressValueLocation);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
        }

        protected override void OnScroll(EventArgs e)
        {
            base.OnScroll(e);
            Invalidate();
        }

        protected override void OnValueChanged(EventArgs e)
        {
            Invalidate();
        }

        /// <summary>This member overrides <see cref="Control.ProcessCmdKey">Control.ProcessCmdKey</see>.</summary>
        /// <param name="msg">The msg.</param>
        /// <param name="keyData">The key Data.</param>
        /// <returns>The <see cref="bool" />.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var result = true;

            // Specified WM_KEYDOWN enumeration value.
            const int WM_KEYDOWN = 0x0100;

            // Specified WM_SYSKEYDOWN enumeration value.
            const int WM_SYSKEYDOWN = 0x0104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Left:
                    case Keys.Down:
                        {
                            Decrement(SmallChange);
                            break;
                        }

                    case Keys.Right:
                    case Keys.Up:
                        {
                            Increment(SmallChange);
                            break;
                        }

                    case Keys.PageUp:
                        {
                            Increment(LargeChange);
                            break;
                        }

                    case Keys.PageDown:
                        {
                            Decrement(LargeChange);
                            break;
                        }

                    case Keys.Home:
                        {
                            Value = Maximum;
                            break;
                        }

                    case Keys.End:
                        {
                            Value = Minimum;
                            break;
                        }

                    default:
                        {
                            result = base.ProcessCmdKey(ref msg, keyData);
                            break;
                        }
                }
            }

            return result;
        }

        private static Color hatchBackColor;

        private static Rectangle trackerRectangle = Rectangle.Empty;

        /// <summary>Configures the tick style.</summary>
        /// <param name="graphics">Graphics input.</param>
        private void ConfigureTickStyle(Graphics graphics)
        {
            int currentTrackerPos;
            Point trackBarLocation = new Point();
            Size trackBarSize;
            Point trackerLocation;
            Size trackerSize;

            // Draw tick by orientation
            if (_orientation == Orientation.Horizontal)
            {
                // Start location
                _currentUsedPos = _indentHeight;

                // Draw value tick
                if (_valueTicksVisible)
                {
                    HorizontalStyle(graphics, _workingRectangle, false);
                }

                // Draw line tick
                if (_lineTicksVisible)
                {
                    HorizontalStyle(graphics, _workingRectangle, true);
                }

                // Setups the location & sizing
                switch (TickStyle)
                {
                    case TickStyle.TopLeft:
                        {
                            if (_buttonVisible)
                            {
                                trackBarLocation = new Point(0, _indentHeight + _textAreaSize.Height + _tickHeight + _barTickSpacing);
                                Size = new Size(ClientRectangle.Width, _indentHeight + _textAreaSize.Height + _tickHeight + _barTickSpacing + _barThickness + (_buttonSize.Height / 2));
                            }
                            else
                            {
                                trackBarLocation = new Point(0, _indentHeight + _textAreaSize.Height + _tickHeight + _barTickSpacing);
                                Size = new Size(ClientRectangle.Width, _indentHeight + _textAreaSize.Height + _tickHeight + _barTickSpacing + _barThickness);
                            }

                            break;
                        }

                    case TickStyle.BottomRight:
                        {
                            if (_buttonVisible)
                            {
                                trackBarLocation = new Point(0, _indentHeight + (_buttonSize.Height / 2));
                                Size = new Size(ClientRectangle.Width, _indentHeight + _barThickness + _barTickSpacing + _tickHeight + _textAreaSize.Height + (_textAreaSize.Height / 2));
                            }
                            else
                            {
                                trackBarLocation = new Point(0, _indentHeight);
                                Size = new Size(ClientRectangle.Width, _indentHeight + _barThickness + _barTickSpacing + _tickHeight + _textAreaSize.Height);
                            }

                            break;
                        }

                    case TickStyle.None:
                        {
                            if (_buttonVisible)
                            {
                                trackBarLocation = new Point(0, _indentHeight + (_buttonSize.Height / 2));
                                Size = new Size(ClientRectangle.Width, _indentHeight + _barThickness + _buttonSize.Height);
                            }
                            else
                            {
                                trackBarLocation = new Point(0, _indentHeight);
                                Size = new Size(ClientRectangle.Width, _indentHeight + _barThickness);
                            }

                            break;
                        }

                    case TickStyle.Both:
                        {
                            int totalHeight = _indentHeight + _textAreaSize.Height + _tickHeight + _barTickSpacing + _barThickness + _barTickSpacing + _tickHeight + _textAreaSize.Height + (_textAreaSize.Height / 2);

                            trackBarLocation = new Point(0, _indentHeight + _textAreaSize.Height + _tickHeight + _barTickSpacing);
                            Size = new Size(ClientRectangle.Width, totalHeight);

                            break;
                        }
                }

                trackBarSize = new Size(_workingRectangle.Width, _barThickness);
                _trackBarRectangle = new Rectangle(trackBarLocation, trackBarSize);

                // Get tracker position
                currentTrackerPos = RetrieveTrackerPosition(_workingRectangle);

                // Remember this for drawing the Tracker later
                trackerLocation = new Point(currentTrackerPos, _currentUsedPos);
                trackerSize = new Size(_buttonSize.Width, _buttonSize.Height);
                trackerRectangle = new Rectangle(trackerLocation, trackerSize);

                // Draws track bar
                DrawBar(graphics, _trackBarRectangle);

                // Update current position
                _currentUsedPos += _buttonSize.Height;

                // Draw value tick
                if (_valueTicksVisible)
                {
                    HorizontalStyle(graphics, _workingRectangle, false);
                }

                // Draw line tick
                if (_lineTicksVisible)
                {
                    HorizontalStyle(graphics, _workingRectangle, true);
                }
            }
            else
            {
                // Start location
                _currentUsedPos = _indentWidth;

                // Draw value tick
                if (_valueTicksVisible)
                {
                    VerticalStyle(graphics, _workingRectangle, false);
                }

                // Draw line tick
                if (_lineTicksVisible)
                {
                    VerticalStyle(graphics, _workingRectangle, true);
                }

                // Setups the location & sizing
                switch (TickStyle)
                {
                    case TickStyle.TopLeft:
                        {
                            if (_buttonVisible)
                            {
                                trackBarLocation = new Point(_indentWidth + _textAreaSize.Width + _tickHeight + _barTickSpacing, 0);
                                Size = new Size(_indentWidth + _textAreaSize.Width + _tickHeight + _barTickSpacing + _barThickness + (_buttonSize.Width / 2), ClientRectangle.Height);
                            }
                            else
                            {
                                trackBarLocation = new Point(_indentWidth + _textAreaSize.Width + _tickHeight + _barTickSpacing, 0);
                                Size = new Size(_indentWidth + _textAreaSize.Width + _tickHeight + _barTickSpacing + _barThickness, ClientRectangle.Height);
                            }

                            break;
                        }

                    case TickStyle.BottomRight:
                        {
                            if (_buttonVisible)
                            {
                                trackBarLocation = new Point(_indentWidth + (_buttonSize.Width / 2), 0);
                                Size = new Size(_indentWidth + _barThickness + _barTickSpacing + _tickHeight + _textAreaSize.Width + (_buttonSize.Width / 2), ClientRectangle.Height);
                            }
                            else
                            {
                                trackBarLocation = new Point(0, _indentWidth);
                                Size = new Size(_indentWidth + _barThickness + _barTickSpacing + _tickHeight + _textAreaSize.Width, ClientRectangle.Height);
                            }

                            break;
                        }

                    case TickStyle.None:
                        {
                            if (_buttonVisible)
                            {
                                trackBarLocation = new Point(_indentWidth + (_buttonSize.Width / 2), _indentHeight);
                                Size = new Size(_indentWidth + _barThickness + _buttonSize.Width, ClientRectangle.Height);
                            }
                            else
                            {
                                trackBarLocation = new Point(_indentWidth, _indentHeight);
                                Size = new Size(_indentWidth + _barThickness, ClientRectangle.Height);
                            }

                            break;
                        }

                    case TickStyle.Both:
                        {
                            int totalWidth = _indentWidth + _textAreaSize.Width + _tickHeight + _barTickSpacing + _barThickness + _barTickSpacing + _tickHeight + _textAreaSize.Width;

                            trackBarLocation = new Point(_indentWidth + _textAreaSize.Width + _tickHeight + _barTickSpacing, 0);
                            Size = new Size(totalWidth, ClientRectangle.Height);

                            break;
                        }
                }

                trackBarSize = new Size(_barThickness, ClientRectangle.Height - _indentHeight);
                _trackBarRectangle = new Rectangle(trackBarLocation, trackBarSize);

                // Get tracker position
                currentTrackerPos = RetrieveTrackerPosition(_workingRectangle);

                // Remember this for drawing the Tracker later
                trackerLocation = new Point(_currentUsedPos, _workingRectangle.Bottom - currentTrackerPos - _buttonSize.Height);
                trackerSize = new Size(_buttonSize.Width, _buttonSize.Height);
                trackerRectangle = new Rectangle(trackerLocation, trackerSize);

                // Draw the track bar
                DrawBar(graphics, _trackBarRectangle);

                // Update current position
                _currentUsedPos += _buttonSize.Height;

                // Draw value tick
                if (_valueTicksVisible)
                {
                    VerticalStyle(graphics, _workingRectangle, false);
                }

                // Draw line tick
                if (_lineTicksVisible)
                {
                    VerticalStyle(graphics, _workingRectangle, true);
                }
            }
        }

        /// <summary>Draws the bar.</summary>
        /// <param name="graphics">Graphics input.</param>
        /// <param name="barRectangle">Bar rectangle.</param>
        private void DrawBar(Graphics graphics, Rectangle barRectangle)
        {
            Point trackLocation;
            Size trackSize;

            if (Orientation == Orientation.Horizontal)
            {
                trackLocation = new Point(_indentWidth + barRectangle.Left, _indentHeight + barRectangle.Top);
                trackSize = new Size(barRectangle.Width, barRectangle.Height);
            }
            else
            {
                trackLocation = new Point(_indentWidth + barRectangle.Left, _indentHeight + barRectangle.Top);
                trackSize = new Size(barRectangle.Width, barRectangle.Height);
            }

            _trackBarRectangle = new Rectangle(trackLocation, trackSize);
            _trackBarPath = VisualBorderRenderer.CreateBorderTypePath(_trackBarRectangle, _trackBarBorder);

            Color _backColor = Enabled ? _trackBarColor.Enabled : _trackBarColor.Disabled;
            graphics.FillPath(new SolidBrush(_backColor), _trackBarPath);

            VisualBorderRenderer.DrawBorderStyle(graphics, _trackBarBorder, _trackBarPath, State);
        }

        /// <summary>Draws the button.</summary>
        /// <param name="graphics">Graphics input.</param>
        /// <param name="progressValue">The progress Value.</param>
        private void DrawButton(Graphics graphics, Size progressValue)
        {
            Point buttonLocation = new Point();
            graphics.ResetClip();

            // Determine button location by orientation
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        buttonLocation = new Point(trackerRectangle.X, (_trackBarRectangle.Top + (_barThickness / 2)) - (_buttonSize.Height / 2));

                        if (_buttonAutoSize)
                        {
                            _buttonSize = new Size(progressValue.Width, _buttonSize.Height);
                        }
                        else
                        {
                            _buttonSize = new Size(_buttonSize.Width, _buttonSize.Height);
                        }

                        break;
                    }

                case Orientation.Vertical:
                    {
                        buttonLocation = new Point((_trackBarRectangle.Left + (_barThickness / 2)) - (_buttonSize.Width / 2), trackerRectangle.Y);

                        if (_buttonAutoSize)
                        {
                            _buttonSize = new Size(_buttonSize.Width, progressValue.Height);
                        }
                        else
                        {
                            _buttonSize = new Size(_buttonSize.Width, _buttonSize.Height);
                        }

                        break;
                    }
            }

            _buttonRectangle = new Rectangle(buttonLocation, _buttonSize);

            if (_buttonVisible)
            {
                // Setup button colors
                Color _backColor = GDI.GetBackColorState(Enabled, _buttonControlColorState.Enabled, _buttonControlColorState.Hover, _buttonControlColorState.Pressed, _buttonControlColorState.Disabled, _mouseState);

                _buttonPath = VisualBorderRenderer.CreateBorderTypePath(_buttonRectangle, _buttonBorder);
                graphics.FillPath(new SolidBrush(_backColor), _buttonPath);

                VisualBorderRenderer.DrawBorderStyle(graphics, _buttonBorder, _buttonPath, State);
            }
        }

        /// <summary>Draws the TrackBar progress.</summary>
        /// <param name="graphics">Graphics input.</param>
        private void DrawProgress(Graphics graphics)
        {
            GraphicsPath progressPath = new GraphicsPath();
            Rectangle progressRectangle;
            Point progressLocation;
            Size progressSize;

            var barProgress = 0;

            // Progress setup
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        // Draws the progress to the middle of the button
                        barProgress = _buttonRectangle.X + (_buttonRectangle.Width / 2);

                        progressLocation = new Point(0, 0);
                        progressSize = new Size(barProgress, Height);

                        if ((Value == Minimum) && _progressFilling)
                        {
                            progressLocation = new Point(barProgress, Height);
                        }

                        if ((Value == Maximum) && _progressFilling)
                        {
                            progressSize = new Size(barProgress + _fillingValue, Height);
                        }

                        progressRectangle = new Rectangle(progressLocation, progressSize);
                        progressPath = VisualBorderRenderer.CreateBorderTypePath(progressRectangle, _trackBarBorder);
                    }

                    break;
                case Orientation.Vertical:
                    {
                        // Draws the progress to the middle of the button
                        barProgress = _buttonRectangle.Y + (_buttonRectangle.Height / 2);

                        progressLocation = new Point(0, barProgress);

                        if ((Value == Minimum) && _progressFilling)
                        {
                            progressLocation = new Point(0, barProgress + _fillingValue);
                        }

                        if ((Value == Maximum) && _progressFilling)
                        {
                            progressLocation = new Point(0, barProgress - _fillingValue);
                        }

                        progressSize = new Size(Width, Height + _textAreaSize.Height);
                        progressRectangle = new Rectangle(progressLocation, progressSize);
                        progressPath = VisualBorderRenderer.CreateBorderTypePath(progressRectangle, _trackBarBorder);
                    }

                    break;
            }

            graphics.SetClip(_trackBarPath);

            if (barProgress > 1)
            {
                graphics.FillPath(new SolidBrush(_progressColor), progressPath);

                if (_hatchVisible)
                {
                    HatchBrush hatchBrush = new HatchBrush(_hatchStyle, _hatchForeColor, hatchBackColor);
                    using (TextureBrush textureBrush = GDI.DrawTextureUsingHatch(hatchBrush))
                    {
                        textureBrush.ScaleTransform(_hatchSize, _hatchSize);
                        graphics.FillPath(textureBrush, progressPath);
                        graphics.ResetClip();
                    }
                }

                graphics.ResetClip();
            }
        }

        private void HorizontalStyle(Graphics graphics, Rectangle rectangle, bool line)
        {
            Rectangle tickRectangle;
            _currentUsedPos = _indentHeight;
            Point location;
            Size size;
            _textAreaSize = GDI.MeasureText(graphics, Maximum.ToString(), _textFont);

            if (line)
            {
                if ((TickStyle == TickStyle.TopLeft) || (TickStyle == TickStyle.Both))
                {
                    // Retrieve tick barRectangle
                    location = new Point(rectangle.Left, _currentUsedPos + _textAreaSize.Height);
                    size = new Size(rectangle.Width, _tickHeight);
                    tickRectangle = new Rectangle(location, size);

                    // Enlarge tick barRectangle
                    // tickRectangle.Inflate(-buttonSize.Width / 2, 0);

                    // tickRectangle.Inflate(-buttonSize.Width / 2, 0);

                    // Move next tick area
                    _currentUsedPos += _tickHeight;

                    // Draw tick line
                    GDI.DrawTickLine(graphics, tickRectangle, TickFrequency, Minimum, Maximum, _tickColor, _orientation);
                }

                if ((TickStyle == TickStyle.BottomRight) || (TickStyle == TickStyle.Both))
                {
                    // Retrieve tick barRectangle
                    location = new Point(rectangle.Left, _trackBarRectangle.Bottom + _barTickSpacing);
                    size = new Size(rectangle.Width, _tickHeight);
                    tickRectangle = new Rectangle(location, size);

                    // Enlarge tick barRectangle
                    tickRectangle.Inflate(-_buttonSize.Width / 2, 0);

                    // Move next tick area
                    _currentUsedPos += _tickHeight;

                    // Draw tick line
                    GDI.DrawTickLine(graphics, tickRectangle, TickFrequency, Minimum, Maximum, _tickColor, _orientation);
                }
            }
            else
            {
                if ((TickStyle == TickStyle.TopLeft) || (TickStyle == TickStyle.Both))
                {
                    // Retrieve text barRectangle
                    location = new Point(rectangle.Left, _currentUsedPos);
                    size = new Size(rectangle.Width, _textAreaSize.Height);
                    tickRectangle = new Rectangle(location, size);

                    // Enlarge text barRectangle
                    tickRectangle.Inflate(-_buttonSize.Width / 2, 0);

                    // Move next text area
                    _currentUsedPos += _tickHeight;

                    // Draw text 
                    GDI.DrawTickTextLine(graphics, tickRectangle, TickFrequency, Minimum, Maximum, _foreColor, _textFont, _orientation);
                }

                if ((TickStyle == TickStyle.BottomRight) || (TickStyle == TickStyle.Both))
                {
                    // Retrieve text barRectangle
                    location = new Point(rectangle.Left, _trackBarRectangle.Y + _trackBarRectangle.Height + _trackBarBorder.Rounding + _barTickSpacing + _tickHeight + _currentUsedPos);
                    size = new Size(rectangle.Width, _tickHeight);
                    tickRectangle = new Rectangle(location, size);

                    // Enlarge text barRectangle
                    tickRectangle.Inflate(-_buttonSize.Width / 2, 0);

                    // Move next text area
                    _currentUsedPos += _tickHeight;

                    // Draw text 
                    GDI.DrawTickTextLine(graphics, tickRectangle, TickFrequency, Minimum, Maximum, _foreColor, _textFont, _orientation);
                }
            }
        }

        private int RetrieveTrackerPosition(Rectangle workingRect)
        {
            var currentTrackerPos = 0;

            if (Orientation == Orientation.Horizontal)
            {
                if (Maximum == Minimum)
                {
                    currentTrackerPos = workingRect.Left;
                }
                else
                {
                    currentTrackerPos = (((workingRect.Width - _buttonSize.Width) * (Value - Minimum)) / (Maximum - Minimum)) + workingRect.Left;
                }
            }
            else if (Orientation == Orientation.Vertical)
            {
                if (Maximum == Minimum)
                {
                    currentTrackerPos = workingRect.Top;
                }
                else
                {
                    currentTrackerPos = ((workingRect.Height - _buttonSize.Height) * (Value - Minimum)) / (Maximum - Minimum);
                }
            }

            return currentTrackerPos;
        }

        private void VerticalStyle(Graphics graphics, Rectangle rectangle, bool line)
        {
            Rectangle tickRectangle;
            _currentUsedPos = _indentWidth;
            Point location;
            Size size;
            _textAreaSize = GDI.MeasureText(graphics, Maximum.ToString(), _textFont);

            if (line)
            {
                if ((TickStyle == TickStyle.TopLeft) || (TickStyle == TickStyle.Both))
                {
                    // Retrieve tick barRectangle
                    location = new Point(_currentUsedPos + _textAreaSize.Width, _textAreaSize.Height / 2);
                    size = new Size(_tickHeight, _trackBarRectangle.Height - _textAreaSize.Height);
                    tickRectangle = new Rectangle(location, size);

                    // Enlarge tick barRectangle
                    // tickRectangle.Inflate(-buttonSize.Width / 2, 0);

                    // Move next tick area
                    _currentUsedPos += _tickHeight;

                    // Draw tick line
                    GDI.DrawTickLine(graphics, tickRectangle, TickFrequency, Minimum, Maximum, _tickColor, _orientation);
                }

                if ((TickStyle == TickStyle.BottomRight) || (TickStyle == TickStyle.Both))
                {
                    // Retrieve tick barRectangle
                    location = new Point(_trackBarRectangle.Right + _barTickSpacing, rectangle.Top + (_textAreaSize.Height / 2));
                    size = new Size(_tickHeight, _trackBarRectangle.Height - _textAreaSize.Height);
                    tickRectangle = new Rectangle(location, size);

                    // Enlarge tick barRectangle
                    // tickRectangle.Inflate(-buttonSize.Width / 2, 0);

                    // Move next tick area
                    _currentUsedPos += _tickHeight;

                    // Draw tick line
                    GDI.DrawTickLine(graphics, tickRectangle, TickFrequency, Minimum, Maximum, _tickColor, _orientation);
                }
            }
            else
            {
                if ((TickStyle == TickStyle.TopLeft) || (TickStyle == TickStyle.Both))
                {
                    // Retrieve text barRectangle
                    location = new Point(_currentUsedPos, _textAreaSize.Height / 2);
                    size = new Size(_textAreaSize.Width, _trackBarRectangle.Height - _textAreaSize.Height);
                    tickRectangle = new Rectangle(location, size);

                    // Enlarge text barRectangle
                    // tickRectangle.Inflate(-buttonSize.Width / 2, 0);

                    // Move next text area
                    _currentUsedPos += _tickHeight;

                    // Draw text 
                    GDI.DrawTickTextLine(graphics, tickRectangle, TickFrequency, Minimum, Maximum, _foreColor, _textFont, _orientation);
                }

                if ((TickStyle == TickStyle.BottomRight) || (TickStyle == TickStyle.Both))
                {
                    // Retrieve text barRectangle
                    location = new Point(_trackBarRectangle.Right + _barTickSpacing + _tickHeight, rectangle.Top + (_textAreaSize.Height / 2));
                    size = new Size(_textAreaSize.Width, rectangle.Height - _textAreaSize.Height);
                    tickRectangle = new Rectangle(location, size);

                    // Enlarge text barRectangle
                    // tickRectangle.Inflate(-buttonSize.Width / 2, 0);

                    // Move next text area
                    _currentUsedPos += _tickHeight;

                    // Draw text 
                    GDI.DrawTickTextLine(graphics, tickRectangle, TickFrequency, Minimum, Maximum, _foreColor, _textFont, _orientation);
                }
            }
        }

        #endregion
    }
}