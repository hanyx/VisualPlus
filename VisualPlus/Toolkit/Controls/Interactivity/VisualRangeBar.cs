namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent("Click")]
    [DefaultProperty("Value")]
    [Description("The Visual RangeBar")]

    // [Designer(ControlManager.FilterProperties.VisualProgressBar)]
    public class VisualRangeBar : VisualStyleBase
    {
    }
}