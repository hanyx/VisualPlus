namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Managers;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(RadioButton))]
    [DefaultEvent("ToggleChanged")]
    [DefaultProperty("Checked")]
    [Description("The Visual RadioButton")]
    [Designer(ControlManager.FilterProperties.VisualRadioButton)]
    public class VisualRadioButton : RadioButtonBase, IThemeSupport
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualRadioButton" /> class.</summary>
        public VisualRadioButton()
        {
            Cursor = Cursors.Hand;
            Size = new Size(125, 23);

            Border = new Border { Rounding = Settings.DefaultValue.Rounding.RoundedRectangle };

            CheckStyle = new CheckStyle(ClientRectangle)
                {
                    Style = CheckStyle.CheckType.Shape,
                    ShapeRounding = Settings.DefaultValue.Rounding.Default,
                    Bounds = new Rectangle(new Point(), new Size(8, 8))
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

            CheckStyle.Color = StyleManager.CheckmarkStyle.EnabledGradient.Colors[0];

            BoxColorState.Enabled = StyleManager.ControlStyle.Background(0);
            BoxColorState.Disabled = Color.FromArgb(224, 224, 224);
            BoxColorState.Hover = Color.FromArgb(224, 224, 224);
            BoxColorState.Pressed = Color.Silver;

            Border.Color = StyleManager.BorderStyle.Color;
            Border.HoverColor = StyleManager.BorderStyle.HoverColor;

            Invalidate();
        }

        #endregion
    }
}