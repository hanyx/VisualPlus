namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Delegates;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;

    #endregion

    [Obsolete]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class VisualControlBase : Control
    {
        #region Variables

        private MouseStates _mouseState;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="VisualControlBase" /> class.</summary>
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
        }

        [Category(Localization.Category.Events.Appearance)]
        [Description(Property.Color)]
        public event BackgroundChangedEventHandler BackgroundDisabledChanged;

        [Category(Localization.Category.Events.Mouse)]
        [Description("Occours when the MouseState of the control has changed.")]
        public event MouseStateChangedEventHandler MouseStateChanged;

        #endregion

        #region Properties

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

        #endregion

        #region Events

        protected virtual void OnBackgroundDisabledChanged(ColorEventArgs e)
        {
            BackgroundDisabledChanged?.Invoke(e);
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