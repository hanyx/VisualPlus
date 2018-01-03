namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System.ComponentModel;
    using System.Runtime.InteropServices;

    using VisualPlus.Delegates;
    using VisualPlus.EventArgs;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class ToggleBase : VisualStyleBase
    {
        #region Constructors

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description("Occours when the toggle has been changed on the control.")]
        public event ToggleChangedEventHandler ToggleChanged;

        #endregion

        #region Properties

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal bool Toggle { get; set; }

        #endregion

        #region Events

        protected virtual void OnToggleChanged(ToggleEventArgs e)
        {
            ToggleChanged?.Invoke(e);
        }

        #endregion
    }
}