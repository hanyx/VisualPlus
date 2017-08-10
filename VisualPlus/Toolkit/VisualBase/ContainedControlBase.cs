namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization.Category;
    using VisualPlus.Renders;
    using VisualPlus.Structure;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public abstract class ContainedControlBase : VisualStyleBase
    {
        #region Constructors

        protected ContainedControlBase()
        {
            UpdateTheme(this, Settings.DefaultValue.DefaultStyle);
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Appearance)]
        public Border Border
        {
            get
            {
                return ControlBorder;
            }

            set
            {
                ControlBorder = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            MouseState = MouseStates.Hover;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            MouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            MouseState = MouseStates.Normal;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlGraphicsPath = VisualBorderRenderer.GetBorderShape(ClientRectangle, ControlBorder);
        }

        protected override void OnThemeChanged(ThemeEventArgs e)
        {
            Background = StyleManager.ControlStyle.Background(3);
            base.OnThemeChanged(e);
        }

        /// <summary>Gets the internal control location.</summary>
        /// <param name="shape">The shape of the container control.</param>
        /// <returns>The internal control location.</returns>
        internal Point GetInternalControlLocation(Shape shape)
        {
            return new Point((shape.Rounding / 2) + shape.Thickness + 1, (shape.Rounding / 2) + shape.Thickness + 1);
        }

        /// <summary>Gets the internal control size.</summary>
        /// <param name="size">The size of the container control.</param>
        /// <param name="shape">The shape of the container control.</param>
        /// <returns>The internal control size.</returns>
        internal Size GetInternalControlSize(Size size, Shape shape)
        {
            return new Size(size.Width - shape.Rounding - shape.Thickness - 3, size.Height - shape.Rounding - shape.Thickness - 3);
        }

        #endregion
    }
}