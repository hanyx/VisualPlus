namespace VisualPlus.Designer.ActionList
{
    #region Namespace

    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.Controls.DataManagement;

    #endregion

    internal class VisualCheckedListBoxActionList : DesignerActionList
    {
        #region Variables

        private VisualCheckedListBox _control;
        private DesignerActionUIService _designerService;

        #endregion

        #region Constructors

        public VisualCheckedListBoxActionList(IComponent component) : base(component)
        {
            _control = (VisualCheckedListBox)component;
            _designerService = (DesignerActionUIService)GetService(typeof(DesignerActionUIService));
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
                return _control.Items;
            }
        }

        #endregion

        #region Events

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection
                {
                    new DesignerActionPropertyItem("Items", "Edit Items...", "Unbound Mode")
                };

            return items;
        }

        #endregion
    }
}