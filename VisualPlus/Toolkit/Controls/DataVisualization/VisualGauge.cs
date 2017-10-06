namespace VisualPlus.Toolkit.Controls.DataVisualization
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
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ProgressBar))]
    [DefaultEvent("Click")]
    [DefaultProperty("Value")]
    [Description("The Visual Gauge")]
    public class VisualGauge : ProgressBase
    {
        #region Variables

        private ColorState _colorState;

        private Label _labelMaximum;
        private Label _labelMinimum;
        private Label _labelProgress;
        private Color _progress;

        private Size _progressTextSize;
        private int _thickness;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.Controls.Layout.VisualGauge" /> class.</summary>
        public VisualGauge()
        {
            _thickness = 25;
            Maximum = 100;

            _colorState = new ColorState();

            ConstructDisplay();
            Controls.Add(_labelMaximum);
            Controls.Add(_labelMinimum);
            Controls.Add(_labelProgress);

            Margin = new Padding(6);
            Size = new Size(174, 117);

            UpdateTheme(StyleManager.Style);
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
                _colorState = value;
                Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Green")]
        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color Progress
        {
            get
            {
                return _progress;
            }

            set
            {
                _progress = value;
                Invalidate();
            }
        }

        [DefaultValue(30)]
        [Category(Propertys.Layout)]
        [Description(Property.Thickness)]
        public int Thickness
        {
            get
            {
                return _thickness;
            }

            set
            {
                _thickness = value;
                Invalidate();
            }
        }

        [DefaultValue(0)]
        [Category(Propertys.Behavior)]
        public new int Value
        {
            get
            {
                return base.Value;
            }

            set
            {
                base.Value = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        public void UpdateTheme(Styles style)
        {
            StyleManager = new VisualStyleManager(style);

            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;

            BackColorState.Enabled = StyleManager.ControlStyle.Background(3);
            BackColorState.Disabled = StyleManager.ColorStateStyle.ControlDisabled;

            _progress = StyleManager.ProgressStyle.Progress;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _progressTextSize = GDI.MeasureText(e.Graphics, _labelProgress.Text + @"%", Font);
            _labelProgress.Location = new Point((Width / 2) - (_progressTextSize.Width / 2), Height - _progressTextSize.Height - 30);

            Graphics _graphics = e.Graphics;
            _graphics.Clear(Parent.BackColor);
            _graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            _graphics.SmoothingMode = SmoothingMode.HighQuality;

            Color _backColor = Enabled ? BackColorState.Enabled : BackColorState.Disabled;

            Pen _penBackground = new Pen(_backColor, _thickness);
            int _width = Size.Width - (_thickness * 2);
            Rectangle _rectangle = new Rectangle(_thickness, Size.Height / 4, _width, _width);
            Pen _penProgress = new Pen(_progress, _thickness);

            _graphics.DrawArc(_penBackground, _rectangle, 180F, 180F);
            _graphics.DrawArc(_penProgress, _rectangle, 180F, MathManager.GetHalfRadianAngle(Value));

            _labelProgress.Text = Value + @"%";
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            _labelMinimum.Top = _labelMaximum.Top = Height - _labelMaximum.Height - 10;
            _labelMinimum.Left = 20;
            _labelMaximum.Left = Size.Width - _labelMaximum.Width - 20;
        }

        private void ConstructDisplay()
        {
            _labelProgress = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    Location = new Point(83, 34),
                    Margin = new Padding(6, 0, 6, 0),
                    Name = "visualLabelProgress",
                    Size = new Size(22, 24),
                    TabIndex = 1,
                    Text = @"0",
                    BackColor = Color.Transparent
                };

            _labelMinimum = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    Location = new Point(26, 86),
                    Margin = new Padding(6, 0, 6, 0),
                    Name = "visualLabelMinimum",
                    Size = new Size(15, 17),
                    TabIndex = 2,
                    Text = @"0",
                    BackColor = Color.Transparent
                };

            _labelMaximum = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    Location = new Point(145, 86),
                    Margin = new Padding(6, 0, 6, 0),
                    Name = "visualLabelMaximum",
                    Size = new Size(29, 17),
                    TabIndex = 3,
                    Text = @"100",
                    BackColor = Color.Transparent
                };
        }

        #endregion
    }
}