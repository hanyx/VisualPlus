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

        /// <summary>Update the internal style manager and invalidate.</summary>
        /// <param name="style">The style.</param>
        public void UpdateTheme(Styles style)
        {
            _styleManager.UpdateStyle(style);
            Invalidate();
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Color stateColor = Enabled ? _backgroundColor : _backgroundDisabledColor;
            ControlGraphicsPath = Border.GetBorderShape(ClientRectangle, ControlBorder);

            Graphics graphics = e.Graphics;
            graphics.Clear(Parent.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.SetClip(ControlGraphicsPath);
            graphics.FillRectangle(new SolidBrush(stateColor), ClientRectangle);
            graphics.ResetClip();
            Border.DrawBorderStyle(graphics, ControlBorder, MouseState, ControlGraphicsPath);

            ForeColor = Enabled ? ForeColor : ForeColorDisabled;
        }

        #endregion
    }
}