namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.Controls.Navigation;

    #endregion

    public sealed class VisualToolStripMenuItem : ToolStripMenuItem
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.VisualBase.VisualToolStripMenuItem" />
        ///     class.
        /// </summary>
        public VisualToolStripMenuItem()
        {
            AutoSize = false;
            Size = new Size(160, 30);
            Font = new Font("Arial", 8.25F);
        }

        #endregion

        #region Events

        protected override ToolStripDropDown CreateDefaultDropDown()
        {
            if (DesignMode)
            {
                return base.CreateDefaultDropDown();
            }

            VisualContextMenuStrip defaultDropDown = new VisualContextMenuStrip();
            defaultDropDown.Items.AddRange(base.CreateDefaultDropDown().Items);
            return defaultDropDown;
        }

        #endregion
    }
}