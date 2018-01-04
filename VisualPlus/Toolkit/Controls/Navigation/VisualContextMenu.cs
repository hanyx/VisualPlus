namespace VisualPlus.Toolkit.Controls.Navigation
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Opening")]
    [DefaultProperty("Items")]
    [Description("The Visual Context Menu Strip")]
    [ToolboxBitmap(typeof(VisualContextMenuStrip), "Resources.ToolboxBitmaps.VisualContextMenuStrip.bmp")]
    [ToolboxItem(true)]
    public class VisualContextMenuStrip : ContextMenuStrip
    {
        #region Variables

        private StylesManager _styleManager;
        private ToolStripItemClickedEventArgs _toolStripItemClickedEventArgs;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="T:VisualPlus.Toolkit.Controls.Navigation.VisualContextMenuStrip" /> class.
        /// </summary>
        public VisualContextMenuStrip()
        {
            _styleManager = new StylesManager(Settings.DefaultValue.DefaultStyle);

            Renderer = new VisualToolStripRender();
            ConfigureStyleManager();
        }

        public delegate void ClickedEventHandler(object sender);

        public event ClickedEventHandler Clicked;

        #endregion

        #region Properties

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color ArrowColor
        {
            get
            {
                return arrowColor;
            }

            set
            {
                arrowColor = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color ArrowDisabledColor
        {
            get
            {
                return arrowDisabledColor;
            }

            set
            {
                arrowDisabledColor = value;
                Invalidate();
            }
        }

        [DefaultValue(Settings.DefaultValue.BorderVisible)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Visible)]
        public bool ArrowVisible
        {
            get
            {
                return _arrowVisible;
            }

            set
            {
                _arrowVisible = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color Background
        {
            get
            {
                return _backgroundColor;
            }

            set
            {
                _backgroundColor = value;
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
                return border;
            }

            set
            {
                border = value;
                Invalidate();
            }
        }

        public new Color ForeColor
        {
            get
            {
                return foreColor;
            }

            set
            {
                base.ForeColor = value;
                foreColor = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color ItemHoverColor
        {
            get
            {
                return _itemHoverColor;
            }

            set
            {
                _itemHoverColor = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Font)]
        public Font MenuFont
        {
            get
            {
                return contextMenuFont;
            }

            set
            {
                contextMenuFont = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color SelectedItemBackColor
        {
            get
            {
                return _selectedItemBackColor;
            }

            set
            {
                _selectedItemBackColor = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color TextDisabledColor
        {
            get
            {
                return textDisabledColor;
            }

            set
            {
                textDisabledColor = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            if ((e.ClickedItem != null) && !(e.ClickedItem is ToolStripSeparator))
            {
                if (ReferenceEquals(e, _toolStripItemClickedEventArgs))
                {
                    OnItemClicked(e);
                }
                else
                {
                    _toolStripItemClickedEventArgs = e;
                    Clicked?.Invoke(this);
                }
            }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            Cursor = Cursors.Hand;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Cursor = Cursors.Hand;
            Invalidate();
        }

        private static bool _arrowVisible = Settings.DefaultValue.TextVisible;
        private static Color _backgroundColor;
        private static Color _itemHoverColor;
        private static Color _selectedItemBackColor;

        private static Color arrowColor;
        private static Color arrowDisabledColor;
        private static Border border;
        private static Font contextMenuFont;
        private static Color foreColor;
        private static Color textDisabledColor;

        private void ConfigureStyleManager()
        {
            border = new Border
                {
                    HoverVisible = false,
                    Type = ShapeType.Rectangle
                };

            Font = _styleManager.Theme.TextSetting.Font;
            foreColor = _styleManager.Theme.TextSetting.Enabled;
            textDisabledColor = _styleManager.Theme.TextSetting.Disabled;

            BackColor = _backgroundColor;
            arrowColor = _styleManager.Theme.OtherSettings.FlatControlEnabled;
            arrowDisabledColor = _styleManager.Theme.OtherSettings.FlatControlDisabled;
            contextMenuFont = Font;

            _backgroundColor = _styleManager.Theme.BackgroundSettings.Type1;
            _selectedItemBackColor = _styleManager.Theme.ListItemSettings.ItemHover;
            _itemHoverColor = _styleManager.Theme.BorderSettings.Hover;
        }

        #endregion

        #region Methods

        public sealed class VisualToolStripRender : ToolStripProfessionalRenderer
        {
            #region Events

            /// <summary>Renders the arrow.</summary>
            /// <param name="e">The tool strip render event args.</param>
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                if (!_arrowVisible)
                {
                    return;
                }

                int _xArrow = e.Item.ContentRectangle.X + e.Item.ContentRectangle.Width;
                int _yArrow = (e.ArrowRectangle.Y + e.ArrowRectangle.Height) / 2;

                Point[] _arrowLocation =
                    {
                        new Point(_xArrow - 5, _yArrow - 5),
                        new Point(_xArrow, _yArrow),
                        new Point(_xArrow - 5, _yArrow + 5)
                    };

                Color _arrowStateColor = e.Item.Enabled ? arrowColor : arrowDisabledColor;
                e.Graphics.FillPolygon(new SolidBrush(_arrowStateColor), _arrowLocation);
            }

            /// <summary>Renders the image margin.</summary>
            /// <param name="e">The tool strip render event args.</param>
            protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
            {
                // Allow to add images to ToolStrips
                // MyBase.OnRenderImageMargin(e) 
            }

            /// <summary>Renders the item text.</summary>
            /// <param name="e">The tool strip render event args.</param>
            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                Rectangle _itemTextRectangle = new Rectangle(25, e.Item.ContentRectangle.Y, e.Item.ContentRectangle.Width - (24 + 16), e.Item.ContentRectangle.Height - 4);

                Color _itemTextColor = e.Item.Enabled ? e.Item.Selected ? _itemHoverColor : foreColor : textDisabledColor;

                StringFormat _stringFormat = new StringFormat
                    {
                        // Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                e.Graphics.DrawString(e.Text, contextMenuFont, new SolidBrush(_itemTextColor), _itemTextRectangle, _stringFormat);
            }

            /// <summary>Renders the menu item background.</summary>
            /// <param name="e">The tool strip render event args.</param>
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                e.Graphics.InterpolationMode = InterpolationMode.High;
                e.Graphics.Clear(_backgroundColor);

                Rectangle _menuItemBackgroundRectangle = new Rectangle(0, e.Item.ContentRectangle.Y - 2, e.Item.ContentRectangle.Width + 4, e.Item.ContentRectangle.Height + 3);

                e.Graphics.FillRectangle(e.Item.Selected && e.Item.Enabled ? new SolidBrush(_selectedItemBackColor) : new SolidBrush(_backgroundColor), _menuItemBackgroundRectangle);
            }

            /// <summary>Renders the separator.</summary>
            /// <param name="e">The tool strip render event args.</param>
            protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
            {
                Point _pt1 = new Point(e.Item.Bounds.Left, e.Item.Bounds.Height / 2);
                Point _pt2 = new Point(e.Item.Bounds.Right - 5, e.Item.Bounds.Height / 2);

                e.Graphics.DrawLine(new Pen(Color.FromArgb(200, border.Color), border.Thickness), _pt1, _pt2);
            }

            /// <summary>Renders the tool strip background.</summary>
            /// <param name="e">The tool strip render event args.</param>
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                base.OnRenderToolStripBackground(e);
                e.Graphics.Clear(_backgroundColor);
            }

            /// <summary>Renders the tool strip border.</summary>
            /// <param name="e">The tool strip render event args.</param>
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (border.Visible)
                {
                    e.Graphics.InterpolationMode = InterpolationMode.High;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    Rectangle borderRectangle = new Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - border.Thickness - 1, e.AffectedBounds.Height - border.Thickness - 1);
                    GraphicsPath borderPath = new GraphicsPath();
                    borderPath.AddRectangle(borderRectangle);
                    borderPath.CloseAllFigures();

                    e.Graphics.SetClip(borderPath);
                    e.Graphics.DrawPath(new Pen(border.Color), borderPath);
                    e.Graphics.ResetClip();
                }
            }

            #endregion
        }

        #endregion
    }
}