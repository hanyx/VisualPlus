namespace VisualPlus.Toolkit.ActionList
{
    #region Namespace

    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    using VisualPlus.Toolkit.Controls.Editors;

    #endregion

    internal class VisualRichBoxTasks : ControlDesigner
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
                    _actionListCollection = new DesignerActionListCollection { new VisualRichBoxActionList(Component) };
                }

                return _actionListCollection;
            }
        }

        #endregion
    }

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