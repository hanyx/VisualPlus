namespace VisualPlus.Toolkit.Controls
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
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.ActionList;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    // TODO: Clear Button
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(TextBox))]
    [DefaultEvent("TextChanged")]
    [DefaultProperty("Text")]
    [Description("The Visual TextBox")]
    [Designer(typeof(VisualTextBoxTasks))]
    public class VisualTextBox : ContainedControlBase, IInputMethods
    {
        #region Variables

        private Border _buttonBorder;
        private Color _buttonColor;
        private Font _buttonFont;
        private Rectangle _buttonRectangle;
        private string _buttontext;
        private bool _buttonVisible;
        private ControlColorState _controlColorState;
        private Image _image;
        private Rectangle _imageRectangle;
        private bool _imageVisible;
        private int _imageWidth;
        private TextBox _textBox;
        private int _textWidth;
        private Watermark _watermark;
        private Panel _waterMarkContainer;
        private int _xValue;
        private int _yValue;

        #endregion

        #region Constructors

        public VisualTextBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);

            // Contains another control
            SetStyle(ControlStyles.ContainerControl, true);

            // Cannot select this control, only the child and does not generate a click event
            SetStyle(ControlStyles.Selectable | ControlStyles.StandardClick, false);

            _textWidth = 125;

            _textBox = new TextBox
                {
                    Size = new Size(_textWidth, 25),
                    Location = new Point(VisualBorderRenderer.GetBorderDistance(ControlBorder), VisualBorderRenderer.GetBorderDistance(ControlBorder)),
                    Text = string.Empty,
                    BorderStyle = BorderStyle.None,
                    TextAlign = HorizontalAlignment.Left,
                    Font = Font,
                    ForeColor = ForeColor,
                    BackColor = Background,
                    Multiline = false
                };

            _imageWidth = 35;
            _buttonFont = Font;

            _buttontext = "visualButton";

            _image = null;

            _watermark = new Watermark();

            _controlColorState = new ControlColorState();
            _buttonBorder = new Border();

            BackColor = Color.Transparent;

            AutoSize = true;
            Size = new Size(135, 25);

            _textBox.KeyDown += TextBox_KeyDown;
            _textBox.Leave += OnLeave;
            _textBox.Enter += OnEnter;
            _textBox.GotFocus += OnEnter;
            _textBox.LostFocus += OnLeave;
            _textBox.MouseLeave += OnLeave;
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.SizeChanged += TextBox_SizeChanged;

            Controls.Add(_textBox);

            _waterMarkContainer = null;

            if (_watermark.Visible)
            {
                DrawWaterMark();
            }
        }

        public delegate void ButtonClickedEventHandler();

        public event ButtonClickedEventHandler ButtonClicked;

        #endregion

        #region Properties

        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Localizable(true)]
        [Browsable(true)]
        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.AutoCompleteCustomSource)]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get
            {
                return _textBox.AutoCompleteCustomSource;
            }

            set
            {
                _textBox.AutoCompleteCustomSource = value;
            }
        }

        [DefaultValue(typeof(AutoCompleteMode), "None")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.AutoCompleteMode)]
        public AutoCompleteMode AutoCompleteMode
        {
            get
            {
                return _textBox.AutoCompleteMode;
            }

            set
            {
                _textBox.AutoCompleteMode = value;
            }
        }

        [DefaultValue(typeof(AutoCompleteSource), "None")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.AutoCompleteSource)]
        public AutoCompleteSource AutoCompleteSource
        {
            get
            {
                return _textBox.AutoCompleteSource;
            }

            set
            {
                _textBox.AutoCompleteSource = value;
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Appearance)]
        public Border ButtonBorder
        {
            get
            {
                return _buttonBorder;
            }

            set
            {
                _buttonBorder = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Appearance)]
        public ControlColorState ButtonColor
        {
            get
            {
                return _controlColorState;
            }

            set
            {
                _controlColorState = value;
                Invalidate();
            }
        }

        [Description(Localization.Descriptions.Property.Description.Strings.Font)]
        [Category(Property.Appearance)]
        public Font ButtonFont
        {
            get
            {
                return _buttonFont;
            }

            set
            {
                _buttonFont = value;
                Invalidate();
            }
        }

        public string ButtonText
        {
            get
            {
                return _buttontext;
            }

            set
            {
                _buttontext = value;
                Invalidate();
            }
        }

        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.Visible)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(false)]
        [Description("Gets access to the contained control.")]
        public Control ContainedControl
        {
            get
            {
                return _textBox;
            }
        }

        public new Font Font
        {
            get
            {
                return base.Font;
            }

            set
            {
                _textBox.Font = value;
                base.Font = value;
            }
        }

        public new Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }

            set
            {
                _textBox.ForeColor = value;
                base.ForeColor = value;
            }
        }

        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.Image)]
        public Image Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
                Invalidate();
            }
        }

        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.Image)]
        public bool ImageVisible
        {
            get
            {
                return _imageVisible;
            }

            set
            {
                if (_imageVisible != value)
                {
                    _imageVisible = value;
                    Invalidate();
                }
            }
        }

        [Category(Property.Layout)]
        [Description(Localization.Descriptions.Property.Description.Common.Size)]
        public int ImageWidth
        {
            get
            {
                return _imageWidth;
            }

            set
            {
                _imageWidth = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(false)]
        [Description("Gets whether the mouse is on the button.")]
        public bool MouseOnButton { get; private set; }

        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.MultiLine)]
        [DefaultValue(false)]
        public virtual bool MultiLine
        {
            get
            {
                return _textBox.Multiline;
            }

            set
            {
                _textBox.Multiline = value;
                Invalidate();
            }
        }

        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.ReadOnly)]
        public bool ReadOnly
        {
            get
            {
                return _textBox.ReadOnly;
            }

            set
            {
                _textBox.ReadOnly = value;
            }
        }

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(false)]
        public new string Text
        {
            get
            {
                return _textBox.Text;
            }

            set
            {
                if (_textBox.Text != value)
                {
                    if (!MultiLine)
                    {
                        string text = RemoveLineBreaks(value);
                        value = text;
                    }

                    _textBox.Text = value;
                    base.Text = value;
                }

                if (_watermark.Visible)
                {
                    // If the text of the text box is not empty.
                    if (_textBox.TextLength > 0)
                    {
                        // Remove the watermark
                        RemoveWaterMark();
                    }
                    else
                    {
                        // But if the text is empty, draw the watermark again.
                        DrawWaterMark();
                    }
                }
            }
        }

        [DefaultValue(125)]
        [Category(Property.Layout)]
        [Description(Localization.Descriptions.Property.Description.Common.Size)]
        public int TextBoxWidth
        {
            get
            {
                return _textWidth;
            }

            set
            {
                _textBox.Width = value;
                _textWidth = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(WatermarkConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Behavior)]
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

        /// <summary>Appends text to the current text of a rich text box.</summary>
        /// <param name="text">The text to append to the current contents of the text box.</param>
        public void AppendText(string text)
        {
            _textBox.AppendText(text);
        }

        /// <summary>Clears all text from the text box control.</summary>
        public void Clear()
        {
            _textBox.Clear();
        }

        /// <summary>Clears information about the most recent operation from the undo buffer of the rich text box.</summary>
        public void ClearUndo()
        {
            _textBox.ClearUndo();
        }

        /// <summary>Copies the current selection in the text box to the Clipboard.</summary>
        public void Copy()
        {
            _textBox.Copy();
        }

        /// <summary>Moves the current selection in the text box to the Clipboard.</summary>
        public void Cut()
        {
            _textBox.Cut();
        }

        /// <summary>
        ///     Specifies that the value of the SelectionLength property is zero so that no characters are selected in the
        ///     control.
        /// </summary>
        public void DeselectAll()
        {
            _textBox.DeselectAll();
        }

        /// <summary>Retrieves the character that is closest to the specified location within the control.</summary>
        /// <param name="pt">The location from which to seek the nearest character.</param>
        /// <returns>The character at the specified location.</returns>
        public int GetCharFromPosition(Point pt)
        {
            return _textBox.GetCharFromPosition(pt);
        }

        /// <summary>Retrieves the index of the character nearest to the specified location.</summary>
        /// <param name="pt">The location to search.</param>
        /// <returns>The zero-based character index at the specified location.</returns>
        public int GetCharIndexFromPosition(Point pt)
        {
            return _textBox.GetCharIndexFromPosition(pt);
        }

        /// <summary>Retrieves the index of the first character of a given line.</summary>
        /// <param name="lineNumber">The line for which to get the index of its first character.</param>
        /// <returns>The zero-based character index in the specified line.</returns>
        public int GetFirstCharIndexFromLine(int lineNumber)
        {
            return _textBox.GetFirstCharIndexFromLine(lineNumber);
        }

        /// <summary>Retrieves the index of the first character of the current line.</summary>
        /// <returns>The zero-based character index in the current line.</returns>
        public int GetFirstCharIndexOfCurrentLine()
        {
            return _textBox.GetFirstCharIndexOfCurrentLine();
        }

        /// <summary>Retrieves the line number from the specified character position within the text of the RichTextBox control.</summary>
        /// <param name="index">The character index position to search.</param>
        /// <returns>The zero-based line number in which the character index is located.</returns>
        public int GetLineFromCharIndex(int index)
        {
            return _textBox.GetLineFromCharIndex(index);
        }

        /// <summary>Retrieves the location within the control at the specified character index.</summary>
        /// <param name="index">The index of the character for which to retrieve the location.</param>
        /// <returns>The location of the specified character.</returns>
        public Point GetPositionFromCharIndex(int index)
        {
            return _textBox.GetPositionFromCharIndex(index);
        }

        /// <summary>Replaces the current selection in the text box with the contents of the Clipboard.</summary>
        public void Paste()
        {
            _textBox.Paste();
        }

        /// <summary>Scrolls the contents of the control to the current caret position.</summary>
        public void ScrollToCaret()
        {
            _textBox.ScrollToCaret();
        }

        /// <summary>Selects a range of text in the control.</summary>
        /// <param name="start">The position of the first character in the current text selection within the text box.</param>
        /// <param name="length">The number of characters to select.</param>
        public void Select(int start, int length)
        {
            _textBox.Select(start, length);
        }

        /// <summary>Selects all text in the control.</summary>
        public void SelectAll()
        {
            _textBox.SelectAll();
        }

        /// <summary>Undoes the last edit operation in the text box.</summary>
        public void Undo()
        {
            _textBox.Undo();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (_watermark.Visible)
            {
                // If focused use focus color
                _watermark.Brush = new SolidBrush(_watermark.ActiveColor);

                // Don't draw watermark if contains text.
                if (_textBox.TextLength <= 0)
                {
                    RemoveWaterMark();
                    DrawWaterMark();
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _textBox.Focus();
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);

            // Check if there is a watermark
            if (_waterMarkContainer != null)
            {
                // if there is a watermark it should also be invalidated();
                _waterMarkContainer.Invalidate();
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            MouseState = MouseStates.Normal;

            if (_watermark.Visible)
            {
                // If the user has written something and left the control
                if (_textBox.TextLength > 0)
                {
                    // Remove the watermark
                    RemoveWaterMark();
                }
                else
                {
                    // But if the user didn't write anything, then redraw the control.
                    Invalidate();
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnMouseClick(e);

            if (MouseOnButton)
            {
                MouseState = MouseStates.Down;
            }
            else
            {
                MouseState = MouseStates.Hover;
            }

            if (_buttonVisible)
            {
                // Check if mouse in X position.
                if ((_xValue > _buttonRectangle.X) && (_xValue < Width))
                {
                    // Determine the button middle separator by checking for the Y position.
                    if ((_yValue > _buttonRectangle.Y) && (_yValue < Height))
                    {
                        ButtonClicked?.Invoke();
                    }
                }
            }

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (!_textBox.Focused)
            {
                MouseState = MouseStates.Normal;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _xValue = e.Location.X;
            _yValue = e.Location.Y;
            MouseOnButton = GDI.IsMouseInBounds(e.Location, _buttonRectangle);

            Invalidate();

            // IBeam cursor toggle
            if ((e.X > _textBox.Location.X) && (e.X < _textBox.Right))
            {
                Cursor = Cursors.IBeam;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            if (_textBox.BackColor != Background)
            {
                _textBox.BackColor = Background;
            }

            _buttonRectangle = new Rectangle(_textBox.Right, 0, Width, Height);
            _imageRectangle = new Rectangle(0, 0, _imageWidth, Height - 1);

            if (!_textBox.Multiline)
            {
                if (_imageVisible)
                {
                    _textBox.Location = new Point(VisualBorderRenderer.GetBorderDistance(ControlBorder) + _imageRectangle.Width, _textBox.Location.Y);

                    DrawImage(graphics);

                    if (_buttonVisible)
                    {
                        DrawButton(graphics);
                    }
                }
                else
                {
                    _textBox.Location = new Point(VisualBorderRenderer.GetBorderDistance(ControlBorder), _textBox.Location.Y);

                    if (_buttonVisible)
                    {
                        DrawButton(graphics);
                    }
                }
            }

            graphics.ResetClip();

            if (_watermark.Visible)
            {
                DrawWaterMark();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (!_textBox.Multiline)
            {
                if (_imageVisible)
                {
                    _textBox.Location = new Point(VisualBorderRenderer.GetBorderDistance(ControlBorder) + _imageWidth, _textBox.Location.Y);
                }
                else
                {
                    _textBox.Location = new Point(VisualBorderRenderer.GetBorderDistance(ControlBorder), _textBox.Location.Y);
                }

                if ((!_imageVisible & !_buttonVisible) && AutoSize)
                {
                    _textBox.Width = GetInternalControlSize(Size, ControlBorder).Width;
                }

                _textBox.Height = GetTextBoxHeight();
                Size = new Size(Width, VisualBorderRenderer.GetBorderDistance(ControlBorder) + _textBox.Height + VisualBorderRenderer.GetBorderDistance(ControlBorder));
            }
            else
            {
                _textBox.Location = GetInternalControlLocation(ControlBorder);
                _textBox.Size = GetInternalControlSize(Size, ControlBorder);
            }

            Invalidate();
        }

        private static string RemoveLineBreaks(string text)
        {
            return text.Replace(Environment.NewLine, " ");
        }

        private void DrawButton(Graphics graphics)
        {
            _buttonColor = new Color();

            if (Enabled)
            {
                switch (MouseState)
                {
                    case MouseStates.Normal:
                        {
                            _buttonColor = _controlColorState.Color;
                            break;
                        }

                    case MouseStates.Hover:
                        {
                            _buttonColor = _controlColorState.Hover;
                            break;
                        }

                    case MouseStates.Down:
                        {
                            _buttonColor = _controlColorState.Pressed;
                            break;
                        }

                    default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                }
            }
            else
            {
                _buttonColor = Enabled ? _controlColorState.Color : _controlColorState.Disabled;
            }

            GraphicsPath buttonPath = new GraphicsPath();
            buttonPath.AddRectangle(_buttonRectangle);
            graphics.SetClip(ControlGraphicsPath);
            graphics.FillPath(new SolidBrush(_buttonColor), buttonPath);
            VisualBorderRenderer.DrawBorderStyle(graphics, ControlBorder, MouseState, buttonPath);
            Size textSize = GDI.MeasureText(graphics, _buttontext, _buttonFont);
            graphics.SetClip(buttonPath);
            graphics.DrawString(_buttontext, Font, new SolidBrush(ForeColor), new PointF(_buttonRectangle.X + 3, (Height / 2) - (textSize.Height / 2)));
            graphics.ResetClip();
        }

        private void DrawImage(Graphics graphics)
        {
            if (_imageVisible)
            {
                GraphicsPath _imagePath = new GraphicsPath();
                _imagePath.AddRectangle(_imageRectangle);
                graphics.SetClip(_imagePath);

                if (_image != null)
                {
                    graphics.DrawImage(_image, new Point((_imageRectangle.X + (_imageRectangle.Width / 2)) - (_image.Width / 2), (_imageRectangle.Y + (_imageRectangle.Height / 2)) - (_image.Height / 2)));
                }

                graphics.ResetClip();
            }
        }

        private void DrawWaterMark()
        {
            if ((_waterMarkContainer == null) && (_textBox.TextLength <= 0))
            {
                _waterMarkContainer = new Panel(); // Creates the new panel instance
                _waterMarkContainer.Paint += WaterMarkContainer_Paint;
                _waterMarkContainer.Invalidate();
                _waterMarkContainer.Click += WaterMarkContainer_Click;
                _textBox.Controls.Add(_waterMarkContainer); // adds the control
                _waterMarkContainer.BringToFront();
            }
        }

        private int GetTextBoxHeight()
        {
            if (_textBox.TextLength > 0)
            {
                return GDI.MeasureText(Text, Font).Height;
            }
            else
            {
                return GDI.MeasureText("Hello World.", Font).Height;
            }
        }

        private void OnEnter(object sender, EventArgs e)
        {
            MouseState = MouseStates.Hover;
        }

        private void OnLeave(object sender, EventArgs e)
        {
            if (!_textBox.Focused)
            {
                MouseState = MouseStates.Normal;
            }
        }

        private void RemoveWaterMark()
        {
            if (_waterMarkContainer != null)
            {
                _textBox.Controls.Remove(_waterMarkContainer);
                _waterMarkContainer = null;
            }
        }

        private void TextBox_KeyDown(object obj, KeyEventArgs e)
        {
            // Select all
            if (e.Control && (e.KeyCode == Keys.A))
            {
                _textBox.SelectAll();
                e.SuppressKeyPress = true;
            }

            // Copy
            if (e.Control && (e.KeyCode == Keys.C))
            {
                _textBox.Copy();
                e.SuppressKeyPress = true;
            }
        }

        private void TextBox_SizeChanged(object sender, EventArgs e)
        {
            _textWidth = _textBox.Width;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (_watermark.Visible)
            {
                // If the text of the text box is not empty.
                if (_textBox.TextLength > 0)
                {
                    // Remove the watermark
                    RemoveWaterMark();
                }
                else
                {
                    // But if the text is empty, draw the watermark again.
                    DrawWaterMark();
                }
            }
        }

        private void WaterMarkContainer_Click(object sender, EventArgs e)
        {
            _textBox.Focus();
        }

        private void WaterMarkContainer_Paint(object sender, PaintEventArgs e)
        {
            // Configure the watermark
            _waterMarkContainer.Location = new Point(0, 0);
            _waterMarkContainer.Height = _textBox.Height;
            _waterMarkContainer.Width = _textBox.Width;

            // Forces it to resize with the parent control
            _waterMarkContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            // Set color
            _watermark.Brush = ContainsFocus ? new SolidBrush(_watermark.ActiveColor) : new SolidBrush(_watermark.InactiveColor);

            // Draws the string on the panel
            e.Graphics.DrawString(_watermark.Text, _watermark.Font, _watermark.Brush, new PointF(0F, 0F));
        }

        #endregion
    }
}