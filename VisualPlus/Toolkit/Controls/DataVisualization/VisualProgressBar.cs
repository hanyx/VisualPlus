namespace VisualPlus.Toolkit.Controls.DataVisualization
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("Value")]
    [Description("The Visual ProgressBar")]
    [Designer(typeof(VisualProgressBarDesigner))]
    [ToolboxBitmap(typeof(VisualProgressBar), "Resources.ToolboxBitmaps.VisualProgressBar.bmp")]
    [ToolboxItem(true)]
    public class VisualProgressBar : ProgressBase
    {
        #region Variables

        private Border _border;
        private ColorState _colorState;
        private Hatch _hatch;
        private Timer _marqueeTimer;
        private bool _marqueeTimerEnabled;
        private int _marqueeWidth;
        private int _marqueeX;
        private int _marqueeY;
        private Orientation _orientation;
        private bool _percentageVisible;
        private ProgressBarStyle _progressBarStyle;
        private Color _progressColor;
        private Image _progressImage;
        private StringAlignment _valueAlignment;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualProgressBar" /> class.</summary>
        public VisualProgressBar()
        {
            Maximum = 100;
            _hatch = new Hatch();
            _orientation = Orientation.Horizontal;
            _marqueeWidth = 20;
            Size = new Size(100, 20);
            _percentageVisible = true;
            _border = new Border();
            _progressBarStyle = ProgressBarStyle.Blocks;
            _valueAlignment = StringAlignment.Center;

            UpdateTheme(ThemeManager.Theme);
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(ColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(PropertyCategory.Appearance)]
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
        [Category(PropertyCategory.Appearance)]
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

        [TypeConverter(typeof(HatchConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(PropertyCategory.Behavior)]
        public Hatch Hatch
        {
            get
            {
                return _hatch;
            }

            set
            {
                _hatch = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Size)]
        public int MarqueeWidth
        {
            get
            {
                return _marqueeWidth;
            }

            set
            {
                _marqueeWidth = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Orientation)]
        public Orientation Orientation
        {
            get
            {
                return _orientation;
            }

            set
            {
                _orientation = value;

                if (_orientation == Orientation.Horizontal)
                {
                    if (Width < Height)
                    {
                        int temp = Width;
                        Width = Height;
                        Height = temp;
                    }
                }
                else
                {
                    // Vertical
                    if (Width > Height)
                    {
                        int temp = Width;
                        Width = Height;
                        Height = temp;
                    }
                }

                // Resize check
                OnResize(EventArgs.Empty);

                Invalidate();
            }
        }

        [DefaultValue(Settings.DefaultValue.TextVisible)]
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Visible)]
        public bool PercentageVisible
        {
            get
            {
                return _percentageVisible;
            }

            set
            {
                _percentageVisible = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Image)]
        public Image ProgressImage
        {
            get
            {
                return _progressImage;
            }

            set
            {
                _progressImage = value;
                Invalidate();
            }
        }

        [DefaultValue(typeof(ProgressBarStyle))]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.ProgressBarStyle)]
        public ProgressBarStyle Style
        {
            get
            {
                return _progressBarStyle;
            }

            set
            {
                _progressBarStyle = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Alignment)]
        public StringAlignment ValueAlignment
        {
            get
            {
                return _valueAlignment;
            }

            set
            {
                _valueAlignment = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        public void UpdateTheme(Theme theme)
        {
            try
            {
                _colorState = new ColorState
                    {
                        Enabled = theme.OtherSettings.ProgressBackground,
                        Disabled = theme.OtherSettings.ProgressDisabled
                    };

                _hatch.BackColor = Color.FromArgb(0, theme.OtherSettings.HatchBackColor);
                _hatch.ForeColor = Color.FromArgb(20, theme.OtherSettings.HatchForeColor);

                _progressColor = theme.OtherSettings.Progress;

                _border.Color = theme.BorderSettings.Normal;
                _border.HoverColor = theme.BorderSettings.Hover;

                ForeColor = theme.TextSetting.Enabled;
                TextStyle.Enabled = theme.TextSetting.Enabled;
                TextStyle.Disabled = theme.TextSetting.Disabled;

                Font = theme.TextSetting.Font;

                BackColorState.Enabled = theme.ColorStateSettings.Enabled;
                BackColorState.Disabled = theme.ColorStateSettings.Disabled;
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }

            Invalidate();
            OnThemeChanged(new ThemeEventArgs(theme));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics _graphics = e.Graphics;
            _graphics.Clear(Parent.BackColor);
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            _graphics.TextRenderingHint = TextStyle.TextRenderingHint;
            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

            ControlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);

            _graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, ClientRectangle.Width + 1, ClientRectangle.Height + 1));
            _graphics.SetClip(ControlGraphicsPath);

            Color _backColor = Enabled ? BackColorState.Enabled : BackColorState.Disabled;
            VisualBackgroundRenderer.DrawBackground(_graphics, _backColor, BackgroundImage, MouseState, _clientRectangle, _border);

            DrawProgress(_orientation, _graphics);
            VisualBorderRenderer.DrawBorderStyle(e.Graphics, _border, ControlGraphicsPath, MouseState);
            e.Graphics.ResetClip();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(BackColor);
        }

        private void DrawProgress(Orientation orientation, Graphics graphics)
        {
            if (_progressBarStyle == ProgressBarStyle.Marquee)
            {
                if (!DesignMode && Enabled)
                {
                    StartTimer();
                }

                if (!Enabled)
                {
                    StopTimer();
                }

                if (Value == Maximum)
                {
                    StopTimer();
                    DrawProgressContinuous(graphics);
                }
                else
                {
                    DrawProgressMarquee(graphics);
                }
            }
            else
            {
                int _indexValue;

                GraphicsPath _progressPath;
                Rectangle _progressRectangle;

                switch (orientation)
                {
                    case Orientation.Horizontal:
                        {
                            _indexValue = (int)Math.Round(((Value - Minimum) / (double)(Maximum - Minimum)) * (Width - 2));
                            _progressRectangle = new Rectangle(0, 0, _indexValue + _border.Thickness, Height);
                            _progressPath = VisualBorderRenderer.CreateBorderTypePath(_progressRectangle, _border);
                        }

                        break;
                    case Orientation.Vertical:
                        {
                            _indexValue = (int)Math.Round(((Value - Minimum) / (double)(Maximum - Minimum)) * (Height - 2));
                            _progressRectangle = new Rectangle(0, Height - _indexValue - _border.Thickness - 1, Width, _indexValue);
                            _progressPath = VisualBorderRenderer.CreateBorderTypePath(_progressRectangle, _border);
                        }

                        break;
                    default:
                        {
                            throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
                        }
                }

                if (_indexValue > 1)
                {
                    graphics.SetClip(ControlGraphicsPath);
                    graphics.FillRectangle(new SolidBrush(_progressColor), _progressRectangle);
                    VisualControlRenderer.DrawHatch(graphics, _hatch, _progressPath);
                    graphics.ResetClip();
                }
            }

            DrawText(graphics);
        }

        private void DrawProgressContinuous(Graphics graphics)
        {
            GraphicsPath _progressPath = new GraphicsPath();
            _progressPath.AddRectangle(new Rectangle(0, 0, (Value / Maximum) * ClientRectangle.Width, ClientRectangle.Height));
            graphics.FillPath(new SolidBrush(_progressColor), _progressPath);
        }

        private void DrawProgressMarquee(Graphics graphics)
        {
            Rectangle _progressRectangle;

            switch (_orientation)
            {
                case Orientation.Horizontal:
                    {
                        _progressRectangle = new Rectangle(_marqueeX, 0, _marqueeWidth, ClientRectangle.Height);
                        break;
                    }

                case Orientation.Vertical:
                    {
                        _progressRectangle = new Rectangle(0, _marqueeY, ClientRectangle.Width, ClientRectangle.Height);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            GraphicsPath _progressPath = new GraphicsPath();
            _progressPath.AddRectangle(_progressRectangle);
            graphics.SetClip(ControlGraphicsPath);
            graphics.FillPath(new SolidBrush(_progressColor), _progressPath);
            VisualControlRenderer.DrawHatch(graphics, _hatch, _progressPath);
            graphics.ResetClip();
        }

        private void DrawText(Graphics graphics)
        {
            // Draw value as a string
            string percentValue = Convert.ToString(Convert.ToInt32(Value)) + "%";

            // Toggle percentage
            if (_percentageVisible)
            {
                StringFormat stringFormat = new StringFormat
                    {
                        Alignment = _valueAlignment,
                        LineAlignment = StringAlignment.Center
                    };

                graphics.DrawString(
                    percentValue,
                    Font,
                    new SolidBrush(ForeColor),
                    new Rectangle(0, 0, Width, Height + 2),
                    stringFormat);
            }
        }

        private void MarqueeTimer_Tick(object sender, EventArgs e)
        {
            switch (_orientation)
            {
                case Orientation.Horizontal:
                    {
                        _marqueeX++;
                        if (_marqueeX > ClientRectangle.Width)
                        {
                            _marqueeX = -_marqueeWidth;
                        }

                        break;
                    }

                case Orientation.Vertical:
                    {
                        _marqueeY++;
                        if (_marqueeY > ClientRectangle.Height)
                        {
                            _marqueeY = -_marqueeWidth;
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

        private void StartTimer()
        {
            if (_marqueeTimerEnabled)
            {
                return;
            }

            if (_marqueeTimer == null)
            {
                _marqueeTimer = new Timer { Interval = 10 };
                _marqueeTimer.Tick += MarqueeTimer_Tick;
            }

            switch (_orientation)
            {
                case Orientation.Horizontal:
                    {
                        _marqueeX = -_marqueeWidth;
                        break;
                    }

                case Orientation.Vertical:
                    {
                        _marqueeY = -_marqueeWidth;
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            _marqueeTimer.Stop();
            _marqueeTimer.Start();

            _marqueeTimerEnabled = true;

            Invalidate();
        }

        private void StopTimer()
        {
            if (_marqueeTimer == null)
            {
                return;
            }

            _marqueeTimer.Stop();

            Invalidate();
        }

        #endregion
    }
}