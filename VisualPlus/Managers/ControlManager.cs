namespace VisualPlus.Managers
{
    #region Namespace

    using System;
    using System.Linq;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.Controls.Interactivity;

    #endregion

    internal class ControlManager
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

        #region Methods

        /// <summary>Bind visual studio designer files here to controls.</summary>
        /// <summary>Note: If you implement more controls or to bind your own designer styles, just add here.</summary>
        public struct FilterProperties
        {
            private const string DesignerFilterPath = NamespaceLocations.FilterPropertiesLocation + "Visual";
            private const string DesignerSuffix = "Designer";

            public const string VisualButton = DesignerFilterPath + "Button" + DesignerSuffix;
            public const string VisualCheckBox = DesignerFilterPath + "CheckBox" + DesignerSuffix;
            public const string VisualRadialProgress = DesignerFilterPath + "CircleProgressBar" + DesignerSuffix;
            public const string VisualComboBox = DesignerFilterPath + "ComboBox" + DesignerSuffix;
            public const string VisualContextMenu = DesignerFilterPath + "ContextMenu" + DesignerSuffix;
            public const string VisualForm = DesignerFilterPath + "Form" + DesignerSuffix;
            public const string VisualGroupBox = DesignerFilterPath + "GroupBox" + DesignerSuffix;
            public const string VisualLabel = DesignerFilterPath + "Label" + DesignerSuffix;
            public const string VisualListBox = DesignerFilterPath + "ListBox" + DesignerSuffix;
            public const string VisualListView = DesignerFilterPath + "ListView" + DesignerSuffix;
            public const string VisualNumericUpDown = DesignerFilterPath + "NumericUpDown" + DesignerSuffix;
            public const string VisualPanel = DesignerFilterPath + "Panel" + DesignerSuffix;
            public const string VisualProgressBar = DesignerFilterPath + "ProgressBar" + DesignerSuffix;
            public const string VisualProgressIndicator = DesignerFilterPath + "ProgressIndicator" + DesignerSuffix;
            public const string VisualRadioButton = DesignerFilterPath + "RadioButton" + DesignerSuffix;
            public const string VisualRating = DesignerFilterPath + "Rating" + DesignerSuffix;
            public const string VisualRichTextBox = DesignerFilterPath + "RichTextBox" + DesignerSuffix;
            public const string VisualSeparator = DesignerFilterPath + "Separator" + DesignerSuffix;
            public const string VisualTabControl = DesignerFilterPath + "TabControl" + DesignerSuffix;
            public const string VisualTextBox = DesignerFilterPath + "TextBox" + DesignerSuffix;
            public const string VisualToggle = DesignerFilterPath + "Toggle" + DesignerSuffix;
            public const string VisualToolTip = DesignerFilterPath + "ToolTip" + DesignerSuffix;
            public const string VisualTrackBar = DesignerFilterPath + "TrackBar" + DesignerSuffix;
        }

        private struct NamespaceLocations
        {
            public const string FilterPropertiesLocation = @"VisualPlus.Toolkit.PropertyFilter.";
        }

        #endregion
    }
}