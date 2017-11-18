namespace VisualPlus.Designer
{
    #region Namespace

    using System.Collections;
    using System.ComponentModel.Design;
    using System.Windows.Forms.Design;

    using VisualPlus.Designer.ActionList;

    #endregion

    internal class VisualComboBoxDesigner : ControlDesigner
    {
        #region Variables

        private DesignerActionListCollection actionListCollection;

        #endregion

        #region Properties

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionListCollection == null)
                {
                    actionListCollection = new DesignerActionListCollection
                        {
                            new VisualComboBoxActionList(Component)
                        };
                }

                return actionListCollection;
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
            properties.Remove("DropDownStyle");
            properties.Remove("UseVisualStyleBackColor");
            properties.Remove("RightToLeft");
            properties.Remove("Index");

            base.PreFilterProperties(properties);
        }

        #endregion
    }
}