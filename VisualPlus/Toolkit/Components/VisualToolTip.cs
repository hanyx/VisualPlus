namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.Localization;
    using VisualPlus.Properties;
    using VisualPlus.Structure;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Popup")]
    [DefaultProperty("Text")]
    [Description("The Visual Tool Tip")]
    [Designer(typeof(VisualToolTipDesigner))]
    [ToolboxBitmap(typeof(VisualToolTip), "Resources.ToolboxBitmaps.VisualButton.bmp")]
    [ToolboxItem(true)]
    public class VisualToolTip : ToolTip
    {
        #region Variables

        private bool _autoSize;
        private Color _background;
        private Border _border;
        private Font _font;
        private Color _foreColor;
        private Image _icon;
        private bool _iconBorder;
        private GraphicsPath _iconGraphicsPath;
        private Point _iconPoint;
        private Rectangle _iconRectangle;
        private Size _iconSize;
        private Color _lineColor;
        private Padding _padding;
        private Rectangle _separator;
        private int _separatorThickness;
        private int _spacing;
        private StylesManager _styleManager;
        private string _text;
        private Point _textPoint;
        private TextRenderingHint _textRendererHint;
        private bool _textShadow;
        private string _title;
        private Color _titleColor;
        private Font _titleFont;
        private Point _titlePoint;
        private Size _toolTipSize;
        private ToolTipType _toolTipType;
        private int _xWidth;
        private int _yHeight;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualToolTip" /> class.</summary>
        public VisualToolTip()
        {
            _styleManager = new StylesManager(Settings.DefaultValue.DefaultStyle);
            _iconPoint = new Point(0, 0);
            _iconSize = new Size(24, 24);
            _padding = new Padding(4, 4, 4, 4);
            _separatorThickness = 1;
            _titleColor = Color.Gray;
            _toolTipSize = new Size(100, 40);
            _toolTipType = ToolTipType.Default;
            _spacing = 2;
            _title = "Title";
            _textRendererHint = Settings.DefaultValue.TextRenderingHint;
            _text = "Enter your custom text here.";
            _icon = Resources.VisualPlus;
            _background = _styleManager.Theme.ColorStateSettings.Enabled;
            _font = _styleManager.Theme.TextSetting.Font;
            _autoSize = true;
            _foreColor = _styleManager.Theme.TextSetting.Enabled;
            _lineColor = _styleManager.Theme.OtherSettings.Line;
            _titleFont = _styleManager.Theme.TextSetting.Font;

            _border = new Border();

            IsBalloon = false;
            OwnerDraw = true;
            Popup += VisualToolTip_Popup;
            Draw += VisualToolTip_Draw;
        }

        public enum ToolTipType
        {
            /// <summary>The default.</summary>
            Default = 0,

            /// <summary>The image.</summary>
            Image = 1,

            /// <summary>The text.</summary>
            Text = 2
        }

        #endregion

        #region Properties

        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.AutoSize)]
        public bool AutoSize
        {
            get
            {
                return _autoSize;
            }

            set
            {
                _autoSize = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color Background
        {
            get
            {
                return _background;
            }

            set
            {
                _background = value;
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
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Font)]
        public Font Font
        {
            get
            {
                return _font;
            }

            set
            {
                _font = value;
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
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Image)]
        public Image Icon
        {
            get
            {
                return _icon;
            }

            set
            {
                _icon = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Visible)]
        public bool IconBorder
        {
            get
            {
                return _iconBorder;
            }

            set
            {
                _iconBorder = value;
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Size)]
        public Size IconSize
        {
            get
            {
                return _iconSize;
            }

            set
            {
                _iconSize = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color LineColor
        {
            get
            {
                return _lineColor;
            }

            set
            {
                _lineColor = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Padding)]
        public Padding Padding
        {
            get
            {
                return _padding;
            }

            set
            {
                _padding = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Size)]
        public int SeparatorThickness
        {
            get
            {
                return _separatorThickness;
            }

            set
            {
                _separatorThickness = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Spacing)]
        public int Spacing
        {
            get
            {
                return _spacing;
            }

            set
            {
                _spacing = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Text)]
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.TextRenderingHint)]
        public TextRenderingHint TextRendering
        {
            get
            {
                return _textRendererHint;
            }

            set
            {
                _textRendererHint = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Visible)]
        public bool TextShadow
        {
            get
            {
                return _textShadow;
            }

            set
            {
                _textShadow = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Type)]
        public ToolTipType TipType
        {
            get
            {
                return _toolTipType;
            }

            set
            {
                _toolTipType = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Text)]
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TitleColor
        {
            get
            {
                return _titleColor;
            }

            set
            {
                _titleColor = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Font)]
        public Font TitleFont
        {
            get
            {
                return _titleFont;
            }

            set
            {
                _titleFont = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Size)]
        public Size ToolTipSize
        {
            get
            {
                return _toolTipSize;
            }

            set
            {
                _toolTipSize = value;
            }
        }

        #endregion

        #region Events

        /// <summary>Input the text height to compare it to the icon height.</summary>
        /// <param name="textHeight">The text height.</param>
        /// <returns>The <see cref="int" />.</returns>
        private int GetTipHeight(int textHeight)
        {
            int tipHeight = textHeight > _iconSize.Height ? textHeight : _iconSize.Height;
            return tipHeight;
        }

        /// <summary>Input the title and text width to retrieve total width.</summary>
        /// <param name="titleWidth">The title width.</param>
        /// <param name="textWidth">The text width.</param>
        /// <returns>The <see cref="int" />.</returns>
        private int GetTipWidth(int titleWidth, int textWidth)
        {
            int tipWidth = titleWidth > _iconSize.Width + textWidth ? titleWidth : _iconSize.Width + textWidth;
            return tipWidth;
        }

        private void VisualToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = _textRendererHint;
            graphics.FillRectangle(new SolidBrush(_background), e.Bounds);

            if (_border.Visible)
            {
                Rectangle boxRectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                GraphicsPath borderPath = new GraphicsPath();
                borderPath.AddRectangle(boxRectangle);
                graphics.DrawPath(new Pen(_border.Color, _border.Thickness), borderPath);
            }

            if ((_textShadow && (_toolTipType == ToolTipType.Text)) || (_textShadow && (_toolTipType == ToolTipType.Default)))
            {
                // Draw shadow text
                graphics.DrawString(_text, new Font(Font, FontStyle.Regular), Brushes.Silver, new PointF(_textPoint.X + 1, _textPoint.Y + 1));
            }

            switch (_toolTipType)
            {
                case ToolTipType.Default:
                    {
                        // Draw the title
                        graphics.DrawString(_title, _titleFont, new SolidBrush(_titleColor), new PointF(_titlePoint.X, _titlePoint.Y));

                        // Draw the separator
                        graphics.DrawLine(new Pen(_lineColor), _separator.X, _separator.Y, _separator.Width, _separator.Y);

                        // Draw the text
                        graphics.DrawString(_text, Font, new SolidBrush(_foreColor), new PointF(_textPoint.X, _textPoint.Y));

                        if (Icon != null)
                        {
                            // Update point
                            _iconRectangle.Location = _iconPoint;

                            // Draw icon border
                            if (_iconBorder)
                            {
                                graphics.DrawPath(new Pen(_border.Color), _iconGraphicsPath);
                            }

                            // Draw icon
                            graphics.DrawImage(Icon, _iconRectangle);
                        }

                        break;
                    }

                case ToolTipType.Image:
                    {
                        if (Icon != null)
                        {
                            // Update point
                            _iconRectangle.Location = _iconPoint;

                            // Draw icon border
                            if (_iconBorder)
                            {
                                graphics.DrawPath(new Pen(_border.Color), _iconGraphicsPath);
                            }

                            // Draw icon
                            graphics.DrawImage(Icon, _iconRectangle);
                        }

                        break;
                    }

                case ToolTipType.Text:
                    {
                        // Draw the text
                        graphics.DrawString(_text, Font, new SolidBrush(_foreColor), new PointF(_textPoint.X, _textPoint.Y));
                        break;
                    }
            }
        }

        private void VisualToolTip_Popup(object sender, PopupEventArgs e)
        {
            switch (_toolTipType)
            {
                case ToolTipType.Default:
                    {
                        if (!_autoSize)
                        {
                            _xWidth = _toolTipSize.Width;
                            _yHeight = _toolTipSize.Height;
                        }
                        else
                        {
                            _xWidth = GetTipWidth(TextRenderer.MeasureText(_title, Font).Width, TextRenderer.MeasureText(_text, Font).Width);
                            _yHeight = TextRenderer.MeasureText(_title, Font).Height + SeparatorThickness + GetTipHeight(TextRenderer.MeasureText(_text, Font).Height);
                        }

                        _titlePoint.X = _padding.Left;
                        _titlePoint.Y = _padding.Top;

                        Point separatorPoint = new Point(_padding.Left + Spacing, TextRenderer.MeasureText(_title, Font).Height + 5);
                        Size separatorSize = new Size(_xWidth, SeparatorThickness);
                        _separator = new Rectangle(separatorPoint, separatorSize);

                        _textPoint.X = _padding.Left + _iconSize.Width + Spacing;
                        _textPoint.Y = _separator.Y + Spacing;

                        _iconPoint = new Point(_padding.Left, _textPoint.Y);
                        break;
                    }

                case ToolTipType.Image:
                    {
                        _iconPoint = new Point(_padding.Left, _padding.Top);
                        _xWidth = _iconSize.Width + 1;
                        _yHeight = _iconSize.Height + 1;
                        break;
                    }

                case ToolTipType.Text:
                    {
                        _textPoint = new Point(_padding.Left, _padding.Top);
                        _xWidth = TextRenderer.MeasureText(_text, Font).Width;
                        _yHeight = TextRenderer.MeasureText(_text, Font).Height;
                        break;
                    }
            }

            // Create icon rectangle
            _iconRectangle = new Rectangle(_iconPoint, _iconSize);

            // Create icon path
            _iconGraphicsPath = new GraphicsPath();
            _iconGraphicsPath.AddRectangle(_iconRectangle);
            _iconGraphicsPath.CloseAllFigures();

            // Initialize new size
            e.ToolTipSize = new Size(_padding.Left + _xWidth + _padding.Right, _padding.Top + _yHeight + _padding.Bottom);
        }

        #endregion
    }
}