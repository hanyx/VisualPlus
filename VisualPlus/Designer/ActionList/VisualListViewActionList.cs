namespace VisualPlus.Designer.ActionList
{
    #region Namespace

    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.Controls.DataManagement;

    #endregion

    internal class VisualListViewActionList : DesignerActionList
    {
        #region Variables

        private VisualListView _control;
        private DesignerActionUIService _designerService;

        private bool _dockState;
        private string _dockText;

        #endregion

        #region Constructors

        public VisualListViewActionList(IComponent component) : base(component)
        {
            _control = (VisualListView)component;
            _designerService = (DesignerActionUIService)GetService(typeof(DesignerActionUIService));

            _dockText = "Dock in Parent Container.";
            _dockState = false;
        }

        #endregion

        #region Properties

        [Category("Data")]
        [Description("The items in the VisualListView.")]
        [Editor("System.Windows.Forms.Design.ColumnHeaderCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual ListView.ColumnHeaderCollection Columns
        {
            get
            {
                return _control.Columns;
            }
        }

        [Category("Data")]
        [Description("The items in the VisualListView.")]
        [Editor("System.Windows.Forms.Design.ListViewGroupCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual ListViewGroupCollection Groups
        {
            get
            {
                return _control.Groups;
            }
        }

        [Category("Data")]
        [Description("The items in the VisualListView.")]
        [Editor("System.Windows.Forms.Design.ListViewItemCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual ListView.ListViewItemCollection Items
        {
            get
            {
                return _control.Items;
            }
        }

        [Category("Behavior")]
        [Description("Selects one of five different views that can be shown in.")]
        [DefaultValue(false)]
        public virtual View View
        {
            get
            {
                return _control.View;
            }

            set
            {
                _control.View = value;
            }
        }

        #endregion

        #region Events

        public void DockContainer()
        {
            if (!_dockState)
            {
                _control.Dock = DockStyle.None;
                _dockText = ContainerText.Docked;
                _dockState = true;
            }
            else
            {
                _control.Dock = DockStyle.Fill;
                _dockText = ContainerText.Undock;
                _dockState = false;
            }

            _designerService.Refresh(_control);
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection
                {
                    new DesignerActionPropertyItem("Items", "Edit Items..."),
                    new DesignerActionPropertyItem("Columns", "Edit Columns..."),
                    new DesignerActionPropertyItem("Groups", "Edit Groups..."),
                    new DesignerActionPropertyItem("View", "View:"),
                    new DesignerActionMethodItem(this, "DockContainer", _dockText)
                };

            return items;
        }

        #endregion

        #region Methods

        private struct ContainerText
        {
            public const string Docked = "Dock in Parent Container";
            public const string Undock = "Undock in Parent Container.";
        }

        #endregion
    }
}