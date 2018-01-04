namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.EventArgs;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("ToggleChanged")]
    [DefaultProperty("Checked")]
    [Description("The Visual CheckBox")]
    [Designer(typeof(VisualCheckBoxDesigner))]
    [ToolboxBitmap(typeof(VisualCheckBox), "Resources.ToolboxBitmaps.VisualCheckBox.bmp")]
    [ToolboxItem(true)]
    public class VisualCheckBox : CheckBoxBase, IThemeSupport
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualCheckBox" /> class.</summary>
        public VisualCheckBox()
        {
            Cursor = Cursors.Hand;
            Size = new Size(125, 23);

            Border = new Border { Rounding = Settings.DefaultValue.Rounding.BoxRounding };

            CheckStyle = new CheckStyle(ClientRectangle)
                {
                    Style = CheckStyle.CheckType.Checkmark
                };

            UpdateTheme(ThemeManager.Theme);
        }

        #endregion

        #region Events

        public void UpdateTheme(Theme theme)
        {
            try
            {
                Border.Color = theme.BorderSettings.Normal;
                Border.HoverColor = theme.BorderSettings.Hover;

                CheckStyle.CheckColor = theme.OtherSettings.Progress;

                ForeColor = theme.TextSetting.Enabled;
                TextStyle.Enabled = theme.TextSetting.Enabled;
                TextStyle.Disabled = theme.TextSetting.Disabled;

                Font = theme.TextSetting.Font;

                BoxColorState.Enabled = theme.ColorStateSettings.Enabled;
                BoxColorState.Disabled = theme.ColorStateSettings.Disabled;
                BoxColorState.Hover = theme.ColorStateSettings.Hover;
                BoxColorState.Pressed = theme.ColorStateSettings.Pressed;
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }

            Invalidate();
            OnThemeChanged(new ThemeEventArgs(theme));
        }

        #endregion
    }
}