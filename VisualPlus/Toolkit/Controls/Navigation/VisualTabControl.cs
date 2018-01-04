namespace VisualPlus.Toolkit.Controls.Navigation
{
    #region Namespace

    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Linq;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(TabControl))]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("TabPages")]
    [Description("The Visual TabControl")]
    public class VisualTabControl : TabControl
    {
        #region Variables

        private TabAlignment _alignment;
        private bool _arrowSelectorVisible;
        private int _arrowSpacing;
        private int _arrowThickness;
        private Color _backgroundColor;
        private Border _border;
        private Size _itemSize;
        private Point _mouseLocation;
        private MouseStates _mouseState;
        private TabAlignment _selectorAlignment;
        private TabAlignment _selectorAlignment2;
        private int _selectorThickness;
        private bool _selectorVisible;
        private bool _selectorVisible2;
        private Color _separator;
        private int _separatorSpacing;
        private float _separatorThickness;
        private bool _separatorVisible;
        private StylesManager _styleManager;
        private Color _tabHover;
        private Color _tabMenu;
        private Color _tabNormal;
        private Shape _tabPageBorder;
        private Rectangle _tabPageRectangle;
        private Color _tabSelected;
        private Color _tabSelector;
        private StringAlignment _textAlignment;
        private StringAlignment _textLineAlignment;
        private Color _textNormal;
        private TextRenderingHint _textRendererHint;
        private Color _textSelected;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualTabControl" /> class.</summary>
        public VisualTabControl()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor,
                true);

            UpdateStyles();

            _styleManager = new StylesManager(Settings.DefaultValue.DefaultStyle);

            _textLineAlignment = StringAlignment.Center;
            _border = new Border();
            _alignment = TabAlignment.Top;
            _arrowSelectorVisible = true;
            _arrowSpacing = 10;
            _arrowThickness = 5;
            _itemSize = new Size(100, 25);
            _selectorAlignment = TabAlignment.Top;
            _selectorAlignment2 = TabAlignment.Bottom;
            _selectorThickness = 4;
            _separatorSpacing = 2;
            _separatorThickness = 2F;
            _backgroundColor = _styleManager.Theme.BackgroundSettings.Type4;
            _separator = _styleManager.Theme.OtherSettings.Line;
            _tabMenu = Color.FromArgb(55, 61, 73);
            _textAlignment = StringAlignment.Center;
            _tabSelector = Color.Green;
            _textNormal = Color.FromArgb(174, 181, 187);
            _textRendererHint = Settings.DefaultValue.TextRenderingHint;
            _textSelected = Color.FromArgb(217, 220, 227);
            Font = _styleManager.Theme.TextSetting.Font;

            Size = new Size(320, 160);
            MinimumSize = new Size(144, 85);
            ItemSize = _itemSize;

            _tabPageBorder = new Shape();
            _tabNormal = _styleManager.Theme.OtherSettings.TabPageEnabled;
            _tabSelected = _styleManager.Theme.OtherSettings.TabPageSelected;
            _tabHover = _styleManager.Theme.OtherSettings.TabPageHover;

            foreach (TabPage page in TabPages)
            {
                page.BackColor = _backgroundColor;
                page.Font = Font;
            }
        }

        #endregion

        #region Properties

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Alignment)]
        public new TabAlignment Alignment
        {
            get
            {
                return _alignment;
            }

            set
            {
                _alignment = value;
                base.Alignment = _alignment;

                // Resize tabs
                switch (_alignment)
                {
                    case TabAlignment.Top:
                    case TabAlignment.Bottom:
                        {
                            if (_itemSize.Width < _itemSize.Height)
                            {
                                ItemSize = new Size(_itemSize.Height, _itemSize.Width);
                            }

                            break;
                        }

                    case TabAlignment.Left:
                    case TabAlignment.Right:
                        {
                            if (_itemSize.Width > _itemSize.Height)
                            {
                                ItemSize = new Size(_itemSize.Height, _itemSize.Width);
                            }

                            break;
                        }
                }

                UpdateArrowLocation();
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Visible)]
        public bool ArrowSelectorVisible
        {
            get
            {
                return _arrowSelectorVisible;
            }

            set
            {
                _arrowSelectorVisible = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Spacing)]
        public int ArrowSpacing
        {
            get
            {
                return _arrowSpacing;
            }

            set
            {
                _arrowSpacing = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Size)]
        public int ArrowThickness
        {
            get
            {
                return _arrowThickness;
            }

            set
            {
                _arrowThickness = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }

            set
            {
                _backgroundColor = value;
                foreach (TabPage page in TabPages)
                {
                    page.BackColor = _backgroundColor;
                }

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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Size)]
        public new Size ItemSize
        {
            get
            {
                return _itemSize;
            }

            set
            {
                _itemSize = value;
                base.ItemSize = _itemSize;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Alignment)]
        public TabAlignment SelectorAlignment
        {
            get
            {
                return _selectorAlignment;
            }

            set
            {
                _selectorAlignment = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Alignment)]
        public TabAlignment SelectorAlignment2
        {
            get
            {
                return _selectorAlignment2;
            }

            set
            {
                _selectorAlignment2 = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Size)]
        public int SelectorThickness
        {
            get
            {
                return _selectorThickness;
            }

            set
            {
                _selectorThickness = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Visible)]
        public bool SelectorVisible
        {
            get
            {
                return _selectorVisible;
            }

            set
            {
                _selectorVisible = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Visible)]
        public bool SelectorVisible2
        {
            get
            {
                return _selectorVisible2;
            }

            set
            {
                _selectorVisible2 = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color Separator
        {
            get
            {
                return _separator;
            }

            set
            {
                _separator = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Spacing)]
        public int SeparatorSpacing
        {
            get
            {
                return _separatorSpacing;
            }

            set
            {
                _separatorSpacing = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Size)]
        public float SeparatorThickness
        {
            get
            {
                return _separatorThickness;
            }

            set
            {
                _separatorThickness = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Visible)]
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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.MouseState)]
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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TabHover
        {
            get
            {
                return _tabHover;
            }

            set
            {
                _tabHover = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TabMenu
        {
            get
            {
                return _tabMenu;
            }

            set
            {
                _tabMenu = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TabNormal
        {
            get
            {
                return _tabNormal;
            }

            set
            {
                _tabNormal = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(ShapeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(PropertyCategory.Appearance)]
        public Shape TabPageBorder
        {
            get
            {
                return _tabPageBorder;
            }

            set
            {
                _tabPageBorder = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TabSelected
        {
            get
            {
                return _tabSelected;
            }

            set
            {
                _tabSelected = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TabSelector
        {
            get
            {
                return _tabSelector;
            }

            set
            {
                _tabSelector = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Alignment)]
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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TextNormal
        {
            get
            {
                return _textNormal;
            }

            set
            {
                _textNormal = value;
                Invalidate();
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
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TextSelected
        {
            get
            {
                return _textSelected;
            }

            set
            {
                _textSelected = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        protected override void CreateHandle()
        {
            base.CreateHandle();

            DoubleBuffered = true;
            SizeMode = TabSizeMode.Fixed;
            Appearance = TabAppearance.Normal;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            GraphicsManager.ApplyContainerBackColorChange(this, _backgroundColor);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (!(e.Control is TabPage))
            {
                return;
            }

            try
            {
                IEnumerator enumerator = Controls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    using (new TabPage())
                    {
                        BackColor = _backgroundColor;
                    }
                }
            }
            finally
            {
                GraphicsManager.SetControlBackColor(e.Control, _backgroundColor, false);
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            GraphicsManager.SetControlBackColor(e.Control, Parent.BackColor, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = MouseStates.Hover;
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
            State = MouseStates.Normal;
            if (TabPages.Cast<TabPage>().Any(Tab => Tab.DisplayRectangle.Contains(_mouseLocation)))
            {
                Invalidate();
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _mouseLocation = e.Location;
            if (TabPages.Cast<TabPage>().Any(Tab => Tab.DisplayRectangle.Contains(e.Location)))
            {
                Invalidate();
            }

            Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics _graphics = e.Graphics;
            _graphics.Clear(Parent.BackColor);
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.CompositingMode = CompositingMode.SourceOver;
            _graphics.CompositingQuality = CompositingQuality.Default;
            _graphics.InterpolationMode = InterpolationMode.Default;
            _graphics.PixelOffsetMode = PixelOffsetMode.Default;
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.TextRenderingHint = _textRendererHint;

            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, ClientRectangle.Width + 1, ClientRectangle.Height + 1);
            _graphics.FillRectangle(new SolidBrush(BackColor), _clientRectangle);

            VisualBackgroundRenderer.DrawBackground(e.Graphics, _tabMenu, BackgroundImage, _mouseState, ClientRectangle, _border);

            DrawTabPages(e);
            DrawSeparator(e);
        }

        private void ConfigureAlignmentStyle(int tabIndex)
        {
            if ((Alignment == TabAlignment.Top) && (Alignment == TabAlignment.Bottom))
            {
                // Top - Bottom
                _tabPageRectangle = new Rectangle(
                    new Point(GetTabRect(tabIndex).Location.X, GetTabRect(tabIndex).Location.Y),
                    new Size(GetTabRect(tabIndex).Width, GetTabRect(tabIndex).Height));
            }
            else
            {
                // Left - Right
                _tabPageRectangle = new Rectangle(
                    new Point(GetTabRect(tabIndex).Location.X, GetTabRect(tabIndex).Location.Y),
                    new Size(GetTabRect(tabIndex).Width, GetTabRect(tabIndex).Height));
            }
        }

        private void DrawSelectionArrow(PaintEventArgs e, Rectangle selectedRectangle)
        {
            var points = new Point[3];

            switch (Alignment)
            {
                case TabAlignment.Left:
                    {
                        points[0].X = selectedRectangle.Right - ArrowThickness;
                        points[0].Y = selectedRectangle.Y + (selectedRectangle.Height / 2);

                        points[1].X = selectedRectangle.Right + ArrowSpacing;
                        points[1].Y = selectedRectangle.Top + ArrowSpacing;

                        points[2].X = selectedRectangle.Right + ArrowSpacing;
                        points[2].Y = selectedRectangle.Bottom - ArrowSpacing;
                        break;
                    }

                case TabAlignment.Top:
                    {
                        points[0].X = selectedRectangle.X + (selectedRectangle.Width / 2);
                        points[0].Y = selectedRectangle.Bottom - ArrowThickness;

                        points[1].X = selectedRectangle.Left + ArrowSpacing;
                        points[1].Y = selectedRectangle.Bottom + ArrowSpacing;

                        points[2].X = selectedRectangle.Right - ArrowSpacing;
                        points[2].Y = selectedRectangle.Bottom + ArrowSpacing;
                        break;
                    }

                case TabAlignment.Bottom:
                    {
                        points[0].X = selectedRectangle.X + (selectedRectangle.Width / 2);
                        points[0].Y = selectedRectangle.Top + ArrowThickness;

                        points[1].X = selectedRectangle.Left + ArrowSpacing;
                        points[1].Y = selectedRectangle.Top - ArrowSpacing;

                        points[2].X = selectedRectangle.Right - ArrowSpacing;
                        points[2].Y = selectedRectangle.Top - ArrowSpacing;
                        break;
                    }

                case TabAlignment.Right:
                    {
                        points[0].X = selectedRectangle.Left + ArrowThickness;
                        points[0].Y = selectedRectangle.Y + (selectedRectangle.Height / 2);

                        points[1].X = selectedRectangle.Left - ArrowSpacing;
                        points[1].Y = selectedRectangle.Top + ArrowSpacing;

                        points[2].X = selectedRectangle.Left - ArrowSpacing;
                        points[2].Y = selectedRectangle.Bottom - ArrowSpacing;
                        break;
                    }
            }

            e.Graphics.FillPolygon(new SolidBrush(_backgroundColor), points);
        }

        private void DrawSeparator(PaintEventArgs e)
        {
            if (!_separatorVisible)
            {
                return;
            }

            // Draw divider that separates the panels.
            switch (Alignment)
            {
                case TabAlignment.Top:
                    {
                        e.Graphics.DrawLine(new Pen(_separator, _separatorThickness), 0, ItemSize.Height + _separatorSpacing, Width, ItemSize.Height + _separatorSpacing);
                        break;
                    }

                case TabAlignment.Bottom:
                    {
                        e.Graphics.DrawLine(new Pen(_separator, _separatorThickness), 0, Height - ItemSize.Height - _separatorSpacing, Width, Height - ItemSize.Height - _separatorSpacing);
                        break;
                    }

                case TabAlignment.Left:
                    {
                        e.Graphics.DrawLine(new Pen(_separator, _separatorThickness), ItemSize.Height + _separatorSpacing, 0, ItemSize.Height + _separatorSpacing, Height);
                        break;
                    }

                case TabAlignment.Right:
                    {
                        e.Graphics.DrawLine(new Pen(_separator, _separatorThickness), Width - ItemSize.Height - _separatorSpacing, 0, Width - ItemSize.Height - _separatorSpacing, Height);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }
        }

        private void DrawTabPages(PaintEventArgs e)
        {
            StringFormat _tabTextFormat = new StringFormat
                {
                    Alignment = _textAlignment,
                    LineAlignment = _textLineAlignment
                };

            for (var tabIndex = 0; tabIndex <= TabCount - 1; tabIndex++)
            {
                ConfigureAlignmentStyle(tabIndex);

                // Draws the TabSelector
                Rectangle selectorRectangle = GraphicsManager.ApplyAnchor(_selectorAlignment, GetTabRect(tabIndex), _selectorThickness);
                Rectangle selectorRectangle2 = GraphicsManager.ApplyAnchor(SelectorAlignment2, GetTabRect(tabIndex), _selectorThickness);

                if (tabIndex == SelectedIndex)
                {
                    // Draw selected tab
                    e.Graphics.FillRectangle(new SolidBrush(_tabSelected), _tabPageRectangle);

                    // Draw tab selector
                    if (_selectorVisible)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(_tabSelector), selectorRectangle);
                    }

                    if (_selectorVisible2)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(_tabSelector), selectorRectangle2);
                    }

                    GraphicsPath borderPath = new GraphicsPath();
                    borderPath.AddRectangle(_tabPageRectangle);

                    VisualBorderRenderer.DrawBorder(e.Graphics, _tabPageRectangle, _tabPageBorder.Color, _tabPageBorder.Thickness);

                    if (_arrowSelectorVisible)
                    {
                        DrawSelectionArrow(e, _tabPageRectangle);
                    }

                    // Draw selected tab text
                    e.Graphics.DrawString(TabPages[tabIndex].Text, Font, new SolidBrush(_textSelected), _tabPageRectangle, _tabTextFormat);

                    if (ImageList != null)
                    {
                        e.Graphics.DrawImage(ImageList.Images[tabIndex], _tabPageRectangle.X, (_tabPageRectangle.Y + (_tabPageRectangle.Height / 2)) - (ImageList.ImageSize.Height / 2), ImageList.Images[tabIndex].Size.Height, ImageList.Images[tabIndex].Size.Width);
                    }
                }
                else
                {
                    // Draw other TabPages
                    e.Graphics.FillRectangle(new SolidBrush(_tabNormal), _tabPageRectangle);

                    if ((State == MouseStates.Hover) && _tabPageRectangle.Contains(_mouseLocation))
                    {
                        // Draw hover background
                        e.Graphics.FillRectangle(new SolidBrush(_tabHover), _tabPageRectangle);

                        // Draw tab selector
                        if (_selectorVisible)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(_tabSelector), selectorRectangle);
                        }

                        if (_selectorVisible2)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(_tabSelector), selectorRectangle2);
                        }

                        GraphicsPath borderPath = new GraphicsPath();
                        borderPath.AddRectangle(_tabPageRectangle);

                        VisualBorderRenderer.DrawBorder(e.Graphics, _tabPageRectangle, _tabPageBorder.Color, _tabPageBorder.Thickness);
                    }

                    e.Graphics.DrawString(TabPages[tabIndex].Text, Font, new SolidBrush(_textNormal), _tabPageRectangle, _tabTextFormat);

                    if (ImageList != null)
                    {
                        e.Graphics.DrawImage(ImageList.Images[tabIndex], _tabPageRectangle.X, (_tabPageRectangle.Y + (_tabPageRectangle.Height / 2)) - (ImageList.ImageSize.Height / 2), ImageList.Images[tabIndex].Size.Height, ImageList.Images[tabIndex].Size.Width);
                    }
                }
            }
        }

        private void UpdateArrowLocation()
        {
            switch (_alignment)
            {
                case TabAlignment.Top:
                case TabAlignment.Bottom:
                    {
                        _arrowThickness = 5;
                        _arrowSpacing = 10;
                        break;
                    }

                case TabAlignment.Left:
                case TabAlignment.Right:
                    {
                        _arrowThickness = 10;
                        _arrowSpacing = 3;
                        break;
                    }
            }
        }

        #endregion
    }
}