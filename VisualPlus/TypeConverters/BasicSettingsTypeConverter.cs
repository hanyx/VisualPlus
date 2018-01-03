namespace VisualPlus.TypeConverters
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Globalization;

    using VisualPlus.Structure;

    #endregion

    public class BasicSettingsTypeConverter : ExpandableObjectConverter
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
                return new ObjectBorderSettingsWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            BorderSettings _borderSettings;
            object result;

            result = null;
            _borderSettings = value as BorderSettings;

            // if ((_borderSettings != null) && (destinationType == typeof(string)))
            // {
            // // result = borderStyle.ToString();
            // result = "Settings";
            // }
            result = "Settings";

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(BasicSettingsTypeConverter))]
    public class ObjectBorderSettingsWrapper
    {
        #region Constructors

        public ObjectBorderSettingsWrapper()
        {
        }

        public ObjectBorderSettingsWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}