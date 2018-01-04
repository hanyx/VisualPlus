namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.EventArgs;
    using VisualPlus.Localization;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public abstract class CheckBoxBase : ToggleCheckmarkBase
    {
        #region Variables

        private CheckState _checkState = CheckState.Unchecked;
        private bool _threeState;

        #endregion

        #region Constructors

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(PropertyDescription.Checked)]
        public event EventHandler CheckStateChanged;

        #endregion

        #region Properties

        [DefaultValue(typeof(CheckState), "Unchecked")]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Checked)]
        public CheckState CheckState
        {
            get
            {
                return _checkState;
            }

            set
            {
                if (_checkState != value)
                {
                    // Store new values
                    _checkState = value;
                    bool newChecked = _checkState != CheckState.Unchecked;
                    bool checkedChanged = Checked != newChecked;
                    Checked = newChecked;

                    // Generate events
                    if (checkedChanged)
                    {
                        OnToggleChanged(new ToggleEventArgs(Toggle));
                    }

                    OnCheckStateChanged(EventArgs.Empty);

                    // Repaint
                    Invalidate();
                }
            }
        }

        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Toggle)]
        [DefaultValue(false)]
        public bool ThreeState
        {
            get
            {
                return _threeState;
            }

            set
            {
                if (_threeState != value)
                {
                    _threeState = value;
                    Invalidate();
                }
            }
        }

        #endregion

        #region Events

        protected virtual void OnCheckStateChanged(EventArgs e)
        {
            CheckStateChanged?.Invoke(this, e);
        }

        protected override void OnClick(EventArgs e)
        {
            switch (CheckState)
            {
                case CheckState.Unchecked:
                    {
                        CheckState = CheckState.Checked;
                        break;
                    }

                case CheckState.Checked:
                    {
                        CheckState = ThreeState ? CheckState.Indeterminate : CheckState.Unchecked;
                        break;
                    }

                case CheckState.Indeterminate:
                    {
                        CheckState = CheckState.Unchecked;
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            base.OnClick(e);
        }

        protected override void OnToggleChanged(ToggleEventArgs e)
        {
            base.OnToggleChanged(e);
            _checkState = Checked ? CheckState.Checked : CheckState.Unchecked;
            OnCheckStateChanged(EventArgs.Empty);
            Invalidate();
        }

        #endregion
    }
}