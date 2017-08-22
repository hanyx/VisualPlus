namespace VisualPlus.Toolkit.Controls
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(ProgressBar))]
    [DefaultEvent("Click")]
    [DefaultProperty("Value")]
    [Description("The Visual Gauge")]

    // [Designer(ControlManager.FilterProperties.VisualProgressBar)]
    public class VisualGauge : VisualControlBase
    {
        #region Constructors

        public VisualGauge()
        {
            BackColor = Color.Transparent;
        }

        #endregion
    }
}