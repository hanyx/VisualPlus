namespace VisualPlus.Designer.ActionList
{
    #region Namespace

    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;

    using VisualPlus.Toolkit.Controls.Editors;

    #endregion

    internal class VisualRichBoxActionList : DesignerActionList
    {
        #region Variables

        private VisualRichTextBox _control;
        private DesignerActionUIService _designerService;

        #endregion

        #region Constructors

        public VisualRichBoxActionList(IComponent component) : base(component)
        {
            _control = (VisualRichTextBox)component;
            _designerService = (DesignerActionUIService)GetService(typeof(DesignerActionUIService));
        }

        #endregion

        #region Properties

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(false)]
        public virtual string Text
        {
            get
            {
                return _control.Text;
            }

            set
            {
                _control.Text = value;
            }
        }

        #endregion

        #region Events

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection
                {
                    new DesignerActionPropertyItem("Text", "Edit Text:")
                };

            return items;
        }

        #endregion
    }
}