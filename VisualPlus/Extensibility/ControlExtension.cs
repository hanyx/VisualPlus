namespace VisualPlus.Extensibility
{
    #region Namespace

    using System.Windows.Forms;

    using VisualPlus.Managers;

    #endregion

    public static class ControlExtension
    {
        #region Events

        /// <summary>Centers the control inside the parent control.</summary>
        /// <param name="control">The control to center.</param>
        /// <param name="centerX">Center X coordinate.</param>
        /// <param name="centerY">Center Y coordinate.</param>
        /// <returns>The <see cref="Control" />.</returns>
        public static Control ToCenter(this Control control, bool centerX, bool centerY)
        {
            ControlManager.CenterControl(control, control.Parent, centerX, centerY);
            return control;
        }

        #endregion
    }
}