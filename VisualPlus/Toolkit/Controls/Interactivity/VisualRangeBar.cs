#region Namespace

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VisualPlus.Toolkit.VisualBase;

#endregion

namespace VisualPlus.Toolkit.Controls.Interactivity
{
    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent("Click")]
    [DefaultProperty("Value")]
    [Description("The Visual RangeBar")]

    // [Designer(ControlManager.FilterProperties.VisualProgressBar)]
    public class VisualRangeBar : VisualControlBase
    {
        #region Constructors

        public VisualRangeBar()
        {
            BackColor = Color.Transparent;
        }

        #endregion
    }
}