namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.Enumerators;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;
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
                    Style = CheckStyle.CheckType.Character
                };

            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        #endregion

        #region Events

        public void UpdateTheme(Styles style)
        {
            StyleManager = new VisualStyleManager(style);

            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;
            Font = StyleManager.Font;

            BoxColorState.Enabled = StyleManager.ControlStyle.Background(0);
            BoxColorState.Disabled = Color.FromArgb(224, 224, 224);
            BoxColorState.Hover = Color.FromArgb(224, 224, 224);
            BoxColorState.Pressed = Color.Silver;

            CheckStyle.CheckColor = StyleManager.CheckmarkStyle.CheckColor;

            Border.Color = StyleManager.ShapeStyle.Color;
            Border.HoverColor = StyleManager.BorderStyle.HoverColor;
            Invalidate();
        }

        #endregion
    }
}