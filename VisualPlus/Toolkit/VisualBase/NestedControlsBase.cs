namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public abstract class NestedControlsBase : ContainedControlBase
    {
        #region Constructors

        protected NestedControlsBase()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        #endregion

        #region Events

        protected override void OnBackgroundChanged(ColorEventArgs e)
        {
            base.OnBackgroundChanged(e);
            GDI.ApplyContainerBackColorChange(this, Background);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            GDI.SetControlBackColor(e.Control, Background, false);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            GDI.SetControlBackColor(e.Control, Background, true);
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

        #endregion
    }
}