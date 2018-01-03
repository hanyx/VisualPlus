namespace VisualPlus.TypeConverters
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Globalization;

    using VisualPlus.Structure;

    #endregion

    public class ThemeTypeConverter : ExpandableObjectConverter
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
                return new ObjectThemeWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Theme _theme;
            object result;

            result = null;
            _theme = value as Theme;

            if ((_theme != null) && (destinationType == typeof(string)))
            {
                // result = borderStyle.ToString();
                result = "Theme Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(ThemeTypeConverter))]
    public class ObjectThemeWrapper
    {
        #region Constructors

        public ObjectThemeWrapper()
        {
        }

        public ObjectThemeWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}