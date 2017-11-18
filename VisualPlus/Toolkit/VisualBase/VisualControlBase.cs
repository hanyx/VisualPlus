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
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class VisualControlBase : Control
    {
        #region Variables

        private Color _foreColorDisabled;
        private MouseStates _mouseState;
        private VisualStyleManager _styleManager;
        private TextRenderingHint _textRenderingHint;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.VisualBase.VisualControlBase" /> class.</summary>
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

        [Category(Localization.Category.Events.Appearance)]
        [Description(Property.Color)]
        public event BackgroundChangedEventHandler BackgroundDisabledChanged;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description("Occours when the ForeColorDisabled property for the control has changed.")]
        public event ForeColorDisabledChangedEventHandler ForeColorDisabledChanged;

        [Category(Localization.Category.Events.Mouse)]
        [Description("Occours when the MouseState of the control has changed.")]
        public event MouseStateChangedEventHandler MouseStateChanged;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description("Occours when the TextRenderingHint property has changed.")]
        public event TextRenderingChangedEventHandler TextRenderingHintChanged;

        #endregion

        #region Properties

        [Category(Propertys.Layout)]
        [Description(Property.AutoSize)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override bool AutoSize { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LinearGradientBrush BackgroundStateGradientBrush { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Gradient[] ControlBrushCollection { get; set; }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
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

        [Category(Propertys.Appearance)]
        [Description(Property.MouseState)]
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
                Invalidate();
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

        [Category(Propertys.Appearance)]
        [Description(Property.TextRenderingHint)]
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

        /// <summary>Retrieves the fore state color.</summary>
        /// <param name="control">The control.</param>
        /// <returns>The <see cref="Color" />.</returns>
        public Color GetForeColorState(VisualControlBase control)
        {
            return control.Enabled ? control.ForeColor : control.ForeColorDisabled;
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

        // Reset all the controls to the user's default Control color. 
        private void ResetAllControlsBackColor(Control control)
        {
            control.BackColor = SystemColors.Control;
            control.ForeColor = SystemColors.ControlText;
            if (control.HasChildren)
            {
                // Recursively call this method for each child control.
                foreach (Control childControl in control.Controls)
                {
                    ResetAllControlsBackColor(childControl);
                }
            }
        }

        #endregion
    }
}