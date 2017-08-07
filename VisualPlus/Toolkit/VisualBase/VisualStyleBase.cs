namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Delegates;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class VisualStyleBase : VisualControlBase
    {
        #region Variables

        private Color _backgroundColor;
        private Color _backgroundDisabledColor;
        private Color _foreColorDisabled;
        private StyleManager _styleManager;

        #endregion

        #region Constructors

        protected VisualStyleBase()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            _styleManager = new StyleManager(Settings.DefaultValue.DefaultStyle);
        }

        [Category(Localize.EventsCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
        public event BackgroundChangedEventHandler BackgroundChanged;

        [Category(Localize.EventsCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
        public event BackgroundChangedEventHandler BackgroundDisabledChanged;

        [Category(Localize.EventsCategory.PropertyChanged)]
        [Description("Occours when the ForeColorDisabled property for the control has changed.")]
        public event ForeColorDisabledChangedEventHandler ForeColorDisabledChanged;

        [Category(Localize.EventsCategory.PropertyChanged)]
        [Description("Occours when the theme changed for the control.")]
        public event ThemeChangedEventHandler ThemeChanged;

        #endregion

        #region Properties

        [Category(Localize.PropertiesCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
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

        [Category(Localize.PropertiesCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
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

        [Category(Localize.PropertiesCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
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

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Color BackgroundStateColor { get; private set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal LinearGradientBrush BackgroundStateGradientBrush { get; private set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Gradient[] ControlBrushCollection { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal StyleManager StyleManager
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

        #endregion

        #region Events

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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ForeColor = Enabled ? ForeColor : ForeColorDisabled;
            BackgroundStateColor = Enabled ? _backgroundColor : _backgroundDisabledColor;
            BackgroundStateGradientBrush = GDI.GetControlBrush(e.Graphics, Enabled, MouseState, ControlBrushCollection, ClientRectangle);
            ControlGraphicsPath = VisualBorderRenderer.GetBorderShape(ClientRectangle, ControlBorder);

            e.Graphics.Clear(Parent.BackColor);
            e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, Width + 1, Height + 1));
            VisualBackgroundRenderer.DrawBackground(e.Graphics, ControlBorder, ClientRectangle, BackgroundStateColor, MouseState);
        }

        protected virtual void OnThemeChanged(ThemeEventArgs e)
        {
            ThemeChanged?.Invoke(e);
        }

        /// <summary>Update the visual style on the control.</summary>
        /// <param name="control">The control to update the theme for.</param>
        /// <param name="style">The theme style.</param>
        internal void UpdateTheme(VisualStyleBase control, Styles style)
        {
            _styleManager.UpdateStyle(style);

            Font = _styleManager.Font;
            ForeColor = _styleManager.FontStyle.ForeColor;
            ForeColorDisabled = _styleManager.FontStyle.ForeColorDisabled;
            _backgroundColor = StyleManager.ControlStyle.Background(0);
            _backgroundDisabledColor = StyleManager.FontStyle.ForeColorDisabled;

            ControlBrushCollection = new[]
                {
                    _styleManager.ControlStatesStyle.ControlEnabled,
                    _styleManager.ControlStatesStyle.ControlHover,
                    _styleManager.ControlStatesStyle.ControlPressed,
                    _styleManager.ControlStatesStyle.ControlDisabled
                };

            control.Invalidate();

            OnThemeChanged(new ThemeEventArgs(control, style));
        }

        #endregion
    }
}