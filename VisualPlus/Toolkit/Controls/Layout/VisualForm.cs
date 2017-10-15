namespace VisualPlus.Toolkit.Controls.Layout
{
    #region Namespace

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Delegates;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Managers;
    using VisualPlus.PInvoke;
    using VisualPlus.Properties;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    #endregion

    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(Form))]
    [Description("The Visual Form")]
    [Designer(ControlManager.FilterProperties.VisualForm)]
    public class VisualForm : Form
    {
        #region Variables

        private readonly Cursor[] _resizeCursors;
        private readonly Dictionary<int, int> _resizedLocationsCommand;
        private Color _background;
        private Border _border;
        private Color _buttonBackHoverColor;
        private Color _buttonBackPressedColor;
        private Size _buttonSize;
        private ButtonState _buttonState;
        private Color _closeColor;
        private bool _headerMouseDown;
        private bool _magnetic;
        private int _magneticRadius;
        private Rectangle _maxButtonBounds;
        private Color _maxColor;
        private bool _maximized;
        private Rectangle _minButtonBounds;
        private Color _minColor;
        private MouseStates _mouseState;
        private Point _previousLocation;
        private Size _previousSize;
        private ResizeDirection _resizeDir;
        private Rectangle _statusBarBounds;
        private VisualStyleManager _styleManager;
        private Alignment.TextAlignment _titleAlignment;
        private Size _titleTextSize;
        private VisualBitmap _vsImage;
        private Color _windowBarColor;
        private int _windowBarHeight;
        private Rectangle _xButtonBounds;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.Controls.Layout.VisualForm" /> class.</summary>
        public VisualForm()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            _styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);
            _resizeCursors = new[] { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };

            _resizedLocationsCommand = new Dictionary<int, int>
                {
                    { HTTOP, WMSZ_TOP },
                    { HTTOPLEFT, WMSZ_TOPLEFT },
                    { HTTOPRIGHT, WMSZ_TOPRIGHT },
                    { HTLEFT, WMSZ_LEFT },
                    { HTRIGHT, WMSZ_RIGHT },
                    { HTBOTTOM, WMSZ_BOTTOM },
                    { HTBOTTOMLEFT, WMSZ_BOTTOMLEFT },
                    { HTBOTTOMRIGHT, WMSZ_BOTTOMRIGHT }
                };

            _titleAlignment = Alignment.TextAlignment.Center;
            FormBorderStyle = FormBorderStyle.None;
            Sizable = true;
            _closeColor = Color.IndianRed;
            _buttonBackHoverColor = _styleManager.ControlColorStateStyle.ControlHover;
            _buttonBackPressedColor = _styleManager.ControlColorStateStyle.ControlPressed;
            _buttonState = ButtonState.None;
            _maxColor = _styleManager.ControlStyle.FlatButtonEnabled;
            _minColor = _styleManager.ControlStyle.FlatButtonEnabled;
            _buttonSize = new Size(25, 25);
            _windowBarColor = _styleManager.ControlStyle.Background(0);
            _background = _styleManager.ControlStyle.Background(3);
            _magneticRadius = 100;
            _magnetic = true;
            _windowBarHeight = 30;
            TransparencyKey = Color.Fuchsia;
            DoubleBuffered = true;

            Padding = new Padding(0, 0, 0, 0);

            _border = new Border
                {
                    Thickness = 3,
                    Type = ShapeType.Rectangle
                };

            _vsImage = new VisualBitmap(Resources.VisualPlus, new Size(16, 16)) { Visible = true };
            _vsImage.Point = new Point(5, (_windowBarHeight / 2) - (_vsImage.Size.Height / 2));

            // This enables the form to trigger the MouseMove event even when mouse is over another control
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;
        }

        [Category(Localization.Category.Events.Appearance)]
        [Description(Property.Color)]
        public event BackgroundChangedEventHandler BackgroundChanged;

        public enum ButtonState
        {
            /// <summary>The x over.</summary>
            XOver,

            /// <summary>The max over.</summary>
            MaxOver,

            /// <summary>The min over.</summary>
            MinOver,

            /// <summary>The x down.</summary>
            XDown,

            /// <summary>The max down.</summary>
            MaxDown,

            /// <summary>The min down.</summary>
            MinDown,

            /// <summary>The None.</summary>
            None
        }

        public enum ControlBoxAlignment
        {
            /// <summary>The bottom.</summary>
            Bottom,

            /// <summary>The center.</summary>
            Center,

            /// <summary>The top.</summary>
            Top
        }

        public enum ResizeDirection
        {
            /// <summary>The bottom left.</summary>
            BottomLeft,

            /// <summary>The left.</summary>
            Left,

            /// <summary>The right.</summary>
            Right,

            /// <summary>The bottom right.</summary>
            BottomRight,

            /// <summary>The bottom.</summary>
            Bottom,

            /// <summary>The none.</summary>
            None
        }

        #endregion

        #region Properties

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color Background
        {
            get
            {
                return _background;
            }

            set
            {
                _background = value;
                OnBackgroundChanged(new ColorEventArgs(_background));
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

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ButtonBackHoverColor
        {
            get
            {
                return _buttonBackHoverColor;
            }

            set
            {
                _buttonBackHoverColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ButtonBackPressedColor
        {
            get
            {
                return _buttonBackPressedColor;
            }

            set
            {
                _buttonBackPressedColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ButtonCloseColor
        {
            get
            {
                return _closeColor;
            }

            set
            {
                _closeColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ButtonMaximizeColor
        {
            get
            {
                return _maxColor;
            }

            set
            {
                _maxColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ButtonMinimizeColor
        {
            get
            {
                return _minColor;
            }

            set
            {
                _minColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public Size ButtonSize
        {
            get
            {
                return _buttonSize;
            }

            set
            {
                _buttonSize = value;
                Invalidate();
            }
        }

        [Browsable(true)]
        public new Icon Icon
        {
            get
            {
                return base.Icon;
            }

            set
            {
                base.Icon = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(VisualBitmapConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public VisualBitmap Image
        {
            get
            {
                return _vsImage;
            }

            set
            {
                _vsImage = value;
                Invalidate();
            }
        }

        [DefaultValue(true)]
        [Category(Propertys.Behavior)]
        [Description("Snap window snaps toggles snapping to screen edges.")]
        public bool Magnetic
        {
            get
            {
                return _magnetic;
            }

            set
            {
                _magnetic = value;
            }
        }

        [DefaultValue(100)]
        [Category(Propertys.Behavior)]
        [Description("The snap radius determines the distance to trigger the snap.")]
        public int MagneticRadius
        {
            get
            {
                return _magneticRadius;
            }

            set
            {
                _magneticRadius = value;
            }
        }

        [Category(Propertys.WindowStyle)]
        [Description(Property.ShowIcon)]
        public new bool ShowIcon
        {
            get
            {
                return _vsImage.Visible;
            }

            set
            {
                _vsImage.Visible = value;
            }
        }

        public bool Sizable { get; set; }

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
        public Alignment.TextAlignment TitleAlignment
        {
            get
            {
                return _titleAlignment;
            }

            set
            {
                _titleAlignment = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color WindowBarColor
        {
            get
            {
                return _windowBarColor;
            }

            set
            {
                _windowBarColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public int WindowBarHeight
        {
            get
            {
                return _windowBarHeight;
            }

            set
            {
                _windowBarHeight = value;
                Invalidate();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams par = base.CreateParams;

                // WS_SYSMENU: Trigger the creation of the system menu
                // WS_MINIMIZEBOX: Allow minimizing from taskbar
                par.Style = par.Style | WS_MINIMIZEBOX | WS_SYSMENU; // Turn on the WS_MINIMIZEBOX style flag
                return par;
            }
        }

        #endregion

        #region Events

        protected virtual void OnBackgroundChanged(ColorEventArgs e)
        {
            GDI.ApplyContainerBackColorChange(this, Background);
            BackgroundChanged?.Invoke(e);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            GDI.SetControlBackColor(e.Control, Background, false);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            GDI.SetControlBackColor(e.Control, Background, true);
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            State = MouseStates.Hover;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            State = MouseStates.Hover;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            State = MouseStates.Normal;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            State = MouseStates.Normal;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            UpdateButtons(e);

            if ((e.Button == MouseButtons.Left) && !_maximized)
            {
                ResizeForm(_resizeDir);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (DesignMode)
            {
                return;
            }

            if (_buttonState != ButtonState.None)
            {
                _buttonState = ButtonState.None;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (DesignMode)
            {
                return;
            }

            if (Sizable)
            {
                // True if the mouse is hovering over a child control
                bool isChildUnderMouse = GetChildAtPoint(e.Location) != null;

                if ((e.Location.X < _border.Thickness) && (e.Location.Y > Height - _border.Thickness) && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.BottomLeft;
                    Cursor = Cursors.SizeNESW;
                }
                else if ((e.Location.X < _border.Thickness) && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.Left;
                    Cursor = Cursors.SizeWE;
                }
                else if ((e.Location.X > Width - _border.Thickness) && (e.Location.Y > Height - _border.Thickness) && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.BottomRight;
                    Cursor = Cursors.SizeNWSE;
                }
                else if ((e.Location.X > Width - _border.Thickness) && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.Right;
                    Cursor = Cursors.SizeWE;
                }
                else if ((e.Location.Y > Height - _border.Thickness) && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.Bottom;
                    Cursor = Cursors.SizeNS;
                }
                else
                {
                    _resizeDir = ResizeDirection.None;

                    // Only reset the cursor when needed, this prevents it from flickering when a child control changes the cursor to its own needs
                    if (((IList)_resizeCursors).Contains(Cursor))
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

            UpdateButtons(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            UpdateButtons(e, true);

            base.OnMouseUp(e);
            User32.ReleaseCapture();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.Clear(BackColor);
            graphics.SmoothingMode = SmoothingMode.Default;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            Rectangle _clientRectangle;

            switch (_border.Type)
            {
                case ShapeType.Rectangle:
                    {
                        _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width + 1, ClientRectangle.Height + 1);
                        break;
                    }

                case ShapeType.Rounded:
                    {
                        _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            GraphicsPath _clientPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);

            graphics.SetClip(_clientPath);
            graphics.FillPath(new SolidBrush(_background), _clientPath);

            // Title box
            graphics.FillRectangle(new SolidBrush(_windowBarColor), _statusBarBounds);

            DrawButtons(graphics);
            DrawImageIcon(graphics);

            graphics.SetClip(_clientPath);

            DrawTitle(graphics);

            graphics.ResetClip();

            VisualBorderRenderer.DrawBorderStyle(graphics, _border, _clientPath, State);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _vsImage.Point = new Point(_vsImage.Point.X, (_windowBarHeight / 2) - (_vsImage.Size.Height / 2));
            _minButtonBounds = new Rectangle(Width - (3 * _buttonSize.Width), (_windowBarHeight / 2) - (_buttonSize.Height / 2), _buttonSize.Width, _buttonSize.Height);
            _maxButtonBounds = new Rectangle(Width - (2 * _buttonSize.Width), (_windowBarHeight / 2) - (_buttonSize.Height / 2), _buttonSize.Width, _buttonSize.Height);
            _xButtonBounds = new Rectangle(Width - _buttonSize.Width, (_windowBarHeight / 2) - (_buttonSize.Height / 2), _buttonSize.Width, _buttonSize.Height);
            _statusBarBounds = new Rectangle(0, 0, Width, _windowBarHeight);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);

            if (_magnetic)
            {
                Screen _screen = Screen.FromPoint(Location);
                if (DoSnap(Left, _screen.WorkingArea.Left))
                {
                    Left = _screen.WorkingArea.Left;
                }

                if (DoSnap(Top, _screen.WorkingArea.Top))
                {
                    Top = _screen.WorkingArea.Top;
                }

                if (DoSnap(_screen.WorkingArea.Right, Right))
                {
                    Left = _screen.WorkingArea.Right - Width;
                }

                if (DoSnap(_screen.WorkingArea.Bottom, Bottom))
                {
                    Top = _screen.WorkingArea.Bottom - Height;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (DesignMode || IsDisposed)
            {
                return;
            }

            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                MaximizeWindow(!_maximized);
            }
            else if ((m.Msg == WM_MOUSEMOVE) && _maximized && _statusBarBounds.Contains(PointToClient(Cursor.Position)) && !(_minButtonBounds.Contains(PointToClient(Cursor.Position)) || _maxButtonBounds.Contains(PointToClient(Cursor.Position)) || _xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (_headerMouseDown)
                {
                    _maximized = false;
                    _headerMouseDown = false;

                    Point mousePoint = PointToClient(Cursor.Position);
                    if (mousePoint.X < Width / 2)
                    {
                        Location = mousePoint.X < _previousSize.Width / 2 ? new Point(Cursor.Position.X - mousePoint.X, Cursor.Position.Y - mousePoint.Y) : new Point(Cursor.Position.X - (_previousSize.Width / 2), Cursor.Position.Y - mousePoint.Y);
                    }
                    else
                    {
                        Location = Width - mousePoint.X < _previousSize.Width / 2 ? new Point(((Cursor.Position.X - _previousSize.Width) + Width) - mousePoint.X, Cursor.Position.Y - mousePoint.Y) : new Point(Cursor.Position.X - (_previousSize.Width / 2), Cursor.Position.Y - mousePoint.Y);
                    }

                    Size = _previousSize;
                    User32.ReleaseCapture();
                    User32.SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            else if ((m.Msg == WM_LBUTTONDOWN) && _statusBarBounds.Contains(PointToClient(Cursor.Position)) && !(_minButtonBounds.Contains(PointToClient(Cursor.Position)) || _maxButtonBounds.Contains(PointToClient(Cursor.Position)) || _xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (!_maximized)
                {
                    User32.ReleaseCapture();
                    User32.SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
                else
                {
                    _headerMouseDown = true;
                }
            }
            else if (m.Msg == WM_RBUTTONDOWN)
            {
                Point cursorPos = PointToClient(Cursor.Position);

                if (_statusBarBounds.Contains(cursorPos) && !_minButtonBounds.Contains(cursorPos) &&
                    !_maxButtonBounds.Contains(cursorPos) && !_xButtonBounds.Contains(cursorPos))
                {
                    // Show default system menu when right clicking titlebar
                    int id = User32.TrackPopupMenuEx(User32.GetSystemMenu(Handle, false), TPM_LEFTALIGN | TPM_RETURNCMD, Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);

                    // Pass the command as a WM_SYSCOMMAND message
                    User32.SendMessage(Handle, WM_SYSCOMMAND, id, 0);
                }
            }
            else if (m.Msg == WM_NCLBUTTONDOWN)
            {
                // This re-enables resizing by letting the application know when the
                // user is trying to resize a side. This is disabled by default when using WS_SYSMENU.
                if (!Sizable)
                {
                    return;
                }

                byte bFlag = 0;

                // Get which side to resize from
                if (_resizedLocationsCommand.ContainsKey((int)m.WParam))
                {
                    bFlag = (byte)_resizedLocationsCommand[(int)m.WParam];
                }

                if (bFlag != 0)
                {
                    User32.SendMessage(Handle, WM_SYSCOMMAND, 0xF000 | bFlag, (int)m.LParam);
                }
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                _headerMouseDown = false;
            }
        }

        private const int HT_CAPTION = 0x2;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int MONITOR_DEFAULTTONEAREST = 2;
        private const uint TPM_LEFTALIGN = 0x0000;
        private const uint TPM_RETURNCMD = 0x0100;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int WMSZ_BOTTOM = 6;
        private const int WMSZ_BOTTOMLEFT = 7;
        private const int WMSZ_BOTTOMRIGHT = 8;
        private const int WMSZ_LEFT = 1;
        private const int WMSZ_RIGHT = 2;
        private const int WMSZ_TOP = 3;
        private const int WMSZ_TOPLEFT = 4;
        private const int WMSZ_TOPRIGHT = 5;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x00080000;

        /// <summary>Snap the position to edge.</summary>
        /// <param name="position">The position.</param>
        /// <param name="edge">The edge.</param>
        /// <returns>Does a snap.</returns>
        private bool DoSnap(int position, int edge)
        {
            return (position - edge > 0) && (position - edge <= _magneticRadius);
        }

        private void DrawButtons(Graphics graphics)
        {
            // Determine whether or not we even should be drawing the buttons.
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;
            SolidBrush hoverBrush = new SolidBrush(_buttonBackHoverColor);
            SolidBrush downBrush = new SolidBrush(_buttonBackPressedColor);

            // When MaximizeButton == false, the minimize button will be painted in its place
            DrawMinimizeOverMaximizeButton(graphics, showMin, showMax, hoverBrush, downBrush);

            var penWidth = 2;
            Pen minPen = new Pen(_minColor, penWidth);
            Pen maxPen = new Pen(_maxColor, penWidth);
            Pen closePen = new Pen(_closeColor, penWidth);

            // Minimize button.
            if (showMin)
            {
                int x = showMax ? _minButtonBounds.X : _maxButtonBounds.X;
                int y = showMax ? _minButtonBounds.Y : _maxButtonBounds.Y;

                graphics.DrawLine(
                    minPen,
                    x + (int)(_minButtonBounds.Width * 0.33),
                    y + (int)(_minButtonBounds.Height * 0.66),
                    x + (int)(_minButtonBounds.Width * 0.66),
                    y + (int)(_minButtonBounds.Height * 0.66));
            }

            // Maximize button
            if (showMax)
            {
                graphics.DrawRectangle(
                    maxPen,
                    _maxButtonBounds.X + (int)(_maxButtonBounds.Width * 0.33),
                    _maxButtonBounds.Y + (int)(_maxButtonBounds.Height * 0.36),
                    (int)(_maxButtonBounds.Width * 0.39),
                    (int)(_maxButtonBounds.Height * 0.35));

                graphics.DrawRectangle(
                    maxPen,
                    _maxButtonBounds.X + (int)(_maxButtonBounds.Width * 0.33),
                    _maxButtonBounds.Y + (int)(_maxButtonBounds.Height * 0.36),
                    (int)(_maxButtonBounds.Width * 0.39),
                    (int)(_maxButtonBounds.Height * 0.08));
            }

            // Close button
            if (ControlBox)
            {
                graphics.DrawLine(
                    closePen,
                    _xButtonBounds.X + (int)(_xButtonBounds.Width * 0.33),
                    _xButtonBounds.Y + (int)(_xButtonBounds.Height * 0.33),
                    _xButtonBounds.X + (int)(_xButtonBounds.Width * 0.66),
                    _xButtonBounds.Y + (int)(_xButtonBounds.Height * 0.66));

                graphics.DrawLine(
                    closePen,
                    _xButtonBounds.X + (int)(_xButtonBounds.Width * 0.66),
                    _xButtonBounds.Y + (int)(_xButtonBounds.Height * 0.33),
                    _xButtonBounds.X + (int)(_xButtonBounds.Width * 0.33),
                    _xButtonBounds.Y + (int)(_xButtonBounds.Height * 0.66));
            }
        }

        private void DrawImageIcon(Graphics graphics)
        {
            VisualBitmap.DrawImage(graphics, _vsImage.Border, _vsImage.Point, _vsImage.Image, _vsImage.Size, _vsImage.Visible);
        }

        private void DrawMinimizeOverMaximizeButton(Graphics graphics, bool showMin, bool showMax, SolidBrush hoverBrush, SolidBrush downBrush)
        {
            if ((_buttonState == ButtonState.MinOver) && showMin)
            {
                graphics.FillRectangle(hoverBrush, showMax ? _minButtonBounds : _maxButtonBounds);
            }

            if ((_buttonState == ButtonState.MinDown) && showMin)
            {
                graphics.FillRectangle(downBrush, showMax ? _minButtonBounds : _maxButtonBounds);
            }

            if ((_buttonState == ButtonState.MaxOver) && showMax)
            {
                graphics.FillRectangle(hoverBrush, _maxButtonBounds);
            }

            if ((_buttonState == ButtonState.MaxDown) && showMax)
            {
                graphics.FillRectangle(downBrush, _maxButtonBounds);
            }

            if ((_buttonState == ButtonState.XOver) && ControlBox)
            {
                graphics.FillRectangle(hoverBrush, _xButtonBounds);
            }

            if ((_buttonState == ButtonState.XDown) && ControlBox)
            {
                graphics.FillRectangle(downBrush, _xButtonBounds);
            }
        }

        private void DrawTitle(Graphics graphics)
        {
            _titleTextSize = GDI.MeasureText(graphics, Text, Font);
            Point titlePoint;

            switch (_titleAlignment)
            {
                case Alignment.TextAlignment.Center:
                    {
                        titlePoint = new Point((Width / 2) - (_titleTextSize.Width / 2), (_windowBarHeight / 2) - (_titleTextSize.Height / 2));
                        break;
                    }

                case Alignment.TextAlignment.Left:
                    {
                        titlePoint = new Point(_vsImage.Point.X + _vsImage.Size.Width, (_windowBarHeight / 2) - (_titleTextSize.Height / 2));
                        break;
                    }

                case Alignment.TextAlignment.Right:
                    {
                        titlePoint = new Point(_minButtonBounds.X - _titleTextSize.Width, (_windowBarHeight / 2) - (_titleTextSize.Height / 2));
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            Rectangle textRectangle = new Rectangle(titlePoint.X, titlePoint.Y, Width, _titleTextSize.Height);
            graphics.DrawString(Text, Font, new SolidBrush(ForeColor), textRectangle);
        }

        private void MaximizeWindow(bool maximize)
        {
            if (!MaximizeBox || !ControlBox)
            {
                return;
            }

            _maximized = maximize;

            if (maximize)
            {
                IntPtr _monitorHandle = User32.MonitorFromWindow(Handle, MONITOR_DEFAULTTONEAREST);
                MonitorManager _monitorInfo = new MonitorManager();
                User32.GetMonitorInfo(new HandleRef(null, _monitorHandle), _monitorInfo);
                _previousSize = Size;
                _previousLocation = Location;
                Size = new Size(_monitorInfo.WorkingArea.Width, _monitorInfo.WorkingArea.Height);
                Location = new Point(_monitorInfo.WorkingArea.Left, _monitorInfo.WorkingArea.Top);
            }
            else
            {
                Size = _previousSize;
                Location = _previousLocation;
            }
        }

        private void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            // Convert to client position and pass to Form.MouseMove
            Point clientCursorPos = PointToClient(e.Location);
            MouseEventArgs newE = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
            OnMouseMove(newE);
        }

        private void ResizeForm(ResizeDirection direction)
        {
            if (DesignMode)
            {
                return;
            }

            int _dir = -1;
            switch (direction)
            {
                case ResizeDirection.BottomLeft:
                    {
                        _dir = HTBOTTOMLEFT;
                        break;
                    }

                case ResizeDirection.Left:
                    {
                        _dir = HTLEFT;
                        break;
                    }

                case ResizeDirection.Right:
                    {
                        _dir = HTRIGHT;
                        break;
                    }

                case ResizeDirection.BottomRight:
                    {
                        _dir = HTBOTTOMRIGHT;
                        break;
                    }

                case ResizeDirection.Bottom:
                    {
                        _dir = HTBOTTOM;
                        break;
                    }

                case ResizeDirection.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            User32.ReleaseCapture();
            if (_dir != -1)
            {
                User32.SendMessage(Handle, WM_NCLBUTTONDOWN, _dir, 0);
            }
        }

        private void UpdateButtons(MouseEventArgs e, bool up = false)
        {
            if (DesignMode)
            {
                return;
            }

            ButtonState oldState = _buttonState;
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;

            if ((e.Button == MouseButtons.Left) && !up)
            {
                if (showMin && !showMax && _maxButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MinDown;
                }
                else if (showMin && showMax && _minButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MinDown;
                }
                else if (showMax && _maxButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MaxDown;
                }
                else if (ControlBox && _xButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.XDown;
                }
                else
                {
                    _buttonState = ButtonState.None;
                }
            }
            else
            {
                if (showMin && !showMax && _maxButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MinOver;

                    if ((oldState == ButtonState.MinDown) && up)
                    {
                        WindowState = FormWindowState.Minimized;
                    }
                }
                else if (showMin && showMax && _minButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MinOver;

                    if ((oldState == ButtonState.MinDown) && up)
                    {
                        WindowState = FormWindowState.Minimized;
                    }
                }
                else if (MaximizeBox && ControlBox && _maxButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MaxOver;

                    if ((oldState == ButtonState.MaxDown) && up)
                    {
                        MaximizeWindow(!_maximized);
                    }
                }
                else if (ControlBox && _xButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.XOver;

                    if ((oldState == ButtonState.XDown) && up)
                    {
                        Close();
                    }
                }
                else
                {
                    _buttonState = ButtonState.None;
                }
            }

            if (oldState != _buttonState)
            {
                Invalidate(_maxButtonBounds);
                Invalidate(_minButtonBounds);
                Invalidate(_xButtonBounds);
            }
        }

        #endregion

        #region Methods

        private class MouseMessageFilter : IMessageFilter
        {
            #region Constructors

            public static event MouseEventHandler MouseMove;

            #endregion

            #region Events

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_MOUSEMOVE)
                {
                    if (MouseMove != null)
                    {
                        int x = MousePosition.X, y = MousePosition.Y;

                        MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                    }
                }

                return false;
            }

            private const int WM_MOUSEMOVE = 0x0200;

            #endregion
        }

        #endregion
    }
}