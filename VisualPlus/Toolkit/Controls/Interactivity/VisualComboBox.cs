namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Items")]
    [Description("The Visual ComboBox")]
    [ToolboxBitmap(typeof(ComboBox), "Resources.ToolboxBitmaps.VisualComboBox.bmp")]
    [ToolboxItem(true)]
    public class VisualComboBox : ComboBox
    {
        #region Variables

        private ColorState _backColorState;
        private Border _border;
        private BorderEdge _borderEdge;
        private Color _buttonColor;
        private Image _buttonImage;
        private ButtonStyles _buttonStyle;
        private bool _buttonVisible;
        private int _buttonWidth;
        private GraphicsPath _controlGraphicsPath;
        private Color _foreColor;
        private ImageList _imageList;
        private bool _imageVisible;
        private int _index;
        private bool _itemImageVisible;
        private Color _menuItemHover;
        private Color _menuItemNormal;
        private Color _menuTextColor;
        private MouseStates _mouseState;
        private VisualStyleManager _styleManager;
        private StringAlignment _textAlignment;
        private Color _textDisabledColor;
        private TextImageRelation _textImageRelation;
        private StringAlignment _textLineAlignment;
        private TextRenderingHint _textRendererHint;
        private Watermark _watermark;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.Controls.Interactivity.VisualComboBox" />
        ///     class.
        /// </summary>
        public VisualComboBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor,
                true);

            SetStyle((ControlStyles)139286, true);
            SetStyle(ControlStyles.Selectable, false);

            _styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);
            _textImageRelation = TextImageRelation.ImageBeforeText;
            _textAlignment = StringAlignment.Center;
            _textLineAlignment = StringAlignment.Center;
            _itemImageVisible = true;
            _imageVisible = false;
            _buttonWidth = 30;
            _buttonStyle = ButtonStyles.Arrow;
            _buttonVisible = Settings.DefaultValue.TextVisible;
            _watermark = new Watermark();
            _mouseState = MouseStates.Normal;

            DrawMode = DrawMode.OwnerDrawVariable;
            DropDownStyle = ComboBoxStyle.DropDownList;

            _borderEdge = new BorderEdge();

            Size = new Size(135, 26);
            ItemHeight = 24;
            UpdateStyles();
            DropDownHeight = 100;

            // BackColor = SystemColors.Control;
            _border = new Border();

            _textRendererHint = Settings.DefaultValue.TextRenderingHint;

            Controls.Add(_borderEdge);
            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        public enum ButtonStyles
        {
            /// <summary>Arrow style.</summary>
            Arrow,

            /// <summary>Bars style.</summary>
            Bars,

            /// <summary>Image style.</summary>
            Image
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(ColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public ColorState BackColorState
        {
            get
            {
                return _backColorState;
            }

            set
            {
                _backColorState = value;
                Invalidate();
            }
        }

        [Browsable(true)]
        [Description(Property.Image)]
        public new Image BackgroundImage { get; set; }

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
        [Description(Property.Image)]
        public Image ButtonImage
        {
            get
            {
                return _buttonImage;
            }

            set
            {
                _buttonImage = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Type)]
        public ButtonStyles ButtonStyle
        {
            get
            {
                return _buttonStyle;
            }

            set
            {
                _buttonStyle = value;
                Invalidate();
            }
        }

        [DefaultValue(Settings.DefaultValue.TextVisible)]
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

        [Category(Propertys.Appearance)]
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
        [Description(Property.Image)]
        public ImageList ImageList
        {
            get
            {
                return _imageList;
            }

            set
            {
                _imageList = value;
                Invalidate();
            }
        }

        [Category(Propertys.Behavior)]
        [Description(Property.Visible)]
        public bool ImageVisible
        {
            get
            {
                return _imageVisible;
            }

            set
            {
                _imageVisible = value;
                Invalidate();
            }
        }

        [Category(Propertys.Behavior)]
        [Description(Property.StartIndex)]
        public int Index
        {
            get
            {
                return _index;
            }

            set
            {
                _index = value;
                try
                {
                    SelectedIndex = value;
                }
                catch (Exception)
                {
                    // ignored
                }

                Invalidate();
            }
        }

        [Category(Propertys.Behavior)]
        [Description(Property.Visible)]
        public bool ItemImageVisible
        {
            get
            {
                return _itemImageVisible;
            }

            set
            {
                _itemImageVisible = value;
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color MenuItemHover
        {
            get
            {
                return _menuItemHover;
            }

            set
            {
                _menuItemHover = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color MenuItemNormal
        {
            get
            {
                return _menuItemNormal;
            }

            set
            {
                _menuItemNormal = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color MenuTextColor
        {
            get
            {
                return _menuTextColor;
            }

            set
            {
                _menuTextColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.MouseState)]
        public MouseStates MouseState
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

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color SeparatorColor
        {
            get
            {
                return _borderEdge.BackColor;
            }

            set
            {
                _borderEdge.BackColor = value;
                Invalidate();
            }
        }

        [DefaultValue(Settings.DefaultValue.TextVisible)]
        [Category(Propertys.Behavior)]
        [Description(Property.Visible)]
        public bool SeparatorVisible
        {
            get
            {
                return _borderEdge.Visible;
            }

            set
            {
                _borderEdge.Visible = value;
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

        [Category(Propertys.Appearance)]
        [Description(Property.Alignment)]
        public StringAlignment TextAlignment
        {
            get
            {
                return _textAlignment;
            }

            set
            {
                _textAlignment = value;
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

        [Category(Propertys.Behavior)]
        [Description(Property.TextImageRelation)]
        public TextImageRelation TextImageRelation
        {
            get
            {
                return _textImageRelation;
            }

            set
            {
                _textImageRelation = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Alignment)]
        public StringAlignment TextLineAlignment
        {
            get
            {
                return _textLineAlignment;
            }

            set
            {
                _textLineAlignment = value;
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

        [TypeConverter(typeof(WatermarkConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Behavior)]
        public Watermark Watermark
        {
            get
            {
                return _watermark;
            }

            set
            {
                _watermark = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        /// <summary>Update the style of the control.</summary>
        /// <param name="style">The visual style.</param>
        public void UpdateTheme(Styles style)
        {
            _styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);

            _border.Color = _styleManager.ShapeStyle.Color;
            _border.HoverColor = _styleManager.BorderStyle.HoverColor;

            Font = _styleManager.Font;
            _foreColor = _styleManager.FontStyle.ForeColor;
            _textDisabledColor = _styleManager.FontStyle.ForeColorDisabled;

            _backColorState = new ColorState
                {
                    Enabled = _styleManager.ControlStyle.BoxEnabled,
                    Disabled = _styleManager.ControlStyle.BoxDisabled
                };

            _buttonColor = _styleManager.ControlStyle.FlatButtonEnabled;
            _menuTextColor = _styleManager.FontStyle.ForeColor;

            _menuItemNormal = _styleManager.ControlStyle.ItemEnabled;
            _menuItemHover = _styleManager.ControlStyle.ItemHover;

            _borderEdge.BackColor = _styleManager.ControlStyle.Line;

            Invalidate();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            // e.DrawBackground();
            e.Graphics.FillRectangle((e.State & DrawItemState.Selected) == DrawItemState.Selected ? new SolidBrush(_menuItemHover) : new SolidBrush(_menuItemNormal), e.Bounds);

            if (e.Index != -1)
            {
                Point _location;

                if ((_imageList != null) && _itemImageVisible)
                {
                    e.Graphics.DrawImage(_imageList.Images[e.Index], e.Bounds.X, (e.Bounds.Y + (e.Bounds.Height / 2)) - (_imageList.ImageSize.Height / 2), _imageList.ImageSize.Width, _imageList.ImageSize.Height);

                    _location = new Point(e.Bounds.X + _imageList.ImageSize.Width, e.Bounds.Y);
                }
                else
                {
                    _location = new Point(e.Bounds.X, e.Bounds.Y);
                }

                StringFormat _stringFormat = new StringFormat
                    {
                        LineAlignment = _textLineAlignment
                    };

                e.Graphics.DrawString(GetItemText(Items[e.Index]), Font, new SolidBrush(_menuTextColor), new Rectangle(_location, new Size(DropDownWidth, ItemHeight)), _stringFormat);
            }

            // Draw rectangle over the item selected 
            // e.DrawFocusRectangle();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            _watermark.Brush = new SolidBrush(_watermark.ActiveColor);
            _mouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            _watermark.Brush = new SolidBrush(_watermark.InactiveColor);
            _mouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            SuspendLayout();
            Update();
            ResumeLayout();
            _mouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            Graphics g = CreateGraphics();
            var maxWidth = 0;

            foreach (int width in Items.Cast<object>().Select(element => (int)g.MeasureString(element.ToString(), Font).Width).Where(width => width > maxWidth))
            {
                maxWidth = width;
            }

            DropDownWidth = maxWidth + 20;
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
            _mouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _mouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseState = MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics _graphics = e.Graphics;
            _graphics.Clear(Parent.BackColor);
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.TextRenderingHint = _textRendererHint;

            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            _controlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);

            _graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, ClientRectangle.Width + 1, ClientRectangle.Height + 1));
            _graphics.SetClip(_controlGraphicsPath);

            Color _backColor = Enabled ? _backColorState.Enabled : _backColorState.Disabled;
            VisualBackgroundRenderer.DrawBackground(_graphics, _backColor, BackgroundImage, _mouseState, _clientRectangle, Border);

            Rectangle _buttonRectangle = new Rectangle(new Point(Width - _buttonWidth, 0), new Size(_buttonWidth, Height));
            Rectangle _textBoxRectangle = new Rectangle(0, 0, Width - _buttonWidth, Height);

            ConfigureSeparator(_buttonRectangle);

            DrawContent(_graphics, _textBoxRectangle);
            DrawButton(_graphics, _buttonRectangle);

            _graphics.ResetClip();

            DrawWatermark(_graphics, _textBoxRectangle);
            VisualBorderRenderer.DrawBorderStyle(_graphics, _border, _controlGraphicsPath, _mouseState);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(BackColor);
        }

        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            OnLostFocus(e);
        }

        private void ConfigureSeparator(Rectangle rectangle)
        {
            if (!_borderEdge.Visible)
            {
                return;
            }

            _borderEdge.Location = new Point(rectangle.X - 1, _border.Thickness);
            _borderEdge.Size = new Size(1, Height - _border.Thickness - 1);
        }

        /// <summary>Draws the button.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The rectangle to draw on.</param>
        private void DrawButton(Graphics graphics, Rectangle rectangle)
        {
            if (!_buttonVisible)
            {
                return;
            }

            Point _buttonImageLocation;
            Size _buttonImageSize;

            switch (_buttonStyle)
            {
                case ButtonStyles.Arrow:
                    {
                        _buttonImageSize = new Size(10, 6);
                        _buttonImageLocation = new Point((rectangle.X + (rectangle.Width / 2)) - (_buttonImageSize.Width / 2), (rectangle.Y + (rectangle.Height / 2)) - (_buttonImageSize.Height / 2));
                        GraphicsManager.DrawTriangle(graphics, new Rectangle(_buttonImageLocation, _buttonImageSize), new SolidBrush(_buttonColor), false);

                        break;
                    }

                case ButtonStyles.Bars:
                    {
                        _buttonImageSize = new Size(18, 10);
                        _buttonImageLocation = new Point((rectangle.X + (rectangle.Width / 2)) - (_buttonImageSize.Width / 2), (rectangle.Y + (rectangle.Height / 2)) - _buttonImageSize.Height);
                        Bars.DrawBars(graphics, _buttonImageLocation, _buttonImageSize, _buttonColor, 3, 5);

                        break;
                    }

                case ButtonStyles.Image:
                    {
                        if (_buttonImage != null)
                        {
                            _buttonImageLocation = new Point((rectangle.X + (rectangle.Width / 2)) - (_buttonImage.Width / 2), (rectangle.Y + (rectangle.Height / 2)) - (_buttonImage.Height / 2));
                            graphics.DrawImage(_buttonImage, _buttonImageLocation);
                        }

                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }
        }

        /// <summary>Draws the button.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The rectangle to draw on.</param>
        private void DrawContent(Graphics graphics, Rectangle rectangle)
        {
            Color _textColor = Enabled ? _foreColor : _textDisabledColor;

            if ((SelectedIndex != -1) && (_imageList != null) && _imageVisible)
            {
                VisualControlRenderer.DrawContent(graphics, rectangle, Text, Font, _textColor, _imageList.Images[SelectedIndex], _imageList.ImageSize, _textImageRelation);
            }
            else
            {
                VisualControlRenderer.DrawContentText(graphics, rectangle, Text, Font, _textColor, _textAlignment, _textLineAlignment);
            }
        }

        /// <summary>Draws the watermark.</summary>
        /// <param name="graphics">The graphics to draw on.</param>
        /// <param name="rectangle">The rectangle to draw on.</param>
        private void DrawWatermark(Graphics graphics, Rectangle rectangle)
        {
            if (Text.Length != 0)
            {
                return;
            }

            StringFormat _stringFormat = new StringFormat
                {
                    Alignment = _textAlignment,
                    LineAlignment = _textLineAlignment
                };

            Watermark.DrawWatermark(graphics, rectangle, _stringFormat, _watermark);
        }

        #endregion
    }
}