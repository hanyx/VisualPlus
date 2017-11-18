namespace VisualPlus.Designer
{
    #region Namespace

    using System.Collections;
    using System.ComponentModel.Design;
    using System.Windows.Forms.Design;

    using VisualPlus.Designer.ActionList;

    #endregion

    internal class VisualTextBoxDesigner : ControlDesigner
    {
        #region Variables

        private DesignerActionListCollection _actionListCollection;

        #endregion

        #region Properties

        /// <summary>Gets the design-time action lists supported by the component associated with the designer.</summary>
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_actionListCollection == null)
                {
                    _actionListCollection = new DesignerActionListCollection
                        {
                            new VisualTextBoxActionList(Component)
                        };
                }

                return _actionListCollection;
            }
        }

        #endregion

        #region Events

        protected override void PreFilterProperties(IDictionary properties)
        {
            properties.Remove("ImeMode");
            properties.Remove("Padding");
            properties.Remove("FlatAppearance");
            properties.Remove("FlatStyle");
            properties.Remove("AutoEllipsis");
            properties.Remove("UseCompatibleTextRendering");
            properties.Remove("Image");
            properties.Remove("ImageAlign");
            properties.Remove("ImageIndex");
            properties.Remove("ImageKey");
            properties.Remove("ImageList");
            properties.Remove("TextImageRelation");
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("UseVisualStyleBackColor");
            properties.Remove("RightToLeft");

            base.PreFilterProperties(properties);
        }

        #endregion
    }
}