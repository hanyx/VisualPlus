namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Delegates;
    using VisualPlus.Designer;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("MaximizeVisible")]
    [Description("The Visual ControlBox")]
    [Designer(typeof(VisualControlBoxDesigner))]
    [ToolboxBitmap(typeof(VisualControlBox), "Resources.ToolboxBitmaps.VisualControlBox.bmp")]
    [ToolboxItem(true)]
    public class VisualControlBox : VisualControlBase
    {
        #region Variables

        private ControlColorState _closeBack;
        private ControlColorState _closeFore;
        private MouseStates _closeMouseState;
        private Rectangle _closeRectangle;
        private bool _maximize;
        private ControlColorState _maximizeBack;
        private ControlColorState _maximizeFore;
        private MouseStates _maximizeMouseState;
        private Rectangle _maximizeRectangle;
        private bool _maximizeVisible;
        private bool _minimize;
        private ControlColorState _minimizeBack;
        private ControlColorState _minimizeFore;
        private MouseStates _minimizeMouseState;
        private Rectangle _minimizeRectangle;
        private bool _minimizeVisible;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualControlBox" /> class.</summary>
        public VisualControlBox()
        {
            SetStyle(
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            InitializeControlBox();

            // TODO: Add ThemeSupport
            _closeBack = new ControlColorState
                {
                    Disabled = Color.Transparent,
                    Enabled = Color.Transparent,
                    Hover = Color.FromArgb(183, 40, 40),
                    Pressed = Color.FromArgb(183, 40, 40)
                };
            _closeFore = new ControlColorState
                {
                    Disabled = Color.DimGray,
                    Enabled = Color.Gray,
                    Hover = Color.White,
                    Pressed = Color.White
                };
            _maximizeBack = new ControlColorState
                {
                    Disabled = Color.Transparent,
                    Enabled = Color.Transparent,
                    Hover = Color.FromArgb(238, 238, 238),
                    Pressed = Color.FromArgb(238, 238, 238)
                };
            _maximizeFore = new ControlColorState
                {
                    Disabled = Color.DimGray,
                    Enabled = Color.Gray,
                    Hover = Color.Gray,
                    Pressed = Color.Gray
                };
            _minimizeBack = new ControlColorState
                {
                    Disabled = Color.Transparent,
                    Enabled = Color.Transparent,
                    Hover = Color.FromArgb(238, 238, 238),
                    Pressed = Color.FromArgb(238, 238, 238)
                };
            _minimizeFore = new ControlColorState
                {
                    Disabled = Color.DimGray,
                    Enabled = Color.Gray,
                    Hover = Color.Gray,
                    Pressed = Color.Gray
                };
        }

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event ControlBoxEventHandler CloseClick;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event ControlBoxEventHandler MaximizeClick;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event ControlBoxEventHandler MinimizeClick;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event ControlBoxEventHandler RestoredFormWindow;

        public enum ControlBoxButtons
        {
            /// <summary>The close.</summary>
            Close,

            /// <summary>The maximize.</summary>
            Maximize,

            /// <summary>The minimize.</summary>
            Minimize
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState CloseBackColor
        {
            get
            {
                return _closeBack;
            }

            set
            {
                if (value == _closeBack)
                {
                    return;
                }

                _closeBack = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState CloseForeColor
        {
            get
            {
                return _closeFore;
            }

            set
            {
                if (value == _closeFore)
                {
                    return;
                }

                _closeFore = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.MouseState)]
        public MouseStates CloseMouseState
        {
            get
            {
                return _closeMouseState;
            }

            set
            {
                _closeMouseState = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Toggle)]
        public bool Maximize
        {
            get
            {
                return _maximize;
            }

            set
            {
                _maximize = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState MaximizeBackColor
        {
            get
            {
                return _maximizeBack;
            }

            set
            {
                if (value == _maximizeBack)
                {
                    return;
                }

                _maximizeBack = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState MaximizeForeColor
        {
            get
            {
                return _maximizeFore;
            }

            set
            {
                if (value == _maximizeFore)
                {
                    return;
                }

                _maximizeFore = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.MouseState)]
        public MouseStates MaximizeMouseState
        {
            get
            {
                return _maximizeMouseState;
            }

            set
            {
                _maximizeMouseState = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Visible)]
        public bool MaximizeVisible
        {
            get
            {
                return _maximizeVisible;
            }

            set
            {
                _maximizeVisible = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Toggle)]
        public bool Minimize
        {
            get
            {
                return _minimize;
            }

            set
            {
                _minimize = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState MinimizeBackColor
        {
            get
            {
                return _minimizeBack;
            }

            set
            {
                if (value == _minimizeBack)
                {
                    return;
                }

                _minimizeBack = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState MinimizeForeColor
        {
            get
            {
                return _minimizeFore;
            }

            set
            {
                if (value == _minimizeFore)
                {
                    return;
                }

                _minimizeFore = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.MouseState)]
        public MouseStates MinimizeMouseState
        {
            get
            {
                return _minimizeMouseState;
            }

            set
            {
                _minimizeMouseState = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Visible)]
        public bool MinimizeVisible
        {
            get
            {
                return _minimizeVisible;
            }

            set
            {
                _minimizeVisible = value;
                Invalidate();
            }
        }

        /// <summary>Gets the parent form.</summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Form ParentForm
        {
            get
            {
                return Parent.FindForm();
            }
        }

        #endregion

        #region Events

        /// <summary>The OnCloseClick.</summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnCloseClick(ControlBoxEventArgs e)
        {
            CloseClick?.Invoke(e);
            ParentForm.Close();
        }

        /// <summary>The OnMaximizeClick.</summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnMaximizeClick(ControlBoxEventArgs e)
        {
            ParentForm.WindowState = FormWindowState.Maximized;
            MaximizeClick?.Invoke(e);
        }

        /// <summary>The OnMinimizeClick.</summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnMinimizeClick(ControlBoxEventArgs e)
        {
            ParentForm.WindowState = FormWindowState.Minimized;
            MinimizeClick?.Invoke(e);
        }

        /// <summary>Handling mouse down event of the control.</summary>
        /// <param name="e">The MouseEventArgs.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
        }

        /// <summary>Handling mouse leave event of the control.</summary>
        /// <param name="e">The EventArgs.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Cursor = Cursors.Default;
            _closeMouseState = MouseStates.Normal;
            _maximizeMouseState = MouseStates.Normal;
            _minimizeMouseState = MouseStates.Normal;
            Invalidate();
        }

        /// <summary>Handling mouse up event of the control so that we detect if cursor located in our need area.</summary>
        /// <param name="e">The MouseEventArgs.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_closeRectangle.Contains(e.Location))
            {
                Cursor = Cursors.Hand;
                _minimizeMouseState = MouseStates.Normal;
                _maximizeMouseState = MouseStates.Normal;
                _closeMouseState = MouseStates.Hover;
            }
            else
            {
                Cursor = Cursors.Default;
                _closeMouseState = MouseStates.Normal;
            }

            if (_maximizeVisible)
            {
                if (_maximizeRectangle.Contains(e.Location))
                {
                    if (_maximize)
                    {
                        Cursor = Cursors.Hand;
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                    }

                    _minimizeMouseState = MouseStates.Normal;
                    _maximizeMouseState = MouseStates.Hover;
                    _closeMouseState = MouseStates.Normal;
                }

                if (_minimizeVisible)
                {
                    if (_minimizeRectangle.Contains(e.Location))
                    {
                        if (_minimize)
                        {
                            Cursor = Cursors.Hand;
                        }
                        else
                        {
                            Cursor = Cursors.Default;
                        }

                        _minimizeMouseState = MouseStates.Hover;
                        _maximizeMouseState = MouseStates.Normal;
                        _closeMouseState = MouseStates.Normal;
                    }
                }
            }
            else
            {
                if (_minimizeVisible)
                {
                    if (_maximizeRectangle.Contains(e.Location))
                    {
                        if (_minimize)
                        {
                            Cursor = Cursors.Hand;
                        }
                        else
                        {
                            Cursor = Cursors.Default;
                        }

                        _minimizeMouseState = MouseStates.Hover;
                        _maximizeMouseState = MouseStates.Normal;
                        _closeMouseState = MouseStates.Normal;
                    }
                }
            }

            Invalidate();
        }

        /// <summary>Handling mouse up event of the control so that we can perform action commands.</summary>
        /// <param name="e">The MouseEventArgs.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_closeMouseState == MouseStates.Hover)
            {
                OnCloseClick(new ControlBoxEventArgs(Parent.FindForm()));
            }
            else if (_minimizeMouseState == MouseStates.Hover)
            {
                if (_minimize && _minimizeVisible)
                {
                    OnMinimizeClick(new ControlBoxEventArgs(Parent.FindForm()));
                }
            }
            else if (_maximizeMouseState == MouseStates.Hover)
            {
                if (_maximize && _maximizeVisible)
                {
                    if (ParentForm.WindowState == FormWindowState.Normal)
                    {
                        OnMaximizeClick(new ControlBoxEventArgs(Parent.FindForm()));
                    }
                    else
                    {
                        OnRestoredFormWindow(new ControlBoxEventArgs(Parent.FindForm()));
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics _graphics = e.Graphics;
            _graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            _closeRectangle = new Rectangle(70, 5, 27, Height);
            _maximizeRectangle = new Rectangle(38, 5, 24, Height);
            _minimizeRectangle = new Rectangle(5, 5, 27, Height);

            // _minimizeMovedRectangle = new Rectangle(38, 5, 24, Height);
            try
            {
                DrawCloseButton(_graphics, _closeRectangle);
                DrawMaximizeButton(_graphics, _maximizeRectangle);
                DrawMinimizeButton(_graphics, _minimizeRectangle, _maximizeRectangle);
            }
            catch (Exception exception)
            {
                // Throws unhandled exception: Doesn't allow 'Parent.FindForm()' on a 'UserControl' during first run.
                Console.WriteLine(exception);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Size = new Size(100, 25);
        }

        /// <summary>The OnRestoredFormWindow.</summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnRestoredFormWindow(ControlBoxEventArgs e)
        {
            ParentForm.WindowState = FormWindowState.Normal;
            RestoredFormWindow?.Invoke(e);
        }

        /// <summary>Draws the button.</summary>
        /// <param name="controlBoxButtons">The button type to draw.</param>
        /// <param name="graphics">The specified graphics to draw on.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="backColor">The back color.</param>
        /// <param name="foreColor">The fore color.</param>
        /// <param name="point">The point.</param>
        private void DrawButton(ControlBoxButtons controlBoxButtons, Graphics graphics, Rectangle rectangle, Color backColor, Color foreColor, Point point)
        {
            string _text;
            switch (controlBoxButtons)
            {
                case ControlBoxButtons.Close:
                    {
                        _text = "r";
                        break;
                    }

                case ControlBoxButtons.Maximize:
                    {
                        _text = ParentForm.WindowState == FormWindowState.Maximized ? "2" : "1";
                        break;
                    }

                case ControlBoxButtons.Minimize:
                    {
                        _text = "0";
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(controlBoxButtons), controlBoxButtons, null);
                    }
            }

            graphics.FillRectangle(new SolidBrush(backColor), rectangle);

            StringFormat _stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center
                };

            Font _font = new Font("Marlett", 12);
            graphics.DrawString(_text, _font, new SolidBrush(foreColor), point, _stringFormat);
        }

        /// <summary>Draws the close button.</summary>
        /// <param name="graphics">The specified graphics to draw on.</param>
        /// <param name="rectangle">The rectangle.</param>
        private void DrawCloseButton(Graphics graphics, Rectangle rectangle)
        {
            Color _backColor = ControlColorState.BackColorState(_closeBack, true, _closeMouseState);
            Color _foreColor = ControlColorState.BackColorState(_closeFore, true, _closeMouseState);

            DrawButton(ControlBoxButtons.Close, graphics, rectangle, _backColor, _foreColor, new Point(Width - 16, 8));
        }

        /// <summary>Draws the maximize button.</summary>
        /// <param name="graphics">The specified graphics to draw on.</param>
        /// <param name="rectangle">The rectangle.</param>
        private void DrawMaximizeButton(Graphics graphics, Rectangle rectangle)
        {
            if (_maximizeVisible)
            {
                Color _backColor = ControlColorState.BackColorState(_maximizeBack, _maximize, _maximizeMouseState);
                Color _foreColor = ControlColorState.BackColorState(_maximizeFore, _maximize, _maximizeMouseState);

                DrawButton(ControlBoxButtons.Maximize, graphics, rectangle, _backColor, _foreColor, new Point(51, 7));
            }
        }

        /// <summary>Draws the minimize button.</summary>
        /// <param name="graphics">The specified graphics to draw on.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="maximizeRectangle">The maximize rectangle area replacement.</param>
        private void DrawMinimizeButton(Graphics graphics, Rectangle rectangle, Rectangle maximizeRectangle)
        {
            if (_minimizeVisible)
            {
                Color _backColor = ControlColorState.BackColorState(_minimizeBack, _minimize, _minimizeMouseState);
                Color _foreColor = ControlColorState.BackColorState(_minimizeFore, _minimize, _minimizeMouseState);

                Point _point;
                Rectangle _rectangle;

                if (_maximizeVisible)
                {
                    _point = new Point(22, 7);
                    _rectangle = rectangle;
                }
                else
                {
                    _point = new Point(52, 7);
                    _rectangle = maximizeRectangle;
                }

                DrawButton(ControlBoxButtons.Minimize, graphics, _rectangle, _backColor, _foreColor, _point);
            }
        }

        /// <summary>Initializes the <see cref="VisualControlBox" />.</summary>
        private void InitializeControlBox()
        {
            DoubleBuffered = true;
            UpdateStyles();

            Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackColor = Color.Transparent;
            Size = new Size(100, 25);

            _minimizeVisible = true;
            _maximizeVisible = true;
            _maximize = true;
            _minimize = true;

            _closeMouseState = MouseStates.Normal;
            _maximizeMouseState = MouseStates.Normal;
            _minimizeMouseState = MouseStates.Normal;
        }

        #endregion
    }
}