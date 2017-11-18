namespace VisualPlus.Managers
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.Controls.Interactivity;

    #endregion

    [Description("The control manager.")]
    public sealed class ControlManager
    {
        #region Events

        /// <summary>Gets the checked VisualRadioButton.</summary>
        /// <param name="control">The container control.</param>
        /// <returns>The checked VisualRadioButton.</returns>
        public static VisualRadioButton GetCheckedFromContainer(Control control)
        {
            return control.Controls.OfType<VisualRadioButton>().FirstOrDefault(r => r.Checked);
        }

        /// <summary>Gets the namespace location from the control.</summary>
        /// <param name="controlName">The control Name.</param>
        /// <returns>Returns namespace name.</returns>
        public static string GetControlNamespace(Control controlName)
        {
            return controlName.GetType().Namespace;
        }

        /// <summary>Gets the control type.</summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="controlName">The control name.</param>
        /// <returns>The control type.</returns>
        private static T ControlType<T>(string controlName)
        {
            return (T)Activator.CreateInstance(Type.GetType(controlName));
        }

        #endregion
    }
}