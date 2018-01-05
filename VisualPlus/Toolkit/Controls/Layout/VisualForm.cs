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
    using VisualPlus.Designer;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Managers;
    using VisualPlus.Native;
    using VisualPlus.Properties;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.Controls.Interactivity;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Load")]
    [DefaultProperty("Text")]
    [Description("The Visual Form")]
    [Designer(typeof(VisualFormDesigner))]
    [DesignerCategory("Form")]
    [InitializationEvent("Load")]
    [ToolboxBitmap(typeof(VisualForm), "Resources.ToolboxBitmaps.VisualForm.bmp")]
    [ToolboxItem(false)]
    public class VisualForm : Form, IThemeSupport
    {
        #region Variables

        private readonly Cursor[] _resizeCursors;
        private readonly Dictionary<int, int> _resizedLocationsCommand;
        private Color _background;
        private Border _border;
        private bool _headerMouseDown;
        private bool _magnetic;
        private int _magneticRadius;
        private bool _maximized;
        private MouseStates _mouseState;
        private Size _previousSize;
        private ResizeDirection _resizeDir;
        private Rectangle _statusBarBounds;
        private StylesManager _styleManager;
        private Alignment.TextAlignment _titleAlignment;
        private Size _titleTextSize;
        private VisualBitmap _vsImage;
        private Color _windowBarColor;
        private int _windowBarHeight;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualForm" /> class.</summary>
        public VisualForm()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

            UpdateStyles();

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

            _styleManager = new StylesManager(Settings.DefaultValue.DefaultStyle);

            InitializeVisualForm();

            UpdateTheme(_styleManager.Theme);

            // This enables the form to trigger the MouseMove event even when mouse is over another control
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;
        }

        [Category(Localization.Category.Events.Appearance)]
        [Description(PropertyDescription.Color)]
        public event BackgroundChangedEventHandler BackgroundChanged;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description("Occours when the theme of the control has changed.")]
        public event ThemeChangedEventHandler ThemeChanged;

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
                if (_background == value)
                {
                    return;
                }

                _background = value;
                Invalidate();
                BackgroundChanged?.Invoke(new ColorEventArgs(_background));
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
        [Category(PropertyCategory.Appearance)]
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
        [Category(PropertyCategory.Behavior)]
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
        [Category(PropertyCategory.Behavior)]
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

        [Category(PropertyCategory.WindowStyle)]
        [Description(PropertyDescription.ShowIcon)]
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

        /// <summary>Gets or sets the <see cref="StylesManager" />.</summary>
        [Browsable(false)]
        [Category(PropertyCategory.Appearance)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StylesManager StyleManager
        {
            get
            {
                return _styleManager;
            }

            set
            {
                _styleManager = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Alignment)]
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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
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

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Size)]
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

        public void UpdateTheme(Theme theme)
        {
            try
            {
                _styleManager = new StylesManager(theme);

                _background = theme.OtherSettings.FormBackground;
                _border.Color = theme.BorderSettings.Normal;
                _border.HoverColor = theme.BorderSettings.Hover;
                ForeColor = theme.TextSetting.Enabled;
                Font = theme.TextSetting.Font;
                _windowBarColor = theme.OtherSettings.FormWindowBar;
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }

            OnThemeChanged(new ThemeEventArgs(theme));
        }

        /// <summary>Raises the <see cref="OnControlAdded" /> event.</summary>
        /// <param name="e">The event args.</param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            VisualControlBase _control = (VisualControlBase)e.Control;

            if (_control != null)
            {
                // Successfully cast.
                if (_control is VisualControlBox)
                {
                    _control.BackColor = _windowBarColor;
                }
                else
                {
                    _control.BackColor = _background;
                }
            }
            else
            {
                // Cast failed - Unsupported control.
            }
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

            if ((e.Button == MouseButtons.Left) && !_maximized)
            {
                ResizeForm(_resizeDir);
            }

            base.OnMouseDown(e);
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
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

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

        /// <summary>Invokes the theme changed event.</summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnThemeChanged(ThemeEventArgs e)
        {
            Invalidate();
            ThemeChanged?.Invoke(e);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (DesignMode || IsDisposed)
            {
                return;
            }

            if ((m.Msg == WM_MOUSEMOVE) && _maximized && _statusBarBounds.Contains(PointToClient(Cursor.Position)))
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
            else if ((m.Msg == WM_LBUTTONDOWN) && _statusBarBounds.Contains(PointToClient(Cursor.Position)))
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

                if (_statusBarBounds.Contains(cursorPos))
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

        private void DrawImageIcon(Graphics graphics)
        {
            VisualBitmap.DrawImage(graphics, _vsImage.Border, _vsImage.Point, _vsImage.Image, _vsImage.Size, _vsImage.Visible);
        }

        private void DrawTitle(Graphics graphics)
        {
            _titleTextSize = GraphicsManager.MeasureText(graphics, Text, Font);
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
                        titlePoint = new Point(Width - _border.Thickness - _titleTextSize.Width, (_windowBarHeight / 2) - (_titleTextSize.Height / 2));
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

        /// <summary>Initializes the <see cref="VisualForm" />.</summary>
        private void InitializeVisualForm()
        {
            _border = new Border
                {
                    Thickness = 3,
                    Type = ShapeType.Rectangle
                };

            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            _magnetic = true;
            _magneticRadius = 100;
            Padding = new Padding(0, 0, 0, 0);
            Sizable = true;
            _titleAlignment = Alignment.TextAlignment.Center;
            TransparencyKey = Color.Fuchsia;
            _windowBarHeight = 30;

            _vsImage = new VisualBitmap(Resources.VisualPlus, new Size(16, 16)) { Visible = true };
            _vsImage.Point = new Point(5, (_windowBarHeight / 2) - (_vsImage.Size.Height / 2));
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