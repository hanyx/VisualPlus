namespace VisualPlus.Toolkit.Controls.DataManagement
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Items")]
    [Description("The Visual ListView")]
    [Designer(typeof(VisualListViewDesigner))]
    [ToolboxBitmap(typeof(ListView), "Resources.ToolboxBitmaps.VisualListView.bmp")]
    [ToolboxItem(true)]
    public class VisualListView : ContainedControlBase, IThemeSupport
    {
        #region Variables

        private Border _border;
        private ColorState _colorState;
        private Color _columnHeaderColor;
        private Color _itemEnabled;
        private Color _itemSelected;
        private ListView _listView;
        private bool _standardHeader;
        private Font headerFont;
        private Color headerText;
        private int itemPadding = 12;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualListView" /> class.</summary>
        public VisualListView()
        {
            // Contains another control
            SetStyle(ControlStyles.ContainerControl, true);

            // Cannot select this control, only the child ListView and does not generate a click event
            SetStyle(ControlStyles.Selectable | ControlStyles.StandardClick, false);

            _border = new Border();

            ThemeManager = new StylesManager(Settings.DefaultValue.DefaultStyle);
            headerFont = ThemeManager.Theme.TextSetting.Font;
            _colorState = new ColorState
                {
                    Enabled = ThemeManager.Theme.BackgroundSettings.Type4
                };

            _listView = new ListView
                {
                    BackColor = _colorState.Enabled,
                    Size = GetInternalControlSize(Size, _border),
                    BorderStyle = BorderStyle.None,
                    View = View.Details,
                    MultiSelect = false,
                    LabelEdit = false,
                    AllowColumnReorder = false,
                    CheckBoxes = false,
                    FullRowSelect = true,
                    GridLines = true,
                    HeaderStyle = ColumnHeaderStyle.Nonclickable,
                    OwnerDraw = true,
                    Location = GetInternalControlLocation(_border)
                };

            AutoSize = true;
            Size = new Size(250, 150);

            // _listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            // _listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            _listView.DrawColumnHeader += ListView_DrawColumnHeader;
            _listView.DrawItem += ListView_DrawItem;
            _listView.DrawSubItem += ListView_DrawSubItem;

            Controls.Add(_listView);

            UpdateTheme(ThemeManager.Theme);
        }

        #endregion

        #region Properties

        [Category("Behaviour")]
        [Description("Gets or sets a value indicating whether the user can drag column headers to reorder columns in the control.")]
        [DefaultValue(false)]
        public virtual bool AllowColumnReorder
        {
            get
            {
                return _listView.AllowColumnReorder;
            }

            set
            {
                _listView.AllowColumnReorder = value;
            }
        }

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
                _listView.BackColor = value.Enabled;
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

        [Browsable(false)]
        [Description("Gets the indexes of the currently checked items in the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ListView.CheckedIndexCollection CheckedIndices
        {
            get
            {
                return _listView.CheckedIndices;
            }
        }

        [Browsable(false)]
        [Description("Gets the currently checked items in the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ListView.CheckedListViewItemCollection CheckedItems
        {
            get
            {
                return _listView.CheckedItems;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color ColumnHeaderColor
        {
            get
            {
                return _columnHeaderColor;
            }

            set
            {
                _columnHeaderColor = value;
                Invalidate();
            }
        }

        [Category("Data")]
        [Description("The items in the VisualListView.")]
        [Editor("System.Windows.Forms.Design.ColumnHeaderCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual ListView.ColumnHeaderCollection Columns
        {
            get
            {
                return _listView.Columns;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(false)]
        [Description("Gets access to the contained control.")]
        public ListView ContainedControl
        {
            get
            {
                return _listView;
            }
        }

        [Category("Appearance")]
        [Description("Gets or sets a value indicating whether clicking an item selects all its subitems.")]
        [DefaultValue(true)]
        public virtual bool FullRowSelect
        {
            get
            {
                return _listView.FullRowSelect;
            }

            set
            {
                _listView.FullRowSelect = value;
            }
        }

        [Category("Appearance")]
        [Description("Gets or sets a value indicating whether grid lines appear between the rows and columns containing the items and subitems in the control.")]
        [DefaultValue(true)]
        public virtual bool GridLines
        {
            get
            {
                return _listView.GridLines;
            }

            set
            {
                _listView.GridLines = value;
            }
        }

        [Category("Data")]
        [Description("The items in the VisualListView.")]
        [Editor("System.Windows.Forms.Design.ListViewGroupCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual ListViewGroupCollection Groups
        {
            get
            {
                return _listView.Groups;
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Font)]
        public Font HeaderFont
        {
            get
            {
                return headerFont;
            }

            set
            {
                headerFont = value;
                Invalidate();
            }
        }

        [Category("Behaviour")]
        [Description("Gets or sets the column header style.")]
        [DefaultValue(true)]
        public virtual ColumnHeaderStyle HeaderStyle
        {
            get
            {
                return _listView.HeaderStyle;
            }

            set
            {
                _listView.HeaderStyle = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color HeaderText
        {
            get
            {
                return headerText;
            }

            set
            {
                headerText = value;
                Invalidate();
            }
        }

        [Category("Behaviour")]
        [Description("Gets or sets a value indicating whether the selected item in the control remains highlighted when the control loses focus.")]
        [DefaultValue(true)]
        public virtual bool HideSelection
        {
            get
            {
                return _listView.HideSelection;
            }

            set
            {
                _listView.HideSelection = value;
            }
        }

        [Category("Behaviour")]
        [Description("Gets or sets a value indicating whether the text of an item or subitem has the appearance of a hyperlink when the mouse pointer passes over it.")]
        [DefaultValue(false)]
        public virtual bool HotTracking
        {
            get
            {
                return _listView.HotTracking;
            }

            set
            {
                _listView.HotTracking = value;
            }
        }

        [Category("Behaviour")]
        [Description("Gets or sets a value indicating whether an item is automatically selected when the mouse pointer remains over the item for a few seconds.")]
        [DefaultValue(false)]
        public virtual bool HoverSelection
        {
            get
            {
                return _listView.HoverSelection;
            }

            set
            {
                _listView.HoverSelection = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color ItemBackground
        {
            get
            {
                return _itemEnabled;
            }

            set
            {
                _itemEnabled = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        public int ItemPadding
        {
            get
            {
                return itemPadding;
            }

            set
            {
                itemPadding = value;
                Invalidate();
            }
        }

        [Category("Data")]
        [Description("The items in the VisualListView.")]
        [Editor("System.Windows.Forms.Design.ListViewItemCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual ListView.ListViewItemCollection Items
        {
            get
            {
                return _listView.Items;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color ItemSelectedColor
        {
            get
            {
                return _itemSelected;
            }

            set
            {
                _itemSelected = value;
                Invalidate();
            }
        }

        [Category("Behaviour")]
        [Description("Gets or sets a value indicating whether the user can edit the labels of items in the control.")]
        [DefaultValue(false)]
        public virtual bool LabelEdit
        {
            get
            {
                return _listView.LabelEdit;
            }

            set
            {
                _listView.LabelEdit = value;
            }
        }

        [Category("Behaviour")]
        [Description("Gets or sets a value indicating whether item labels wrap when items are displayed in the control as icons.")]
        [DefaultValue(false)]
        public virtual bool LabelWrap
        {
            get
            {
                return _listView.LabelWrap;
            }

            set
            {
                _listView.LabelWrap = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description("Gets or sets the ImageList to use when displaying items as small icons in the control.")]
        public virtual ImageList LargeImageList
        {
            get
            {
                return _listView.LargeImageList;
            }

            set
            {
                _listView.LargeImageList = value;
            }
        }

        [Category("Behaviour")]
        [Description("Gets or sets a value indicating whether multiple items can be selected.")]
        [DefaultValue(false)]
        public virtual bool MultiSelect
        {
            get
            {
                return _listView.MultiSelect;
            }

            set
            {
                _listView.MultiSelect = value;
            }
        }

        [Browsable(false)]
        [Description("Gets a collection that contains the zero-based indexes of all currently selected items in the VisualListBox.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListView.SelectedIndexCollection SelectedIndices
        {
            get
            {
                return _listView.SelectedIndices;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListView.SelectedListViewItemCollection SelectedItems
        {
            get
            {
                return _listView.SelectedItems;
            }
        }

        [Category("Behaviour")]
        [Description("Gets or sets a value indicating whether items are displayed in groups.")]
        [DefaultValue(false)]
        public virtual bool ShowGroups
        {
            get
            {
                return _listView.ShowGroups;
            }

            set
            {
                _listView.ShowGroups = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description("Gets or sets the ImageList to use when displaying items as small icons in the control.")]
        public virtual ImageList SmallImageList
        {
            get
            {
                return _listView.SmallImageList;
            }

            set
            {
                _listView.SmallImageList = value;
            }
        }

        [Category("Behavior")]
        [Description("Gets or sets the sort order for items in the control.")]
        [DefaultValue(false)]
        public virtual SortOrder Sorting
        {
            get
            {
                return _listView.Sorting;
            }

            set
            {
                _listView.Sorting = value;
            }
        }

        [DefaultValue(false)]
        [Category(PropertyCategory.Behavior)]
        [Description("Draws the background of the column header.")]
        public bool StandardHeader
        {
            get
            {
                return _standardHeader;
            }

            set
            {
                _standardHeader = value;
                Invalidate();
            }
        }

        [Category("Behavior")]
        [Description("Gets or sets the size of the tiles shown in tile view.")]
        [DefaultValue(false)]
        public virtual Size TileSize
        {
            get
            {
                return _listView.TileSize;
            }

            set
            {
                _listView.TileSize = value;
            }
        }

        [Browsable(false)]
        [Description("Gets or sets the first visible item in the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListViewItem TopItem
        {
            get
            {
                return _listView.TopItem;
            }

            set
            {
                _listView.TopItem = value;
            }
        }

        [Category("Appearance")]
        [Description("Selects one of five different views that can be shown in.")]
        [DefaultValue(false)]
        public virtual View View
        {
            get
            {
                return _listView.View;
            }

            set
            {
                _listView.View = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Point LastPosition { get; set; }

        #endregion

        #region Events

        public void UpdateTheme(Theme theme)
        {
            try
            {
                _border.Color = theme.BorderSettings.Normal;
                _border.HoverColor = theme.BorderSettings.Hover;

                ForeColor = theme.TextSetting.Enabled;
                TextStyle.Enabled = theme.TextSetting.Enabled;
                TextStyle.Disabled = theme.TextSetting.Disabled;

                Font = theme.TextSetting.Font;

                _itemEnabled = theme.ListItemSettings.Item;
                _itemSelected = theme.ListItemSettings.ItemSelected;

                _columnHeaderColor = theme.OtherSettings.ColumnHeader;
                headerText = theme.OtherSettings.ColumnText;

                _colorState = new ColorState
                    {
                        Enabled = theme.BackgroundSettings.Type4,
                        Disabled = theme.BackgroundSettings.Type1
                    };
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }

            Invalidate();
            OnThemeChanged(new ThemeEventArgs(theme));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            LastPosition = e.Location;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            ControlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);

            Color _backColor = Enabled ? BackColorState.Enabled : BackColorState.Disabled;

            if (_listView.BackColor != _backColor)
            {
                _listView.BackColor = _backColor;
            }

            e.Graphics.SetClip(ControlGraphicsPath);
            VisualBackgroundRenderer.DrawBackground(e.Graphics, _backColor, BackgroundImage, MouseState, _clientRectangle, Border);
            VisualBorderRenderer.DrawBorderStyle(e.Graphics, _border, ControlGraphicsPath, MouseState);
            e.Graphics.ResetClip();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(Parent.BackColor);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _listView.Location = GetInternalControlLocation(_border);
            _listView.Size = GetInternalControlSize(Size, _border);
        }

        private static StringFormat GetStringFormat()
        {
            return new StringFormat
                {
                    FormatFlags = StringFormatFlags.LineLimit,
                    Trimming = StringTrimming.EllipsisCharacter,
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
        }

        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextStyle.TextRenderingHint;

            Rectangle _columnHeaderRectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, Width - 1, e.Bounds.Height - 1);

            // Draws the column header background.
            if (_standardHeader)
            {
                e.DrawBackground();
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(_columnHeaderColor), _columnHeaderRectangle);
            }

            StringFormat _stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

            Rectangle _textRectangle = new Rectangle(e.Bounds.X + itemPadding, e.Bounds.Y + itemPadding, e.Bounds.Width - (itemPadding * 2), e.Bounds.Height - (itemPadding * 2));

            // Draw the header text.
            e.Graphics.DrawString(e.Header.Text, headerFont, new SolidBrush(headerText), _textRectangle, _stringFormat);
        }

        private void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // We draw the current line of items (= item with sub items) on a temp bitmap, then draw the bitmap at once. This is to reduce flickering.
            Bitmap bitmap = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            // always draw default background
            graphics.FillRectangle(new SolidBrush(BackColorState.Enabled), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));

            if (e.State.HasFlag(ListViewItemStates.Selected))
            {
                // selected background
                graphics.FillRectangle(new SolidBrush(_itemSelected), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
            }

            // Draw separator
            // graphics.DrawLine(new Pen(StyleManager.BorderStyle.Color), e.Bounds.Left, 0, e.Bounds.Right, 0);
            foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
            {
                // Draw text
                graphics.DrawString(subItem.Text, Font, new SolidBrush(Color.Black), new Rectangle(subItem.Bounds.X + itemPadding, itemPadding, subItem.Bounds.Width - (2 * itemPadding), subItem.Bounds.Height - (2 * itemPadding)), GetStringFormat());
            }

            // Draw the item text for views other than the Details view
            if (_listView.View != View.Details)
            {
                e.DrawText();
            }

            e.Graphics.DrawImage((Image)bitmap.Clone(), new Point(0, e.Item.Bounds.Location.Y));
            graphics.Dispose();
            bitmap.Dispose();
        }

        private void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            TextFormatFlags _textFormatFlags = TextFormatFlags.Left;

            using (StringFormat _stringFormat = new StringFormat())
            {
                // Store the column text alignment, letting it default
                // to Left if it has not been set to Center or Right.
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        _stringFormat.Alignment = StringAlignment.Center;
                        _textFormatFlags = TextFormatFlags.HorizontalCenter;
                        break;
                    case HorizontalAlignment.Right:
                        _stringFormat.Alignment = StringAlignment.Far;
                        _textFormatFlags = TextFormatFlags.Right;
                        break;
                }

                // Draw the text and background for a sub item with a 
                // negative value. 
                double _subItemValue;
                if ((e.ColumnIndex > 0) && double.TryParse(e.SubItem.Text, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out _subItemValue) && (_subItemValue < 0))
                {
                    // Unless the item is selected, draw the standard 
                    // background to make it stand out from the gradient.
                    if ((e.ItemState & ListViewItemStates.Selected) == 0)
                    {
                        e.DrawBackground();
                    }

                    // Draw the sub item text in red to highlight it. 
                    e.Graphics.DrawString(e.SubItem.Text, Font, Brushes.Red, e.Bounds, _stringFormat);

                    return;
                }

                // Draw normal text for a sub item with a non negative 
                // or non numerical value.
                e.DrawText(_textFormatFlags);
            }
        }

        #endregion
    }
}