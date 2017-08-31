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
    using VisualPlus.Toolkit.Components;

    #endregion

    [TypeConverter(typeof(ColorStateConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The color states of a component.")]
    public class ColorState
    {
        #region Variables

        private Color _disabled;
        private Color _enabled;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ColorState" /> class.</summary>
        /// <param name="disabled">The disabled color.</param>
        /// <param name="enabled">The normal color.</param>
        public ColorState(Color disabled, Color enabled)
        {
            _disabled = disabled;
            _enabled = enabled;
        }

        /// <summary>Initializes a new instance of the <see cref="ColorState" /> class.</summary>
        public ColorState()
        {
            VisualStyleManager _styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);
            _disabled = Color.FromArgb(224, 224, 224);
            _enabled = _styleManager.ControlStyle.Background(0);
        }

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BackColorStateChangedEventHandler DisabledColorChanged;

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BackColorStateChangedEventHandler NormalColorChanged;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Color)]
        public Color Disabled
        {
            get
            {
                return _disabled;
            }

            set
            {
                _disabled = value;
                OnDisabledColorChanged(new ColorEventArgs(_disabled));
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Color)]
        public Color Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
                OnDisabledColorChanged(new ColorEventArgs(_enabled));
            }
        }

        /// <summary>Gets a value indicating whether this <see cref="ColorState" /> is empty.</summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return _disabled.IsEmpty && _enabled.IsEmpty;
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
                _stringBuilder.Append("Normal=");
                _stringBuilder.Append(Enabled);
            }

            _stringBuilder.Append("]");

            return _stringBuilder.ToString();
        }

        protected virtual void OnDisabledColorChanged(ColorEventArgs e)
        {
            DisabledColorChanged?.Invoke(e);
        }

        protected virtual void OnNormalColorChanged(ColorEventArgs e)
        {
            NormalColorChanged?.Invoke(e);
        }

        #endregion
    }

    public class ColorStateConverter : ExpandableObjectConverter
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
                return new ObjectColorStyleWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            ColorState _colorState;
            object result;

            result = null;
            _colorState = value as ColorState;

            if ((_colorState != null) && (destinationType == typeof(string)))
            {
                // result = borderStyle.ToString();
                result = "Color State Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(ColorStateConverter))]
    public class ObjectColorStyleWrapper
    {
        #region Constructors

        public ObjectColorStyleWrapper()
        {
        }

        public ObjectColorStyleWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}