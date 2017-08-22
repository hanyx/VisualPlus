namespace VisualPlus.Toolkit.Controls
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
    public class VisualRadioButton : RadioButtonBase
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualRadioButton"/> class.</summary>
        public VisualRadioButton()
        {
            Cursor = Cursors.Hand;
            Size = new Size(125, 23);

            Border = new Border { Rounding = Settings.DefaultValue.Rounding.RoundedRectangle };

            CheckMark = new Checkmark(ClientRectangle)
                {
                    Style = Checkmark.CheckType.Shape,
                    Location = new Point(3, 8),
                    ImageSize = new Size(19, 16),
                    ShapeSize = new Size(8, 8),
                    ShapeRounding = Settings.DefaultValue.Rounding.Default
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

            Background = StyleManager.ControlStyle.Background(0);
            BackgroundDisabled = StyleManager.ControlStyle.Background(0);

            CheckMark.EnabledGradient = StyleManager.CheckmarkStyle.EnabledGradient;
            CheckMark.DisabledGradient = StyleManager.CheckmarkStyle.DisabledGradient;

            ControlBrushCollection = new[]
                {
                    StyleManager.ControlStatesStyle.ControlEnabled,
                    StyleManager.ControlStatesStyle.ControlHover,
                    StyleManager.ControlStatesStyle.ControlPressed,
                    StyleManager.ControlStatesStyle.ControlDisabled
                };

            Border.Color = StyleManager.BorderStyle.Color;
            Border.HoverColor = StyleManager.BorderStyle.HoverColor;

            Invalidate();
        }

        #endregion
    }
}