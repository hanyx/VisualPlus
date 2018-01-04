namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.Localization;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("RatingChanged")]
    [DefaultProperty("Value")]
    [Description("The Visual Rating")]
    [Designer(typeof(VisualRatingDesigner))]
    [ToolboxBitmap(typeof(VisualRating), "Resources.ToolboxBitmaps.VisualRating.bmp")]
    [ToolboxItem(true)]
    public class VisualRating : VisualStyleBase
    {
        #region Variables

        private readonly BufferedGraphicsContext _bufferedContext;
        private BufferedGraphics _bufferedGraphics;
        private int _maximum;
        private float _mouseOverIndex;
        private StarType _ratingType;
        private bool _settingRating;
        private SolidBrush _starBrush;
        private SolidBrush _starDullBrush;
        private Pen _starDullStroke;
        private int _starSpacing;
        private Pen _starStroke;
        private int _starWidth;
        private bool _toggleHalfStar;
        private float _value;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.Controls.Interactivity.VisualRating" />
        ///     class.
        /// </summary>
        public VisualRating()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            _bufferedContext = BufferedGraphicsManager.Current;
            _toggleHalfStar = true;
            _maximum = 5;
            _mouseOverIndex = -1;
            _ratingType = StarType.Thick;
            _starBrush = new SolidBrush(Color.Yellow);
            _starDullStroke = new Pen(Color.Gray, 3f);
            _starDullBrush = new SolidBrush(Color.Silver);
            _starSpacing = 1;
            _starStroke = new Pen(Color.Gold, 3f);
            _starWidth = 25;
            SetPenBrushDefaults();
            Size = new Size(200, 100);
            UpdateGraphicsBuffer();
        }

        [Description("Occurs when the star rating of the strip has changed (Typically by a click operation)")]
        public event EventHandler RatingChanged;

        [Description("Occurs when a different number of stars are illuminated (does not include mouseleave un-ilum)")]
        public event EventHandler StarsPanned;

        public enum StarType
        {
            /// <summary>Default star.</summary>
            Default,

            /// <summary>Detailed star.</summary>
            Detailed,

            /// <summary>Thick star.</summary>
            Thick
        }

        #endregion

        #region Properties

        [Description("The number of stars to display")]
        [Category(PropertyCategory.Appearance)]
        [DefaultValue(5)]
        public int Maximum
        {
            get
            {
                return _maximum;
            }

            set
            {
                bool changed = _maximum != value;
                _maximum = value;

                if (changed)
                {
                    UpdateSize();
                    Invalidate();
                }
            }
        }

        [Browsable(false)]
        public float MouseOverStarIndex
        {
            get
            {
                return _mouseOverIndex;
            }
        }

        /// <summary>
        ///     Gets or sets the preset appearance of the star
        /// </summary>
        [Description("The star style to use")]
        [Category(PropertyCategory.Appearance)]
        [DefaultValue(StarType.Thick)]
        public StarType RatingType
        {
            get
            {
                return _ratingType;
            }

            set
            {
                _ratingType = value;
                Invalidate();
            }
        }

        [Description("The color to use for the star borders when they are illuminated")]
        [Category(PropertyCategory.Appearance)]
        [DefaultValue(typeof(Color), "Gold")]
        public Color StarBorderColor
        {
            get
            {
                return _starStroke.Color;
            }

            set
            {
                _starStroke.Color = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the width of the border around the star (including the dull version)
        /// </summary>
        [Description("The width of the star border")]
        [Category(PropertyCategory.Appearance)]
        [DefaultValue(3f)]
        public float StarBorderWidth
        {
            get
            {
                return _starStroke.Width;
            }

            set
            {
                _starStroke.Width = value;
                _starDullStroke.Width = value;
                UpdateSize();
                Invalidate();
            }
        }

        [Browsable(false)]
        public SolidBrush StarBrush
        {
            get
            {
                return _starBrush;
            }

            set
            {
                _starBrush = value;
            }
        }

        [Description("The color to use for the star when they are illuminated")]
        [Category(PropertyCategory.Appearance)]
        [DefaultValue(typeof(Color), "Yellow")]
        public Color StarColor
        {
            get
            {
                return _starBrush.Color;
            }

            set
            {
                _starBrush.Color = value;
                Invalidate();
            }
        }

        [Description("The color to use for the star borders when they are not illuminated")]
        [Category(PropertyCategory.Appearance)]
        [DefaultValue(typeof(Color), "Gray")]
        public Color StarDullBorderColor
        {
            get
            {
                return _starDullStroke.Color;
            }

            set
            {
                _starDullStroke.Color = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        public SolidBrush StarDullBrush
        {
            get
            {
                return _starDullBrush;
            }

            set
            {
                _starDullBrush = value;
            }
        }

        [Description("The color to use for the stars when they are not illuminated")]
        [Category(PropertyCategory.Appearance)]
        [DefaultValue(typeof(Color), "Silver")]
        public Color StarDullColor
        {
            get
            {
                return _starDullBrush.Color;
            }

            set
            {
                _starDullBrush.Color = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        public Pen StarDullStroke
        {
            get
            {
                return _starDullStroke;
            }

            set
            {
                _starDullStroke = value;
            }
        }

        [Description("The amount of space between each star")]
        [Category(PropertyCategory.Layout)]
        [DefaultValue(1)]
        public int StarSpacing
        {
            get
            {
                return _starSpacing;
            }

            set
            {
                _starSpacing = _starSpacing < 0 ? 0 : value;
                UpdateSize();
                Invalidate();
            }
        }

        [Browsable(false)]
        public Pen StarStroke
        {
            get
            {
                return _starStroke;
            }

            set
            {
                _starStroke = value;
            }
        }

        [Description("The width and height of the star in pixels (not including the border)")]
        [Category(PropertyCategory.Layout)]
        [DefaultValue(25)]
        public int StarWidth
        {
            get
            {
                return _starWidth;
            }

            set
            {
                _starWidth = _starWidth < 1 ? 1 : value;
                UpdateSize();
                Invalidate();
            }
        }

        [Description("Determines whether the user can rate with a half a star of specificity")]
        [Category(PropertyCategory.Behavior)]
        [DefaultValue(false)]
        public bool ToggleHalfStar
        {
            get
            {
                return _toggleHalfStar;
            }

            set
            {
                bool disabled = !value && _toggleHalfStar;
                _toggleHalfStar = value;

                if (disabled)
                {
                    // Only set rating if half star was enabled and now disabled
                    Value = (int)(Value + 0.5);
                }
            }
        }

        [Description("The number of stars selected (Note: 0 is considered un-rated")]
        [Category(PropertyCategory.Appearance)]
        [DefaultValue(0f)]
        public float Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (value > _maximum)
                {
                    value = _maximum;
                }
                else if (value < 0)
                {
                    value = 0;
                }
                else
                {
                    if (_toggleHalfStar)
                    {
                        value = RoundToNearestHalf(value);
                    }
                    else
                    {
                        value = (int)(value + 0.5f);
                    }
                }

                bool changed = value != _value;
                _value = value;

                if (changed)
                {
                    if (!_settingRating)
                    {
                        _mouseOverIndex = _value;
                        if (!_toggleHalfStar)
                        {
                            _mouseOverIndex -= 1f;
                        }
                    }

                    OnRatingChanged();
                    Invalidate();
                }
            }
        }

        /// <summary>Gets all of the spacing between the stars.</summary>
        private int TotalSpacing
        {
            get
            {
                return (_maximum - 1) * _starSpacing;
            }
        }

        /// <summary>Gets the sum of all star widths.</summary>
        private int TotalStarWidth
        {
            get
            {
                return _maximum * _starWidth;
            }
        }

        /// <summary>Gets the sum of the width of the stroke for each star.</summary>
        private float TotalStrokeWidth
        {
            get
            {
                return _maximum * _starStroke.Width;
            }
        }

        #endregion

        #region Events

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (_value == 0f)
            {
                _settingRating = true;
                Value = _toggleHalfStar ? _mouseOverIndex : _mouseOverIndex + 1f;
                _settingRating = false;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_value > 0)
            {
                return;
            }

            _mouseOverIndex = -1; // No stars will be highlighted
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_value > 0)
            {
                return;
            }

            float index = GetHoveredStarIndex(e.Location);

            if (index != _mouseOverIndex)
            {
                _mouseOverIndex = index;
                OnStarsPanned();
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _bufferedGraphics.Graphics.Clear(BackColor);
            DrawDullStars();
            DrawIlluminatedStars();
            _bufferedGraphics.Render(e.Graphics);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateSize();
            UpdateGraphicsBuffer();
        }

        /// <summary>Gets half of the detailed star polygon as a point[].</summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Star shape.</returns>
        private static PointF[] GetDetailedSemiStar(RectangleF rect)
        {
            return new[]
                {
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0f)),
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 1f)),
                    new PointF(rect.X + (rect.Width * 0.4f), rect.Y + (rect.Height * 0.73f)),
                    new PointF(rect.X + (rect.Width * 0.17f), rect.Y + (rect.Height * 0.83f)),
                    new PointF(rect.X + (rect.Width * 0.27f), rect.Y + (rect.Height * 0.6f)),
                    new PointF(rect.X + (rect.Width * 0f), rect.Y + (rect.Height * 0.5f)),
                    new PointF(rect.X + (rect.Width * 0.27f), rect.Y + (rect.Height * 0.4f)),
                    new PointF(rect.X + (rect.Width * 0.17f), rect.Y + (rect.Height * 0.17f)),
                    new PointF(rect.X + (rect.Width * 0.4f), rect.Y + (rect.Height * 0.27f))
                };
        }

        /// <summary>Gets a detailed star polygon as a point[].</summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Star shape.</returns>
        private static PointF[] GetDetailedStar(RectangleF rect)
        {
            return new[]
                {
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0f)),
                    new PointF(rect.X + (rect.Width * 0.6f), rect.Y + (rect.Height * 0.27f)),
                    new PointF(rect.X + (rect.Width * 0.83f), rect.Y + (rect.Height * 0.17f)),
                    new PointF(rect.X + (rect.Width * 0.73f), rect.Y + (rect.Height * 0.4f)),
                    new PointF(rect.X + (rect.Width * 1f), rect.Y + (rect.Height * 0.5f)),
                    new PointF(rect.X + (rect.Width * 0.73f), rect.Y + (rect.Height * 0.6f)),
                    new PointF(rect.X + (rect.Width * 0.83f), rect.Y + (rect.Height * 0.83f)),
                    new PointF(rect.X + (rect.Width * 0.6f), rect.Y + (rect.Height * 0.73f)),
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 1f)),
                    new PointF(rect.X + (rect.Width * 0.4f), rect.Y + (rect.Height * 0.73f)),
                    new PointF(rect.X + (rect.Width * 0.17f), rect.Y + (rect.Height * 0.83f)),
                    new PointF(rect.X + (rect.Width * 0.27f), rect.Y + (rect.Height * 0.6f)),
                    new PointF(rect.X + (rect.Width * 0f), rect.Y + (rect.Height * 0.5f)),
                    new PointF(rect.X + (rect.Width * 0.27f), rect.Y + (rect.Height * 0.4f)),
                    new PointF(rect.X + (rect.Width * 0.17f), rect.Y + (rect.Height * 0.17f)),
                    new PointF(rect.X + (rect.Width * 0.4f), rect.Y + (rect.Height * 0.27f))
                };
        }

        /// <summary>Gets half of a fat star polygon as a point[].</summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Star shape.</returns>
        private static PointF[] GetFatSemiStar(RectangleF rect)
        {
            return new[]
                {
                    new PointF(rect.X + (rect.Width * 0.31f), rect.Y + (rect.Height * 0.33f)),
                    new PointF(rect.X + (rect.Width * 0f), rect.Y + (rect.Height * 0.37f)),
                    new PointF(rect.X + (rect.Width * 0.25f), rect.Y + (rect.Height * 0.62f)),
                    new PointF(rect.X + (rect.Width * 0.19f), rect.Y + (rect.Height * 1f)),
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0.81f)),
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0f))
                };
        }

        /// <summary>Gets a fat star polygon as a point[].</summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Star shape.</returns>
        private static PointF[] GetFatStar(RectangleF rect)
        {
            return new[]
                {
                    new PointF(rect.X + (rect.Width * 0.31f), rect.Y + (rect.Height * 0.33f)),
                    new PointF(rect.X + (rect.Width * 0f), rect.Y + (rect.Height * 0.37f)),
                    new PointF(rect.X + (rect.Width * 0.25f), rect.Y + (rect.Height * 0.62f)),
                    new PointF(rect.X + (rect.Width * 0.19f), rect.Y + (rect.Height * 1f)),
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0.81f)),
                    new PointF(rect.X + (rect.Width * 0.81f), rect.Y + (rect.Height * 1f)),
                    new PointF(rect.X + (rect.Width * 0.75f), rect.Y + (rect.Height * 0.62f)),
                    new PointF(rect.X + (rect.Width * 1f), rect.Y + (rect.Height * 0.37f)),
                    new PointF(rect.X + (rect.Width * 0.69f), rect.Y + (rect.Height * 0.33f)),
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0f))
                };
        }

        /// <summary>Gets half of a typical thin star polygon as a point[].</summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Star shape.</returns>
        private static PointF[] GetNormalSemiStar(RectangleF rect)
        {
            return new[]
                {
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0f)),
                    new PointF(rect.X + (rect.Width * 0.38f), rect.Y + (rect.Height * 0.38f)),
                    new PointF(rect.X + (rect.Width * 0f), rect.Y + (rect.Height * 0.38f)),
                    new PointF(rect.X + (rect.Width * 0.31f), rect.Y + (rect.Height * 0.61f)),
                    new PointF(rect.X + (rect.Width * 0.19f), rect.Y + (rect.Height * 1f)),
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0.77f))
                };
        }

        /// <summary>Gets a typical thin star polygon as a point[].</summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Star shape.</returns>
        private static PointF[] GetNormalStar(RectangleF rect)
        {
            return new[]
                {
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0f)),
                    new PointF(rect.X + (rect.Width * 0.38f), rect.Y + (rect.Height * 0.38f)),
                    new PointF(rect.X + (rect.Width * 0f), rect.Y + (rect.Height * 0.38f)),
                    new PointF(rect.X + (rect.Width * 0.31f), rect.Y + (rect.Height * 0.61f)),
                    new PointF(rect.X + (rect.Width * 0.19f), rect.Y + (rect.Height * 1f)),
                    new PointF(rect.X + (rect.Width * 0.5f), rect.Y + (rect.Height * 0.77f)),
                    new PointF(rect.X + (rect.Width * 0.8f), rect.Y + (rect.Height * 1f)),
                    new PointF(rect.X + (rect.Width * 0.69f), rect.Y + (rect.Height * 0.61f)),
                    new PointF(rect.X + (rect.Width * 1f), rect.Y + (rect.Height * 0.38f)),
                    new PointF(rect.X + (rect.Width * 0.61f), rect.Y + (rect.Height * 0.38f))
                };
        }

        /// <summary>Rounds precise numbers to a number no more precise than .5.</summary>
        /// <param name="f">The value.</param>
        /// <returns>Star shape.</returns>
        private static float RoundToNearestHalf(float f)
        {
            return (float)Math.Round(f / 5.0, 1) * 5f;
        }

        private void DrawDullStars()
        {
            float height = Height - _starStroke.Width;
            float lastX = _starStroke.Width / 2f; // Start off at stroke size and increment
            float width = (Width - TotalSpacing - TotalStrokeWidth) / _maximum;

            // Draw stars
            for (var i = 0; i < _maximum; i++)
            {
                RectangleF rect = new RectangleF(lastX, _starStroke.Width / 2f, width, height);
                var polygon = GetStarPolygon(rect);
                _bufferedGraphics.Graphics.FillPolygon(_starDullBrush, polygon);
                _bufferedGraphics.Graphics.DrawPolygon(_starDullStroke, polygon);
                lastX += _starWidth + _starSpacing + _starStroke.Width;
                _bufferedGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                _bufferedGraphics.Graphics.FillPolygon(_starDullBrush, polygon);
                _bufferedGraphics.Graphics.DrawPolygon(_starDullStroke, polygon);
                _bufferedGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
            }
        }

        private void DrawIlluminatedStars()
        {
            float height = Height - _starStroke.Width;
            float lastX = _starStroke.Width / 2f; // Start off at stroke size and increment
            float width = (Width - TotalSpacing - TotalStrokeWidth) / _maximum;

            if (_toggleHalfStar)
            {
                // Draw stars
                for (var i = 0; i < _maximum; i++)
                {
                    RectangleF rect = new RectangleF(lastX, _starStroke.Width / 2f, width, height);

                    if (i < _mouseOverIndex - 0.5f)
                    {
                        var polygon = GetStarPolygon(rect);
                        _bufferedGraphics.Graphics.FillPolygon(_starBrush, polygon);
                        _bufferedGraphics.Graphics.DrawPolygon(_starStroke, polygon);
                    }
                    else if (i == _mouseOverIndex - 0.5f)
                    {
                        var polygon = GetSemiStarPolygon(rect);
                        _bufferedGraphics.Graphics.FillPolygon(_starBrush, polygon);
                        _bufferedGraphics.Graphics.DrawPolygon(_starStroke, polygon);
                    }
                    else
                    {
                        break;
                    }

                    lastX += _starWidth + _starSpacing + _starStroke.Width;
                }
            }
            else
            {
                // Draw stars
                for (var i = 0; i < _maximum; i++)
                {
                    RectangleF rect = new RectangleF(lastX, _starStroke.Width / 2f, width, height);
                    var polygon = GetStarPolygon(rect);

                    if (i <= _mouseOverIndex)
                    {
                        _bufferedGraphics.Graphics.FillPolygon(_starBrush, polygon);
                        _bufferedGraphics.Graphics.DrawPolygon(_starStroke, polygon);
                    }
                    else
                    {
                        break;
                    }

                    lastX += _starWidth + _starSpacing + _starStroke.Width;
                }
            }
        }

        private float GetHoveredStarIndex(Point pos)
        {
            if (_toggleHalfStar)
            {
                float widthSection = Width / (float)_maximum / 2f;

                for (var i = 0f; i < _maximum; i += 0.5f)
                {
                    float starX = i * widthSection * 2f;

                    // If cursor is within the x region of the iterated star
                    if ((pos.X >= starX) && (pos.X <= starX + widthSection))
                    {
                        return i + 0.5f;
                    }
                }

                return -1;
            }
            else
            {
                var widthSection = (int)((Width / (double)_maximum) + 0.5);

                for (var i = 0; i < _maximum; i++)
                {
                    float starX = i * widthSection;

                    // If cursor is within the x region of the iterated star
                    if ((pos.X >= starX) && (pos.X <= starX + widthSection))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private PointF[] GetSemiStarPolygon(RectangleF rect)
        {
            switch (_ratingType)
            {
                case StarType.Default: return GetNormalSemiStar(rect);
                case StarType.Thick: return GetFatSemiStar(rect);
                case StarType.Detailed: return GetDetailedSemiStar(rect);
                default: return null;
            }
        }

        private PointF[] GetStarPolygon(RectangleF rect)
        {
            switch (_ratingType)
            {
                case StarType.Default: return GetNormalStar(rect);
                case StarType.Thick: return GetFatStar(rect);
                case StarType.Detailed: return GetDetailedStar(rect);
                default: return null;
            }
        }

        private void OnRatingChanged()
        {
            RatingChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnStarsPanned()
        {
            StarsPanned?.Invoke(this, EventArgs.Empty);
        }

        private void SetPenBrushDefaults()
        {
            _starStroke.LineJoin = LineJoin.Round;
            _starStroke.Alignment = PenAlignment.Outset;
            _starDullStroke.LineJoin = LineJoin.Round;
            _starDullStroke.Alignment = PenAlignment.Outset;
        }

        private void UpdateGraphicsBuffer()
        {
            if ((Width > 0) && (Height > 0))
            {
                _bufferedContext.MaximumBuffer = new Size(Width + 1, Height + 1);
                _bufferedGraphics = _bufferedContext.Allocate(CreateGraphics(), ClientRectangle);
                _bufferedGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        private void UpdateSize()
        {
            var height = (int)(_starWidth + _starStroke.Width + 0.5);
            var width = (int)(TotalStarWidth + TotalSpacing + TotalStrokeWidth + 0.5);
            Size = new Size(width, height);
        }

        #endregion
    }
}