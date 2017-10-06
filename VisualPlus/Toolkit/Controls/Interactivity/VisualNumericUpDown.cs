namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(NumericUpDown))]
    [DefaultEvent("Click")]
    [DefaultProperty("Value")]
    [Description("The Visual NumericUpDown")]
    [Designer(ControlManager.FilterProperties.VisualNumericUpDown)]
    public class VisualNumericUpDown : VisualControlBase
    {
        #region Variables

        private Border _border;
        private BorderEdge _borderButtons;

        private BorderEdge _borderEdge;
        private Color _buttonColor;
        private Font _buttonFont;
        private Color _buttonForeColor;
        private Orientation _buttonOrientation;
        private GraphicsPath _buttonPath;
        private Rectangle _buttonRectangle;
        private int _buttonWidth;
        private ColorState _colorState;
        private Point[] _decrementButtonPoints;
        private Point[] _incrementButtonPoints;
        private bool _keyboardNum;
        private long _maximumValue;
        private long _minimumValue;
        private long _numericValue;
        private int _xValue;
        private int _yValue;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="T:VisualPlus.Toolkit.Controls.Interactivity.VisualNumericUpDown" /> class.
        /// </summary>
        public VisualNumericUpDown()
        {
            _decrementButtonPoints = new Point[2];
            _incrementButtonPoints = new Point[2];
            _buttonWidth = 50;

            _borderEdge = new BorderEdge();
            _borderButtons = new BorderEdge();

            _minimumValue = 0;
            _maximumValue = 100;
            Size = new Size(125, 25);
            MinimumSize = new Size(0, 0);
            _colorState = new ColorState();
            _buttonOrientation = Orientation.Horizontal;

            _border = new Border();

            Controls.Add(_borderEdge);
            Controls.Add(_borderButtons);

            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(ColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorState BackColorState
        {
            get
            {
                return _colorState;
            }

            set
            {
                if (value == _colorState)
                {
                    return;
                }

                _colorState = value;
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

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ButtonColor
        {
            get
            {
                return _buttonColor;
            }

            set
            {
                _buttonColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Font)]
        public Font ButtonFont
        {
            get
            {
                return _buttonFont;
            }

            set
            {
                _buttonFont = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ButtonForeColor
        {
            get
            {
                return _buttonForeColor;
            }

            set
            {
                _buttonForeColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Alignment)]
        public Orientation ButtonOrientation
        {
            get
            {
                return _buttonOrientation;
            }

            set
            {
                _buttonOrientation = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public int ButtonWidth
        {
            get
            {
                return _buttonWidth;
            }

            set
            {
                _buttonWidth = value;
                Invalidate();
            }
        }

        [Category(Propertys.Behavior)]
        public long MaximumValue
        {
            get
            {
                return _maximumValue;
            }

            set
            {
                if (value > _minimumValue)
                {
                    _maximumValue = value;
                }

                if (_numericValue > _maximumValue)
                {
                    _numericValue = _maximumValue;
                }

                Invalidate();
            }
        }

        [Category(Propertys.Behavior)]
        public long MinimumValue
        {
            get
            {
                return _minimumValue;
            }

            set
            {
                if (value < _maximumValue)
                {
                    _minimumValue = value;
                }

                if (_numericValue < _minimumValue)
                {
                    _numericValue = MinimumValue;
                }

                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color Separator
        {
            get
            {
                return _borderEdge.BackColor;
            }

            set
            {
                _borderEdge.BackColor = value;
                _borderButtons.BackColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Behavior)]
        public long Value
        {
            get
            {
                return _numericValue;
            }

            set
            {
                if ((value <= _maximumValue) & (value >= _minimumValue))
                {
                    _numericValue = value;
                }

                Invalidate();
            }
        }

        #endregion

        #region Events

        public void Decrement(int value)
        {
            _numericValue -= value;
            Invalidate();
        }

        public void Increment(int value)
        {
            _numericValue += value;
            Invalidate();
        }

        public void UpdateTheme(Styles style)
        {
            StyleManager = new VisualStyleManager(style);

            _buttonForeColor = Color.Gray;
            _buttonFont = new Font(StyleManager.Font.FontFamily, 14, FontStyle.Bold);
            _buttonColor = StyleManager.ControlStyle.Background(3);

            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;

            _colorState.Enabled = StyleManager.ControlStyle.Background(3);
            _colorState.Disabled = StyleManager.ControlStyle.Background(0);

            _borderButtons.BackColor = StyleManager.ControlStyle.Line;
            _borderEdge.BackColor = StyleManager.ControlStyle.Line;

            _border.Color = StyleManager.ShapeStyle.Color;
            _border.HoverColor = StyleManager.BorderStyle.HoverColor;

            Invalidate();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            try
            {
                if (_keyboardNum)
                {
                    _numericValue = long.Parse(_numericValue + e.KeyChar.ToString());
                }

                if (_numericValue > _maximumValue)
                {
                    _numericValue = _maximumValue;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Back)
            {
                string temporaryValue = _numericValue.ToString();
                temporaryValue = temporaryValue.Remove(Convert.ToInt32(temporaryValue.Length - 1));
                if (temporaryValue.Length == 0)
                {
                    temporaryValue = "0";
                }

                _numericValue = Convert.ToInt32(temporaryValue);
            }

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnMouseClick(e);

            switch (_buttonOrientation)
            {
                case Orientation.Vertical:
                    {
                        // Check if mouse in X position.
                        if ((_xValue > Width - _buttonRectangle.Width) && (_xValue < Width))
                        {
                            // Determine the button middle separator by checking for the Y position.
                            if ((_yValue > _buttonRectangle.Y) && (_yValue < Height / 2))
                            {
                                if (Value + 1 <= _maximumValue)
                                {
                                    _numericValue++;
                                }
                            }
                            else if ((_yValue > Height / 2) && (_yValue < Height))
                            {
                                if (Value - 1 >= _minimumValue)
                                {
                                    _numericValue--;
                                }
                            }
                        }
                        else
                        {
                            _keyboardNum = !_keyboardNum;
                            Focus();
                        }

                        break;
                    }

                case Orientation.Horizontal:
                    {
                        // Check if mouse in X position.
                        if ((_xValue > Width - _buttonRectangle.Width) && (_xValue < Width))
                        {
                            // Determine the button middle separator by checking for the X position.
                            if ((_xValue > _buttonRectangle.X) && (_xValue < _buttonRectangle.X + (_buttonRectangle.Width / 2)))
                            {
                                if (Value + 1 <= _maximumValue)
                                {
                                    _numericValue++;
                                }
                            }
                            else if ((_xValue > _buttonRectangle.X + (_buttonRectangle.Width / 2)) && (_xValue < Width))
                            {
                                if (Value - 1 >= _minimumValue)
                                {
                                    _numericValue--;
                                }
                            }
                        }
                        else
                        {
                            _keyboardNum = !_keyboardNum;
                            Focus();
                        }

                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _xValue = e.Location.X;
            _yValue = e.Location.Y;
            Invalidate();

            // IBeam cursor toggle
            if (e.X < _buttonRectangle.X)
            {
                Cursor = Cursors.IBeam;
            }
            else
            {
                Cursor = Cursors.Hand;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                if (Value + 1 <= _maximumValue)
                {
                    _numericValue++;
                }

                Invalidate();
            }
            else
            {
                if (Value - 1 >= _minimumValue)
                {
                    _numericValue--;
                }

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics _graphics = e.Graphics;
            _graphics.Clear(Parent.BackColor);
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.TextRenderingHint = TextRenderingHint;

            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            ControlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);

            _graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, ClientRectangle.Width + 1, ClientRectangle.Height + 1));
            _graphics.SetClip(ControlGraphicsPath);

            _buttonRectangle = new Rectangle(Width - _buttonWidth, 1, _buttonWidth, Height);

            Size incrementSize = GDI.MeasureText(_graphics, "+", _buttonFont);
            Size decrementSize = GDI.MeasureText(_graphics, "-", _buttonFont);

            _incrementButtonPoints[0] = new Point((_buttonRectangle.X + (_buttonRectangle.Width / 2)) - (incrementSize.Width / 2), (_buttonRectangle.Y + (_buttonRectangle.Height / 2)) - (_buttonRectangle.Height / 4) - (incrementSize.Height / 2));
            _decrementButtonPoints[0] = new Point((_buttonRectangle.X + (_buttonRectangle.Width / 2)) - (decrementSize.Width / 2), (_buttonRectangle.Y + (_buttonRectangle.Height / 2) + (_buttonRectangle.Height / 4)) - (decrementSize.Height / 2));

            _incrementButtonPoints[1] = new Point((_buttonRectangle.X + (_buttonRectangle.Width / 4)) - (incrementSize.Width / 2), (Height / 2) - (incrementSize.Height / 2));
            _decrementButtonPoints[1] = new Point((_buttonRectangle.X + (_buttonRectangle.Width / 2) + (_buttonRectangle.Width / 4)) - (decrementSize.Width / 2), (Height / 2) - (decrementSize.Height / 2));

            int toggleInt;
            switch (_buttonOrientation)
            {
                case Orientation.Horizontal:
                    {
                        toggleInt = 1;
                        _borderButtons.Location = new Point(_buttonRectangle.X + (_buttonRectangle.Width / 2), _border.Thickness);
                        _borderButtons.Size = new Size(1, Height - _border.Thickness - 1);
                        break;
                    }

                case Orientation.Vertical:
                    {
                        toggleInt = 0;
                        _borderButtons.Location = new Point(_buttonRectangle.X, (_buttonRectangle.Bottom / 2) - _border.Thickness);
                        _borderButtons.Size = new Size(Width - _border.Thickness - 1, 1);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            _buttonPath = new GraphicsPath();
            _buttonPath.AddRectangle(_buttonRectangle);
            _buttonPath.CloseAllFigures();

            Color _backColor = Enabled ? BackColorState.Enabled : BackColorState.Disabled;
            VisualBackgroundRenderer.DrawBackground(e.Graphics, _backColor, BackgroundImage, MouseState, _clientRectangle, Border);

            _graphics.SetClip(ControlGraphicsPath);
            _graphics.FillRectangle(new SolidBrush(_buttonColor), _buttonRectangle);
            _graphics.ResetClip();

            _graphics.DrawString("+", _buttonFont, new SolidBrush(_buttonForeColor), _incrementButtonPoints[toggleInt]);
            _graphics.DrawString("-", _buttonFont, new SolidBrush(_buttonForeColor), _decrementButtonPoints[toggleInt]);

            _borderEdge.Location = new Point(_buttonRectangle.X, _border.Thickness);
            _borderEdge.Size = new Size(1, Height - _border.Thickness - 1);

            DrawText(_graphics);

            VisualBorderRenderer.DrawBorderStyle(e.Graphics, _border, ControlGraphicsPath, MouseState);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(BackColor);
        }

        private void DrawText(Graphics _graphics)
        {
            Rectangle textBoxRectangle = new Rectangle(6, 0, Width - 1, Height - 1);
            StringFormat stringFormat = new StringFormat
                {
                    // Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
            _graphics.DrawString(Convert.ToString(Value), Font, new SolidBrush(ForeColor), textBoxRectangle, stringFormat);
        }

        #endregion
    }
}