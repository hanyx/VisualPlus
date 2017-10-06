namespace VisualPlus.Toolkit.ActionList
{
    #region Namespace

    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    using VisualPlus.Toolkit.Controls.DataManagement;

    #endregion

    internal class VisualCheckedListBoxTasks : ControlDesigner
    {
        #region Variables

        private DesignerActionListCollection actionListCollection;

        #endregion

        #region Properties

        /// <summary>Gets the design-time action lists supported by the component associated with the designer.</summary>
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionListCollection == null)
                {
                    actionListCollection = new DesignerActionListCollection { new VisualCheckedListBoxActionList(Component) };
                }

                return actionListCollection;
            }
        }

        #endregion
    }

    internal class VisualCheckedListBoxActionList : DesignerActionList
    {
        #region Variables

        private VisualCheckedListBox _checkedListBox;
        private IComponentChangeService _service;

        private VisualCheckedListBox buttonControl;
        private DesignerActionUIService designerService;

        #endregion

        #region Constructors

        public VisualCheckedListBoxActionList(IComponent component) : base(component)
        {
            buttonControl = (VisualCheckedListBox)component;
            designerService = (DesignerActionUIService)GetService(typeof(DesignerActionUIService));
        }

        #endregion

        #region Properties

        [Category("Data")]
        [Description("The items in the VisualListBox.")]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual CheckedListBox.ObjectCollection Items
        {
            get
            {
                return buttonControl.Items;
            }
        }

        #endregion

        #region Events

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection
                {
                    new DesignerActionHeaderItem("Unbound Mode"),
                    new DesignerActionPropertyItem("Items", "Edit Items...", "Unbound Mode")
                };

            return items;
        }

        #endregion
    }
}