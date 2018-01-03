namespace VisualPlus.Managers
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.Controls.Interactivity;

    #endregion

    [Description("The control manager.")]
    public sealed class ControlManager
    {
        #region Events

        /// <summary>Centers the control inside the parent control.</summary>
        /// <param name="control">The control to center.</param>
        /// <param name="parent">The parent control.</param>
        /// <param name="centerX">Center X coordinate.</param>
        /// <param name="centerY">Center Y coordinate.</param>
        public static void CenterControl(Control control, Control parent, bool centerX, bool centerY)
        {
            Point _controlLocation = control.Location;

            if (centerX)
            {
                _controlLocation.X = (parent.Width - control.Width) / 2;
            }

            if (centerY)
            {
                _controlLocation.Y = (parent.Height - control.Height) / 2;
            }

            control.Location = _controlLocation;
        }

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

        /// <summary>Determines whether the object has the method.</summary>
        /// <param name="objectToCheck">The object.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool HasMethod(object objectToCheck, string methodName)
        {
            Type _methodType = objectToCheck.GetType();
            return _methodType.GetMethod(methodName) != null;
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