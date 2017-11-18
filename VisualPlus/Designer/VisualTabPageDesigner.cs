namespace VisualPlus.Designer
{
    #region Namespace

    using System.Collections;
    using System.Windows.Forms.Design;

    #endregion

    internal class VisualTabPageDesigner : ScrollableControlDesigner
    {
        #region Events

        protected override void PreFilterProperties(IDictionary properties)
        {
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("ForeColor");
            properties.Remove("RightToLeft");
            properties.Remove("ImeMode");
            properties.Remove("BorderStyle");
            properties.Remove("Margin");
            properties.Remove("Padding");
            properties.Remove("Enabled");
            properties.Remove("UseVisualStyleBackColor");

            base.PreFilterProperties(properties);
        }

        #endregion
    }
}