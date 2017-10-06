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
    using VisualPlus.EventArgs;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;

    #endregion

    [TypeConverter(typeof(ControlColorStateConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The control color state of a component.")]
    public class ControlColorState : ColorState
    {
        #region Variables

        private Color _hover;
        private Color _pressed;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:VisualPlus.Structure.ControlColorState" /> class.</summary>
        /// <param name="hover">The hover.</param>
        /// <param name="pressed">The pressed.</param>
        public ControlColorState(Color hover, Color pressed)
        {
            _hover = hover;
            _pressed = pressed;
        }

        /// <summary>Initializes a new instance of the <see cref="ControlColorState" /> class.</summary>
        public ControlColorState()
        {
            // VisualStyleManager _styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);
            _hover = Color.FromArgb(224, 224, 224);
            _pressed = Color.Silver;
        }

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BackColorStateChangedEventHandler HoverColorChanged;

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BackColorStateChangedEventHandler PressedColorChanged;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Color)]
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
                return _hover.IsEmpty && _pressed.IsEmpty && Disabled.IsEmpty && Enabled.IsEmpty;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Color)]
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

        protected virtual void OnHoverColorChanged(ColorEventArgs e)
        {
            HoverColorChanged?.Invoke(e);
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