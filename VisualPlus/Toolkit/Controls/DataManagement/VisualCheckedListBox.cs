namespace VisualPlus.Toolkit.Controls.DataManagement
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.ActionList;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(CheckedListBox))]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Items")]
    [DefaultBindingProperty("Items")]
    [Description("The Visual CheckedListBox")]
    [Designer(typeof(VisualCheckedListBoxTasks))]
    public class VisualCheckedListBox : ContainedControlBase
    {
        #region Variables

        private bool _alternateColors;
        private Border _border;
        private Size _box;
        private int _boxSpacing;
        private CheckedListBox _checkedListBox;
        private ColorState _colorState;
        private Color _itemAlternate;
        private Color _itemNormal;
        private Color _itemSelected;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualCheckedListBox" /> class.</summary>
        public VisualCheckedListBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);

            // Contains another control
            SetStyle(ControlStyles.ContainerControl, true);

            // Cannot select this control, only the child and does not generate a click event
            SetStyle(ControlStyles.Selectable | ControlStyles.StandardClick, false);

            _colorState = new ColorState();

            _border = new Border();

            _alternateColors = true;

            _box = new Size(25, 25);
            _boxSpacing = 5;

            _checkedListBox = new CheckedListBox
                {
                    Size = GetInternalControlSize(Size, _border),
                    Location = GetInternalControlLocation(_border),
                    BorderStyle = BorderStyle.None,
                    Font = Font,
                    ForeColor = ForeColor,
                    BackColor = _colorState.Enabled
                };

            Size = new Size(150, 100);

            // _checkedListBox.DrawItem += CheckedListBox_DrawItem;
            // _checkedListBox.MeasureItem += CheckedListBox_MeasureItem;
            Controls.Add(_checkedListBox);

            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        #endregion

        #region Properties

        [Category(Propertys.Behavior)]
        [Description(Property.Toggle)]
        public bool AlternateColors
        {
            get
            {
                return _alternateColors;
            }

            set
            {
                _alternateColors = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(ColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(false)]
        [Description("Gets access to the contained control.")]
        public CheckedListBox ContainedControl => _checkedListBox;

        [Category(Propertys.Appearance)]
        [Description(Property.Font)]
        public new Font Font
        {
            get
            {
                return base.Font;
            }

            set
            {
                _checkedListBox.Font = value;
                base.Font = value;
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public new Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }

            set
            {
                _checkedListBox.ForeColor = value;
                base.ForeColor = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Browsable(true)]
        [Category(Propertys.Data)]
        [Description("Gets or sets the format-specifier characters that indicate how the value is to be displayed.")]
        public string FormatString
        {
            get
            {
                return _checkedListBox.FormatString;
            }

            set
            {
                _checkedListBox.FormatString = value;
            }
        }

        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category(Propertys.Behavior)]
        [Description("Gets or sets a value indicating whether the formatting is applied to the DisplayMember propery of the ListControl.")]
        public bool FormattingEnabled
        {
            get
            {
                return _checkedListBox.FormattingEnabled;
            }

            set
            {
                _checkedListBox.FormattingEnabled = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category(Propertys.Behavior)]
        [Description("Gets or sets the width by which the horizontal scroll bar of a ListBox can scroll.")]
        public int HorizontalExtent
        {
            get
            {
                return _checkedListBox.HorizontalExtent;
            }

            set
            {
                _checkedListBox.HorizontalExtent = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category(Propertys.Behavior)]
        [Description("Gets or sets a value indicating whether a horizontal scroll bar is displayed in the control.")]
        public bool HorizontalScrollbar
        {
            get
            {
                return _checkedListBox.HorizontalScrollbar;
            }

            set
            {
                _checkedListBox.HorizontalScrollbar = value;
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ItemNormal
        {
            get
            {
                return _itemNormal;
            }

            set
            {
                _itemNormal = value;
                Invalidate();
            }
        }

        [Category(Propertys.Data)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual CheckedListBox.ObjectCollection Items
        {
            get
            {
                return _checkedListBox.Items;
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ItemSelected
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

        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category(Propertys.Behavior)]
        [Description("Gets or sets a value specifying the selection mode.")]
        public SelectionMode SelectionMode
        {
            get
            {
                return _checkedListBox.SelectionMode;
            }

            set
            {
                _checkedListBox.SelectionMode = value;
            }
        }

        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category(Propertys.Behavior)]
        [Description("Gets or sets a value indicating whether the items in the ListBox are sorted alphabetically.")]
        public bool Sorted
        {
            get
            {
                return _checkedListBox.Sorted;
            }

            set
            {
                _checkedListBox.Sorted = value;
            }
        }

        #endregion

        #region Events

        public void UpdateTheme(Styles style)
        {
            StyleManager = new VisualStyleManager(style);

            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;

            _colorState.Enabled = StyleManager.ControlStyle.Background(3);
            _colorState.Disabled = StyleManager.ControlStyle.Background(0);

            _border.Color = StyleManager.ShapeStyle.Color;
            _border.HoverColor = StyleManager.BorderStyle.HoverColor;

            _itemAlternate = Color.Gray;
            _itemNormal = StyleManager.ControlStyle.ItemEnabled;
            _itemSelected = StyleManager.BorderStyle.HoverColor;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            ControlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);

            Color _backColor = Enabled ? _colorState.Enabled : _colorState.Disabled;

            if (_checkedListBox.BackColor != _backColor)
            {
                _checkedListBox.BackColor = _backColor;
            }

            e.Graphics.SetClip(ControlGraphicsPath);
            VisualBackgroundRenderer.DrawBackground(e.Graphics, _backColor, BackgroundImage, MouseState, _clientRectangle, Border);
            VisualBorderRenderer.DrawBorderStyle(e.Graphics, _border, ControlGraphicsPath, MouseState);
            e.Graphics.ResetClip();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(BackColor);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _checkedListBox.Location = GetInternalControlLocation(_border);
            _checkedListBox.Size = GetInternalControlSize(Size, _border);
        }

        private void CheckedListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // We cannot do anything with an invalid index
            if (e.Index <= 0)
            {
                return;
            }

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint;

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            if (e.Index > -1)
            {
                Color color;

                if (_alternateColors)
                {
                    if (isSelected)
                    {
                        color = _itemSelected;
                    }
                    else
                    {
                        if (e.Index % 2 == 0)
                        {
                            color = _itemNormal;
                        }
                        else
                        {
                            color = _itemAlternate;
                        }
                    }
                }
                else
                {
                    if (isSelected)
                    {
                        color = _itemSelected;
                    }
                    else
                    {
                        color = _itemNormal;
                    }
                }

                // Background item brush
                SolidBrush backgroundBrush = new SolidBrush(color);

                // Text color brush
                SolidBrush textBrush = new SolidBrush(e.ForeColor);

                // Draw the background
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

                // Draw checkbox 
                Rectangle _boxRect = new Rectangle(new Point(e.Bounds.X, e.Bounds.Y + 0), _box);

                // CheckBoxRenderer.DrawCheckBox(g, new Point(b.X + checkPad, b.Y + checkPad), new Rectangle(new Point(b.X + b.Height, b.Y), new Size(b.Width - b.Height, b.Height)), text, this.Font, TextFormatFlags.Left, false, state);
                LinearGradientBrush _boxBrush = GDI.GetControlBrush(graphics, Enabled, MouseState, ControlBrushCollection, new Rectangle(e.Bounds.Location, e.Bounds.Size));

                Size textSize = GDI.MeasureText(graphics, Text, Font);
                Point textPoint = new Point(_boxRect.Right + _boxSpacing, (_boxRect.Y + (_boxRect.Height / 2)) - (textSize.Height / 2));

                // VisualToggleRenderer.DrawCheckBox(graphics, _border, CheckMark, _boxRect, GetItemChecked(e.Index), Enabled, _boxBrush, MouseState, GetItemText(Items[e.Index].ToString()), e.Font, ForeColor, textPoint);

                // Draw the text
                // e.Graphics.DrawString(_checkedListBox.GetItemText(Items[e.Index].ToString()), e.Font, new SolidBrush(ForeColor), new RectangleF(e.Bounds.Location, e.Bounds.Size), StringFormat.GenericDefault);

                // Clean up
                backgroundBrush.Dispose();
                textBrush.Dispose();
            }
        }

        private void CheckedListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}