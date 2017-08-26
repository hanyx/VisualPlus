#region Namespace

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VisualPlus.Managers;
using VisualPlus.Structure;
using VisualPlus.Toolkit.Components;
using VisualPlus.Toolkit.VisualBase;

#endregion

namespace VisualPlus.Toolkit.Controls.Interactivity
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(CheckBox))]
    [DefaultEvent("ToggleChanged")]
    [DefaultProperty("Checked")]
    [Description("The Visual CheckBox")]
    [Designer(ControlManager.FilterProperties.VisualCheckBox)]
    public class VisualCheckBox : CheckBoxBase
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualCheckBox" /> class.</summary>
        public VisualCheckBox()
        {
            Cursor = Cursors.Hand;
            Size = new Size(125, 23);

            Border = new Border {Rounding = Settings.DefaultValue.Rounding.BoxRounding};

            CheckMark = new Checkmark(ClientRectangle)
            {
                Style = Checkmark.CheckType.Character,
                Location = new Point(-1, 5),
                ImageSize = new Size(19, 16),
                ShapeSize = new Size(8, 8)
            };

            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        #endregion

        #region Events

        public void UpdateTheme(Enumerators.Styles style)
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