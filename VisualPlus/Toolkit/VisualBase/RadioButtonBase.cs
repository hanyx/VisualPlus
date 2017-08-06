namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.EventArgs;
    using VisualPlus.Toolkit.Controls;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public abstract class RadioButtonBase : ToggleButtonBase
    {
        #region Properties

        [DefaultValue(false)]
        [Category(Localize.PropertiesCategory.Behavior)]
        [Description(Localize.Description.Checkmark.Checked)]
        public bool Checked
        {
            get
            {
                return Toggle;
            }

            set
            {
                if (Toggle != value)
                {
                    // Store new values
                    Toggle = value;

                    // Generate events
                    OnToggleChanged(new ToggleEventArgs(Toggle));

                    // Repaint
                    Invalidate();
                }
            }
        }

        #endregion

        #region Events

        protected override void OnClick(EventArgs e)
        {
            if (!Checked)
            {
                Checked = true;
            }

            base.OnClick(e);
        }

        protected override void OnToggleChanged(ToggleEventArgs e)
        {
            base.OnToggleChanged(e);
            AutoUpdateOthers();
            Invalidate();
        }

        private void AutoUpdateOthers()
        {
            // Only un-check others if they are checked
            if (Checked)
            {
                Control parent = Parent;
                if (parent != null)
                {
                    // Search all sibling controls
                    foreach (Control control in parent.Controls)
                    {
                        // If another radio button found, that is not us
                        if ((control != this) && control is VisualRadioButton)
                        {
                            // Cast to correct type
                            VisualRadioButton radioButton = (VisualRadioButton)control;

                            // If target allows auto check changed and is currently checked
                            if (radioButton.Checked)
                            {
                                // Set back to not checked
                                radioButton.Checked = false;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}