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

    [TypeConverter(typeof(HoverColorStateConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The hover color state of a component.")]
    [Category(PropertyCategory.Appearance)]
    public class HoverColorState : ColorState
    {
        #region Variables

        private Color _hover;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="HoverColorState" /> class.</summary>
        /// <param name="disabled">The disabled color</param>
        /// <param name="enabled">The enabled color.</param>
        /// <param name="hover">The hover color.</param>
        public HoverColorState(Color disabled, Color enabled, Color hover)
        {
            Disabled = disabled;
            Enabled = enabled;
            _hover = hover;
        }

        /// <summary>Initializes a new instance of the <see cref="HoverColorState" /> class.</summary>
        public HoverColorState()
        {
        }

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BackColorStateChangedEventHandler HoverColorChanged;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Color)]
        public Color Hover
        {
            get
            {
                return _hover;
            }

            set
            {
                _hover = value;
                OnDisabledColorChanged(new ColorEventArgs(_hover));
            }
        }

        /// <summary>Gets a value indicating whether this <see cref="ControlColorState" /> is empty.</summary>
        [Browsable(false)]
        public new bool IsEmpty
        {
            get
            {
                return _hover.IsEmpty && Disabled.IsEmpty && Enabled.IsEmpty;
            }
        }

        #endregion

        #region Events

        /// <summary>Get the control back color state.</summary>
        /// <param name="hoverColorState">The hover Color State.</param>
        /// <param name="enabled">The enabled toggle.</param>
        /// <param name="mouseState">The mouse state.</param>
        /// <returns>
        ///     <see cref="Color" />
        /// </returns>
        public static Color BackColorState(HoverColorState hoverColorState, bool enabled, MouseStates mouseState)
        {
            Color _color;

            if (enabled)
            {
                switch (mouseState)
                {
                    case MouseStates.Normal:
                        {
                            _color = hoverColorState.Enabled;
                            break;
                        }

                    case MouseStates.Hover:
                        {
                            _color = hoverColorState.Hover;
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
                _color = hoverColorState.Disabled;
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
            }

            _stringBuilder.Append("]");

            return _stringBuilder.ToString();
        }

        protected virtual void OnHoverColorChanged(ColorEventArgs e)
        {
            HoverColorChanged?.Invoke(e);
        }

        #endregion
    }

    public class HoverColorStateConverter : ExpandableObjectConverter
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
            HoverColorState _hoverColorState;
            object result;

            result = null;
            _hoverColorState = value as HoverColorState;

            if ((_hoverColorState != null) && (destinationType == typeof(string)))
            {
                // result = borderStyle.ToString();
                result = "Color State Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(HoverColorStateConverter))]
    public class ObjectHoverColorStateWrapper
    {
        #region Constructors

        public ObjectHoverColorStateWrapper()
        {
        }

        public ObjectHoverColorStateWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}