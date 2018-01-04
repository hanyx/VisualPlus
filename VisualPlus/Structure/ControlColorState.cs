namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;

    using VisualPlus.Delegates;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;

    #endregion

    [TypeConverter(typeof(ControlColorStateConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The control color state of a component.")]
    [Category(PropertyCategory.Appearance)]
    public class ControlColorState : HoverColorState
    {
        #region Variables

        private Color _pressed;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ControlColorState" /> class.</summary>
        /// <param name="disabled">The disabled color</param>
        /// <param name="enabled">The enabled color.</param>
        /// <param name="hover">The hover color.</param>
        /// <param name="pressed">The pressed color.</param>
        public ControlColorState(Color disabled, Color enabled, Color hover, Color pressed)
        {
            Disabled = disabled;
            Enabled = enabled;
            Hover = hover;
            _pressed = pressed;
        }

        /// <summary>Initializes a new instance of the <see cref="ControlColorState" /> class.</summary>
        public ControlColorState()
        {
        }

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BackColorStateChangedEventHandler PressedColorChanged;

        #endregion

        #region Properties

        /// <summary>Gets a value indicating whether this <see cref="ControlColorState" /> is empty.</summary>
        [Browsable(false)]
        public new bool IsEmpty
        {
            get
            {
                return Hover.IsEmpty && _pressed.IsEmpty && Disabled.IsEmpty && Enabled.IsEmpty;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Color)]
        public Color Pressed
        {
            get
            {
                return _pressed;
            }

            set
            {
                _pressed = value;
                OnDisabledColorChanged(new ColorEventArgs(_pressed));
            }
        }

        #endregion

        #region Events

        /// <summary>Get the control back color state.</summary>
        /// <param name="controlColorState">The control color state.</param>
        /// <param name="enabled">The enabled toggle.</param>
        /// <param name="mouseState">The mouse state.</param>
        /// <returns>
        ///     <see cref="Color" />
        /// </returns>
        public static Color BackColorState(ControlColorState controlColorState, bool enabled, MouseStates mouseState)
        {
            Color _color;

            if (enabled)
            {
                switch (mouseState)
                {
                    case MouseStates.Normal:
                        {
                            _color = controlColorState.Enabled;
                            break;
                        }

                    case MouseStates.Hover:
                        {
                            _color = controlColorState.Hover;
                            break;
                        }

                    case MouseStates.Down:
                        {
                            _color = controlColorState.Pressed;
                            break;
                        }

                    default:
                        {
                            throw new ArgumentOutOfRangeException(nameof(mouseState), mouseState, null);
                        }
                }
            }
            else
            {
                _color = controlColorState.Disabled;
            }

            return _color;
        }

        public override string ToString()
        {
            StringBuilder _stringBuilder = new StringBuilder();
            _stringBuilder.Append(GetType().Name);
            _stringBuilder.Append(" [");

            if (IsEmpty)
            {
                _stringBuilder.Append("IsEmpty");
            }
            else
            {
                _stringBuilder.Append("Disabled=");
                _stringBuilder.Append(Disabled);
                _stringBuilder.Append("Hover=");
                _stringBuilder.Append(Hover);
                _stringBuilder.Append("Normal=");
                _stringBuilder.Append(Enabled);
                _stringBuilder.Append("Pressed=");
                _stringBuilder.Append(Pressed);
            }

            _stringBuilder.Append("]");

            return _stringBuilder.ToString();
        }

        protected virtual void OnPressedColorChanged(ColorEventArgs e)
        {
            PressedColorChanged?.Invoke(e);
        }

        #endregion
    }

    public class ControlColorStateConverter : ExpandableObjectConverter
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
                return new ObjectControlColorStateWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            ControlColorState _controlColorState;
            object result;

            result = null;
            _controlColorState = value as ControlColorState;

            if ((_controlColorState != null) && (destinationType == typeof(string)))
            {
                // result = borderStyle.ToString();
                result = "Color State Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(ControlColorStateConverter))]
    public class ObjectControlColorStateWrapper
    {
        #region Constructors

        public ObjectControlColorStateWrapper()
        {
        }

        public ObjectControlColorStateWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}