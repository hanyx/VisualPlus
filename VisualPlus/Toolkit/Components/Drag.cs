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
    using VisualPlus.Localization.Category;

    #endregion

    [Description("The VisualPlus drag component enables controls to be dragged.")]
    [TypeConverter(typeof(DragConverter))]
    public class Drag
    {
        #region Variables

        private readonly Cursor _default = Cursors.SizeAll;

        private Control _control;
        private Cursor _cursorMove;
        private bool _enabled;
        private Point _lastPosition;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Drag" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        public Drag(Control control)
        {
            _cursorMove = _default;
            _control = control;
        }

        /// <summary>Initializes a new instance of the <see cref="Drag" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="enabled">Dragging enabled state.</param>
        /// <param name="moveCursor">The move Cursor.</param>
        public Drag(Control control, bool enabled, Cursor moveCursor)
        {
            _cursorMove = moveCursor;
            _control = control;
            _enabled = enabled;
            if (_enabled)
            {
                AttachEvents();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Drag" /> class.</summary>
        /// <param name="control">The control to attach.</param>
        /// <param name="enabled">Dragging enabled state.</param>
        public Drag(Control control, bool enabled)
        {
            _cursorMove = _default;
            _control = control;
            _enabled = enabled;

            if (_enabled)
            {
                AttachEvents();
            }
        }

        [Category(Event.DragDrop)]
        [Description(Localization.Descriptions.Event.ControlDragChanged)]
        public event ControlDragEventHandler ControlDrag;

        [Category(Event.PropertyChanged)]
        [Description(Localization.Descriptions.Event.CursorChanged)]
        public event ControlDragCursorChangedEventHandler ControlDragCursorChanged;

        [Category(Event.PropertyChanged)]
        [Description(Localization.Descriptions.Event.ControlDragToggleChanged)]
        public event ControlDragToggleEventHandler ControlDragToggle;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.Description.Common.Cursor)]
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
        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.Description.Common.Toggle)]
        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;

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

        [Browsable(false)]
        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.IsDragging)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool IsDragging { get; private set; }

        #endregion

        #region Events

        /// <summary>Attach the extension events to the control.</summary>
        public void AttachEvents()
        {
            _control.MouseDown += ControlMouseDown;
            _control.MouseMove += ControlMouseMove;
            _control.MouseUp += ControlMouseUp;

            OnControlDragToggle(new ToggleEventArgs(_enabled));
        }

        /// <summary>Detach the extension events to the control.</summary>
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
            if (_enabled && (e.Button == MouseButtons.Left))
            {
                _control.Left += e.Location.X - _lastPosition.X;
                _control.Top += e.Location.Y - _lastPosition.Y;
                _control.Cursor = _cursorMove;
                IsDragging = true;

                OnControlDrag(new DragControlEventArgs(e.Location));
            }
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
            Drag drag = value as Drag;

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