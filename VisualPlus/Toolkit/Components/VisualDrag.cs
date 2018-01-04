namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    using VisualPlus.Delegates;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Localization.Descriptions;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Component))]
    [Description("The VisualPlus drag component enables controls to be dragged.")]
    [TypeConverter(typeof(DragConverter))]
    public class VisualDrag : Component
    {
        #region Variables

        private Control _control;
        private Cursor _cursorMove;
        private bool _enabled;
        private bool _horizontal;
        private Point _lastPosition;
        private bool _vertical;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualDrag" /> class.</summary>
        /// <param name="container">The container.</param>
        public VisualDrag(IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>Initializes a new instance of the <see cref="VisualDrag" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        public VisualDrag(Control control) : this()
        {
            _control = control;
        }

        /// <summary>Initializes a new instance of the <see cref="VisualDrag" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="enabled">Dragging enabled state.</param>
        public VisualDrag(Control control, bool enabled) : this()
        {
            _control = control;
            _enabled = enabled;

            if (_enabled)
            {
                AttachEvents();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="VisualDrag" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="enabled">Dragging enabled state.</param>
        /// <param name="moveCursor">The move Cursor.</param>
        public VisualDrag(Control control, bool enabled, Cursor moveCursor)
        {
            _cursorMove = moveCursor;
            _control = control;
            _enabled = enabled;

            if (_enabled)
            {
                AttachEvents();
            }
        }

        /// <summary>Prevents a default instance of the <see cref="VisualDrag" /> class from being created.</summary>
        private VisualDrag()
        {
            _cursorMove = Cursors.SizeAll;
            _vertical = true;
            _horizontal = true;
        }

        [Category(Localization.Category.Events.DragDrop)]
        [Description(Event.ControlDragChanged)]
        public event ControlDragEventHandler ControlDrag;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.CursorChanged)]
        public event ControlDragCursorChangedEventHandler ControlDragCursorChanged;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.ControlDragToggleChanged)]
        public event ControlDragToggleEventHandler ControlDragToggle;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Behavior)]
        [Description("The control to attach this component.")]
        public Control Control
        {
            get
            {
                return _control;
            }

            set
            {
                _control = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Cursor)]
        public Cursor CursorMove
        {
            get
            {
                return _cursorMove;
            }

            set
            {
                if ((_cursorMove == null) || (_cursorMove == value))
                {
                    return;
                }

                _cursorMove = value;
                OnControlDragCursorChanged(new CursorChangedEventArgs(_cursorMove));
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Toggle)]
        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;

                if (_control == null)
                {
                    return;
                }

                if (_enabled)
                {
                    AttachEvents();
                }
                else
                {
                    DetachEvents();
                }
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Toggle)]
        public bool Horizontal
        {
            get
            {
                return _horizontal;
            }

            set
            {
                _horizontal = value;
            }
        }

        [Browsable(false)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.IsDragging)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool IsDragging { get; private set; }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Toggle)]
        public bool Vertical
        {
            get
            {
                return _vertical;
            }

            set
            {
                _vertical = value;
            }
        }

        #endregion

        #region Events

        /// <summary>Hooks the drag events to the control.</summary>
        public void AttachEvents()
        {
            _control.MouseDown += ControlMouseDown;
            _control.MouseMove += ControlMouseMove;
            _control.MouseUp += ControlMouseUp;

            OnControlDragToggle(new ToggleEventArgs(_enabled));
        }

        /// <summary>Unhooks the drag events from the control.</summary>
        public void DetachEvents()
        {
            _control.MouseDown -= ControlMouseDown;
            _control.MouseMove -= ControlMouseMove;
            _control.MouseUp -= ControlMouseUp;

            OnControlDragToggle(new ToggleEventArgs(_enabled));
        }

        protected virtual void OnControlDrag(DragControlEventArgs e)
        {
            ControlDrag?.Invoke(e);
        }

        protected virtual void OnControlDragCursorChanged(CursorChangedEventArgs e)
        {
            ControlDragCursorChanged?.Invoke(e);
        }

        protected virtual void OnControlDragToggle(ToggleEventArgs e)
        {
            ControlDragToggle?.Invoke(e);
        }

        /// <summary>Control mouse down event.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void ControlMouseDown(object sender, MouseEventArgs e)
        {
            if (_enabled)
            {
                _lastPosition = e.Location;
                _control.Cursor = CursorMove;
            }
        }

        /// <summary>Control mouse move event.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void ControlMouseMove(object sender, MouseEventArgs e)
        {
            if (!_enabled || (e.Button != MouseButtons.Left))
            {
                return;
            }

            if (_horizontal)
            {
                _control.Left += e.Location.X - _lastPosition.X;
            }

            if (_vertical)
            {
                _control.Top += e.Location.Y - _lastPosition.Y;
            }

            IsDragging = true;
            OnControlDrag(new DragControlEventArgs(e.Location));
        }

        /// <summary>Control mouse up event.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void ControlMouseUp(object sender, MouseEventArgs e)
        {
            if (_enabled)
            {
                _control.Cursor = Cursors.Default;
            }
        }

        #endregion
    }

    public class DragConverter : ExpandableObjectConverter
    {
        #region Events

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;

            if (stringValue != null)
            {
                return new ObjectDragWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            VisualDrag drag = value as VisualDrag;

            if ((drag != null) && (destinationType == typeof(string)))
            {
                // result = borderStyle.ToString();
                result = "Drag Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(DragConverter))]
    public class ObjectDragWrapper
    {
        #region Constructors

        public ObjectDragWrapper()
        {
        }

        public ObjectDragWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}