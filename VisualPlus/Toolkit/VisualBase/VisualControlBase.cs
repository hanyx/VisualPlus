namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
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
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    using Property = VisualPlus.Localization.Descriptions.Property;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class VisualControlBase : Control
    {
        #region Variables

        private Color _backgroundColor;
        private Color _backgroundDisabledColor;
        private Color _foreColorDisabled;
        private MouseStates _mouseState;
        private VisualStyleManager _styleManager;
        private TextRenderingHint _textRenderingHint;

        #endregion

        #region Constructors

        protected VisualControlBase()
        {
            // Allow transparent BackColor.
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            // Double buffering to reduce drawing flicker.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            // Repaint entire control whenever resizing.
            SetStyle(ControlStyles.ResizeRedraw, true);

            // Drawn double buffered by default.
            DoubleBuffered = true;
            ResizeRedraw = true;

            _mouseState = MouseStates.Normal;

            _textRenderingHint = Settings.DefaultValue.TextRenderingHint;

            _styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);
        }

        [Category(Event.Appearance)]
        [Description(Property.Description.Common.Color)]
        public event BackgroundChangedEventHandler BackgroundChanged;

        [Category(Event.Appearance)]
        [Description(Property.Description.Common.Color)]
        public event BackgroundChangedEventHandler BackgroundDisabledChanged;

        [Category(Event.PropertyChanged)]
        [Description("Occours when the ForeColorDisabled property for the control has changed.")]
        public event ForeColorDisabledChangedEventHandler ForeColorDisabledChanged;

        [Category(Event.Mouse)]
        [Description("Occours when the MouseState of the control has changed.")]
        public event MouseStateChangedEventHandler MouseStateChanged;

        [Category(Event.PropertyChanged)]
        [Description("Occours when the TextRenderingHint property has changed.")]
        public event TextRenderingChangedEventHandler TextRenderingHintChanged;

        [Category(Event.PropertyChanged)]
        [Description("Occours when the theme changed for the control.")]
        public event ThemeChangedEventHandler ThemeChanged;

        #endregion

        #region Properties

        [Category(Localization.Category.Property.Layout)]
        [Description(Property.Description.Common.AutoSize)]
        public new bool AutoSize { get; set; }

        [Category(Localization.Category.Property.Appearance)]
        [Description(Property.Description.Common.Color)]
        public Color Background
        {
            get
            {
                return _backgroundColor;
            }

            set
            {
                _backgroundColor = value;
                OnBackgroundChanged(new ColorEventArgs(_backgroundColor));
                Invalidate();
            }
        }

        [Category(Localization.Category.Property.Appearance)]
        [Description(Property.Description.Common.Color)]
        public Color BackgroundDisabled
        {
            get
            {
                return _backgroundDisabledColor;
            }

            set
            {
                _backgroundDisabledColor = value;
                OnBackgroundDisabledChanged(new ColorEventArgs(_backgroundDisabledColor));
                Invalidate();
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Color BackgroundStateColor { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LinearGradientBrush BackgroundStateGradientBrush { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Gradient[] ControlBrushCollection { get; set; }

        public override Color ForeColor { get; set; }

        [Category(Localization.Category.Property.Appearance)]
        [Description(Property.Description.Common.Color)]
        public Color ForeColorDisabled
        {
            get
            {
                return _foreColorDisabled;
            }

            set
            {
                _foreColorDisabled = value;
                OnForeColorDisabledChanged(new ColorEventArgs(_foreColorDisabled));
                Invalidate();
            }
        }

        [Category(Localization.Category.Property.Appearance)]
        [Description(Property.Description.Common.MouseState)]
        public MouseStates MouseState
        {
            get
            {
                return _mouseState;
            }

            set
            {
                _mouseState = value;
                OnMouseStateChanged(new MouseStateEventArgs(_mouseState));
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VisualStyleManager StyleManager
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

        [Category(Localization.Category.Property.Appearance)]
        [Description(Property.Description.Strings.TextRenderingHint)]
        public TextRenderingHint TextRenderingHint
        {
            get
            {
                return _textRenderingHint;
            }

            set
            {
                _textRenderingHint = value;
                OnTextRenderingHintChanged(new TextRenderingEventArgs(_textRenderingHint));
                Invalidate();
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal GraphicsPath ControlGraphicsPath { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Point LastPosition { get; set; }

        #endregion

        #region Events

        /// <summary>Retrieves the background state color.</summary>
        /// <param name="control">The control.</param>
        /// <returns>Background color.</returns>
        public Color GetBackgroundState(VisualControlBase control)
        {
            return control.Enabled ? control.Background : control.BackgroundDisabled;
        }

        /// <summary>Retrieves the fore state color.</summary>
        /// <param name="control">The control.</param>
        /// <returns>The fore color.</returns>
        public Color GetForeColorState(VisualControlBase control)
        {
            return control.Enabled ? control.Background : control.BackgroundDisabled;
        }

        protected virtual void OnBackgroundChanged(ColorEventArgs e)
        {
            BackgroundChanged?.Invoke(e);
        }

        protected virtual void OnBackgroundDisabledChanged(ColorEventArgs e)
        {
            BackgroundDisabledChanged?.Invoke(e);
        }

        protected virtual void OnForeColorDisabledChanged(ColorEventArgs e)
        {
            ForeColorDisabledChanged?.Invoke(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseState = MouseStates.Normal;
            Invalidate();
        }

        protected virtual void OnMouseStateChanged(MouseStateEventArgs e)
        {
            MouseStateChanged?.Invoke(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.TextRenderingHint = _textRenderingHint;
        }

        protected virtual void OnTextRenderingHintChanged(TextRenderingEventArgs e)
        {
            TextRenderingHintChanged?.Invoke(e);
        }

        #endregion
    }
}