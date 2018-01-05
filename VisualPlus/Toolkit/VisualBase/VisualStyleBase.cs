namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Delegates;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class VisualStyleBase : VisualControlBase, IThemeManager
    {
        #region Variables

        private MouseStates _mouseState;
        private TextStyle _textStyle;
        private StylesManager _themeManager;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualStyleBase" /> class.</summary>
        public VisualStyleBase()
        {
            // Allow transparent BackColor.
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            // Double buffering to reduce drawing flicker.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            // Repaint entire control whenever resizing.
            SetStyle(ControlStyles.ResizeRedraw, true);

            UpdateStyles();
            Initialize();
        }

        [Category(Localization.Category.Events.Mouse)]
        [Description("Occours when the MouseState of the control has changed.")]
        public event MouseStateChangedEventHandler MouseStateChanged;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description("Occours when the theme of the control has changed.")]
        public event ThemeChangedEventHandler ThemeChanged;

        #endregion

        #region Properties

        /// <summary>Gets or sets the <see cref="MouseState" />.</summary>
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.MouseState)]
        public MouseStates MouseState
        {
            get
            {
                return _mouseState;
            }

            set
            {
                if (value == _mouseState)
                {
                    return;
                }

                _mouseState = value;
                OnMouseStateChanged(new MouseStateEventArgs(_mouseState));
            }
        }

        /// <summary>Gets or sets the <see cref="TextStyle" />.</summary>
        [Browsable(false)]
        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.TextStyle)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TextStyle TextStyle
        {
            get
            {
                return _textStyle;
            }

            set
            {
                _textStyle = value;
            }
        }

        /// <summary>Gets or sets the <see cref="StylesManager" />.</summary>
        [Browsable(false)]
        [Category(PropertyCategory.Appearance)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StylesManager ThemeManager
        {
            get
            {
                return _themeManager;
            }

            set
            {
                if ((_themeManager == null) || (value == _themeManager))
                {
                    return;
                }

                _themeManager = value;
            }
        }

        /// <summary>Gets or sets the <see cref="GraphicsPath" />.</summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal GraphicsPath ControlGraphicsPath { get; set; }

        #endregion

        #region Events

        /// <summary>Invokes the mouse state changed event.</summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnMouseStateChanged(MouseStateEventArgs e)
        {
            Invalidate();
            MouseStateChanged?.Invoke(e);
        }

        /// <summary>Invokes the theme changed event.</summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnThemeChanged(ThemeEventArgs e)
        {
            ThemeChanged?.Invoke(e);
            Invalidate();
        }

        /// <summary>Initialize the base.</summary>
        private void Initialize()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

            _mouseState = MouseStates.Normal;
            _themeManager = new StylesManager(Settings.DefaultValue.DefaultStyle);
            _textStyle = new TextStyle();
        }

        #endregion
    }
}