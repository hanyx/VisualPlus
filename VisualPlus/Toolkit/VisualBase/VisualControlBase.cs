namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

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

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class VisualControlBase : Control
    {
        #region Variables

        private MouseStates _mouseState;
        private TextRenderingHint _textRendererHint;

        #endregion

        #region Constructors

        protected VisualControlBase()
        {
            // Double buffering to reduce drawing flicker.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            // Repaint entire control whenever resizing.
            SetStyle(ControlStyles.ResizeRedraw, true);

            // Drawn double buffered by default
            DoubleBuffered = true;
            ResizeRedraw = true;

            _mouseState = MouseStates.Normal;

            _textRendererHint = Settings.DefaultValue.TextRenderingHint;

            ControlBorder = new Border();
        }

        [Category(Event.Mouse)]
        [Description("Occours when the MouseState of the control has changed.")]
        public event MouseStateChangedEventHandler MouseStateChanged;

        [Category(Event.PropertyChanged)]
        [Description("Occours when the TextRenderingHint property has changed.")]
        public event TextRenderingChangedEventHandler TextRenderingHintChanged;

        #endregion

        #region Properties

        [Category(Property.Layout)]
        [Description(Localization.Descriptions.Property.Description.Common.AutoSize)]
        public new bool AutoSize { get; set; }

        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.MouseState)]
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

        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Strings.TextRenderingHint)]
        public TextRenderingHint TextRenderingHint
        {
            get
            {
                return _textRendererHint;
            }

            set
            {
                _textRendererHint = value;
                OnTextRenderingHintChanged(new TextRenderingEventArgs(_textRendererHint));
                Invalidate();
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Border ControlBorder { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal GraphicsPath ControlGraphicsPath { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Point LastPosition { get; set; }

        #endregion

        #region Events

        protected virtual void OnMouseStateChanged(MouseStateEventArgs e)
        {
            MouseStateChanged?.Invoke(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = _textRendererHint;
        }

        protected virtual void OnTextRenderingHintChanged(TextRenderingEventArgs e)
        {
            TextRenderingHintChanged?.Invoke(e);
        }

        #endregion
    }
}