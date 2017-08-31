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
    using VisualPlus.Toolkit.ActionList;
    using VisualPlus.Toolkit.Components;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ComboBox))]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Items")]
    [Description("The Visual ComboBox")]
    [Designer(ControlManager.FilterProperties.VisualComboBox, typeof(VisualComboBoxTasks))]
    public class VisualComboBox : ComboBox
    {
        #region Variables

        private ColorState _backColorState;

        private Border _border;
        private Color _buttonColor;
        private Alignment.Horizontal _buttonHorizontal;
        private DropDownButtons _buttonStyles;
        private bool _buttonVisible;
        private int _buttonWidth;
        private GraphicsPath _controlGraphicsPath;
        private Color _foreColor;
        private Size _itemSize;
        private Color _menuItemHover;
        private Color _menuItemNormal;
        private Color _menuTextColor;
        private MouseStates _mouseState;
        private Color _separatorColor;
        private Color _separatorShadowColor;
        private bool _separatorVisible;
        private int _startIndex;
        private VisualStyleManager _styleManager;
        private StringAlignment _textAlignment;
        private Color _textDisabledColor;
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

            _buttonWidth = 30;
            _buttonHorizontal = Alignment.Horizontal.Right;
            _buttonStyles = DropDownButtons.Arrow;
            _buttonVisible = Settings.DefaultValue.TextVisible;
            _separatorVisible = Settings.DefaultValue.TextVisible;
            _textAlignment = StringAlignment.Center;
            _watermark = new Watermark();
            _backColorState = new ColorState();
            _mouseState = MouseStates.Normal;
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;

            Size = new Size(135, 26);
            ItemHeight = 20;
            UpdateStyles();
            DropDownHeight = 100;

            BackColor = SystemColors.Control;

            _border = new Border();

            _textRendererHint = Settings.DefaultValue.TextRenderingHint;

            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        public enum DropDownButtons
        {
            /// <summary>Use arrow button.</summary>
            Arrow,

            /// <summary>Use bars button.</summary>
            Bars
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

        [Category(Propertys.Layout)]
        [Description(Property.Direction)]
        public Alignment.Horizontal ButtonHorizontal
        {
            get
            {
                return _buttonHorizontal;
            }

            set
            {
                _buttonHorizontal = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Type)]
        public DropDownButtons ButtonStyles
        {
            get
            {
                return _buttonStyles;
            }

            set
            {
                _buttonStyles = value;
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
                return _separatorColor;
            }

            set
            {
                _separatorColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color SeparatorShadowColor
        {
            get
            {
                return _separatorShadowColor;
            }

            set
            {
                _separatorShadowColor = value;
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
                return _separatorVisible;
            }

            set
            {
                _separatorVisible = value;
                Invalidate();
            }
        }

        [Category(Propertys.Behavior)]
        [Description(Property.StartIndex)]
        public int StartIndex
        {
            get
            {
                return _startIndex;
            }

            set
            {
                _startIndex = value;
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

            _backColorState.Enabled = _styleManager.ControlStyle.BoxEnabled.Colors[0];
            _backColorState.Disabled = _styleManager.ControlStyle.BoxDisabled.Colors[0];

            _buttonColor = _styleManager.ControlStyle.FlatButtonEnabled;
            _menuTextColor = _styleManager.FontStyle.ForeColor;

            _menuItemNormal = _styleManager.ControlStyle.ItemEnabled;
            _menuItemHover = _styleManager.ControlStyle.ItemHover;

            _separatorColor = _styleManager.ControlStyle.Line;
            _separatorShadowColor = _styleManager.ControlStyle.Shadow;

            Invalidate();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle((e.State & DrawItemState.Selected) == DrawItemState.Selected ? new SolidBrush(_menuItemHover) : new SolidBrush(_menuItemNormal), e.Bounds);

            _itemSize = e.Bounds.Size;

            Point itemPoint = new Point(e.Bounds.X, e.Bounds.Y);
            Rectangle itemBorderRectangle = new Rectangle(itemPoint, _itemSize);
            GraphicsPath itemBorderPath = new GraphicsPath();
            itemBorderPath.AddRectangle(itemBorderRectangle);

            if (e.Index != -1)
            {
                e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, new SolidBrush(_menuTextColor), e.Bounds);
            }
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
            _graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            _controlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);

            Color _textColor = Enabled ? _foreColor : _textDisabledColor;
            Color _backColor = Enabled ? _backColorState.Enabled : _backColorState.Disabled;

            VisualBackgroundRenderer.DrawBackground(e.Graphics, _backColor, BackgroundImage, _mouseState, _clientRectangle, Border);

            Point _textBoxLocation;
            Point _buttonLocation;
            Size _buttonSize = new Size(_buttonWidth, Height);

            if (_buttonHorizontal == Alignment.Horizontal.Right)
            {
                _buttonLocation = new Point(Width - _buttonWidth, 0);
                _textBoxLocation = new Point(0, 0);
            }
            else
            {
                _buttonLocation = new Point(0, 0);
                _textBoxLocation = new Point(_buttonWidth, 0);
            }

            Rectangle _buttonRectangle = new Rectangle(_buttonLocation, _buttonSize);
            Rectangle _textBoxRectangle = new Rectangle(_textBoxLocation.X, _textBoxLocation.Y, Width - _buttonWidth, Height);

            DrawButton(_graphics, _buttonRectangle);
            DrawSeparator(_graphics, _buttonRectangle);

            StringFormat _stringFormat = new StringFormat
                {
                    Alignment = _textAlignment,
                    LineAlignment = StringAlignment.Center
                };

            ConfigureDirection(_textBoxRectangle, _buttonRectangle);
            _graphics.DrawString(Text, Font, new SolidBrush(_textColor), _textBoxRectangle, _stringFormat);

            if (Text.Length == 0)
            {
                Watermark.DrawWatermark(_graphics, _textBoxRectangle, _stringFormat, _watermark);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(Parent.BackColor);
        }

        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            OnLostFocus(e);
        }

        private void ConfigureDirection(Rectangle textBoxRectangle, Rectangle buttonRectangle)
        {
            if (_buttonHorizontal == Alignment.Horizontal.Right)
            {
                switch (_textAlignment)
                {
                    case StringAlignment.Far:
                        textBoxRectangle.Width -= buttonRectangle.Width;
                        break;
                    case StringAlignment.Near:
                        textBoxRectangle.X = 0;
                        break;
                    case StringAlignment.Center:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (_textAlignment)
                {
                    case StringAlignment.Far:
                        textBoxRectangle.Width -= buttonRectangle.Width;
                        textBoxRectangle.X = Width - textBoxRectangle.Width;
                        break;
                    case StringAlignment.Near:
                        textBoxRectangle.X = _buttonWidth;
                        break;
                    case StringAlignment.Center:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void DrawButton(Graphics graphics, Rectangle buttonRectangle)
        {
            if (_buttonVisible)
            {
                Point _buttonImageLocation;
                Size _buttonImageSize;

                switch (_buttonStyles)
                {
                    case DropDownButtons.Arrow:
                        {
                            _buttonImageSize = new Size(10, 8);
                            _buttonImageLocation = new Point((buttonRectangle.X + (buttonRectangle.Width / 2)) - (_buttonImageSize.Width / 2), (buttonRectangle.Y + (buttonRectangle.Height / 2)) - (_buttonImageSize.Height / 2));
                            GDI.DrawTriangle(graphics, new Rectangle(_buttonImageLocation, _buttonImageSize), new SolidBrush(_buttonColor), false);
                            break;
                        }

                    case DropDownButtons.Bars:
                        {
                            _buttonImageSize = new Size(18, 10);
                            _buttonImageLocation = new Point((buttonRectangle.X + (buttonRectangle.Width / 2)) - (_buttonImageSize.Width / 2), (buttonRectangle.Y + (buttonRectangle.Height / 2)) - _buttonImageSize.Height);
                            Bars.DrawBars(graphics, _buttonImageLocation, _buttonImageSize, _buttonColor, 3, 5);
                            break;
                        }
                }
            }
        }

        private void DrawSeparator(Graphics graphics, Rectangle buttonRectangle)
        {
            if (_separatorVisible)
            {
                if (_buttonHorizontal == Alignment.Horizontal.Right)
                {
                    graphics.DrawLine(new Pen(_separatorColor), buttonRectangle.X - 1, 4, buttonRectangle.X - 1, Height - 5);
                    graphics.DrawLine(new Pen(_separatorShadowColor), buttonRectangle.X, 4, buttonRectangle.X, Height - 5);
                }
                else
                {
                    graphics.DrawLine(new Pen(_separatorColor), buttonRectangle.Width - 1, 4, buttonRectangle.Width - 1, Height - 5);
                    graphics.DrawLine(new Pen(_separatorShadowColor), buttonRectangle.Width, 4, buttonRectangle.Width, Height - 5);
                }
            }
        }

        #endregion
    }
}