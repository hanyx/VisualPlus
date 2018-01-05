namespace VisualPlus.Designer
{
    #region Namespace

    using System.Collections;
    using System.Windows.Forms.Design;

    #endregion

    internal class VisualControlBoxButtonDesigner : ControlDesigner
    {
        #region Events

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            properties.Remove("ImeMode");
            properties.Remove("Padding");
            properties.Remove("FlatAppearance");
            properties.Remove("FlatStyle");
            properties.Remove("AutoEllipsis");
            properties.Remove("UseCompatibleTextRendering");
            properties.Remove("ImageAlign");
            properties.Remove("ImageIndex");
            properties.Remove("ImageKey");
            properties.Remove("ImageList");

            // properties.Remove("BackgroundImage");
            // properties.Remove("BackgroundImageLayout");
            // properties.Remove("UseVisualStyleBackColor");
            properties.Remove("RightToLeft");

            properties.Remove("AccessibleDescription");
            properties.Remove("AccessibleName");
            properties.Remove("AccessibleRole");
            properties.Remove("AllowDrop");
            properties.Remove("Visible");
            properties.Remove("BackColor");
        }

        #endregion
    }
}