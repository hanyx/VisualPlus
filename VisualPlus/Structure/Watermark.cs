namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;

    using VisualPlus.Delegates;
    using VisualPlus.Localization;
    using VisualPlus.Localization.Category;
    using VisualPlus.Toolkit.Components;

    #endregion

    [Description("The watermark")]
    [TypeConverter(typeof(WatermarkConverter))]
    public class Watermark
    {
        #region Variables

        [Browsable(false)]
        public SolidBrush Brush;

        #endregion

        #region Variables

        private Color activeColor;
        private Font font;
        private Color inactiveColor;
        private string text;
        private bool visible;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Watermark" /> class.</summary>
        public Watermark()
        {
            StylesManager _styleManager = new StylesManager(Settings.DefaultValue.DefaultStyle);

            activeColor = _styleManager.Theme.OtherSettings.WatermarkActive;
            font = _styleManager.Theme.TextSetting.Font;
            inactiveColor = _styleManager.Theme.OtherSettings.WatermarkInactive;
            text = Settings.DefaultValue.WatermarkText;
            visible = Settings.DefaultValue.WatermarkVisible;

            Brush = new SolidBrush(inactiveColor);
        }

        [Category(Events.PropertyChanged)]
        [Description("Occours when the active color property has changed.")]
        public event WatermarkActiveColorChangedEventHandler ActiveColorChanged;

        [Category(Events.PropertyChanged)]
        [Description("Occours when the font property has changed.")]
        public event WatermarkFontChangedEventHandler FontChanged;

        [Category(Events.PropertyChanged)]
        [Description("Occours when the inactive property has changed.")]
        public event WatermarkInactiveColorChangedEventHandler InactiveColorChanged;

        [Category(Events.PropertyChanged)]
        [Description("Occours when the text property has changed.")]
        public event WatermarkTextChangedEventHandler TextChanged;

        [Category(Events.PropertyChanged)]
        [Description("Occours when the visible property has changed.")]
        public event WatermarkVisibleChangedEventHandler VisibleChanged;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Color)]
        public Color ActiveColor
        {
            get
            {
                return activeColor;
            }

            set
            {
                activeColor = value;
                ActiveColorChanged?.Invoke();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Font)]
        public Font Font
        {
            get
            {
                return font;
            }

            set
            {
                font = value;
                FontChanged?.Invoke();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Color)]
        public Color InactiveColor
        {
            get
            {
                return inactiveColor;
            }

            set
            {
                inactiveColor = value;
                InactiveColorChanged?.Invoke();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Text)]
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                TextChanged?.Invoke();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Visible)]
        public bool Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                VisibleChanged?.Invoke();
            }
        }

        #endregion

        #region Events

        public static void DrawWatermark(Graphics graphics, Rectangle textBoxRectangle, StringFormat stringFormat, Watermark watermark)
        {
            if (watermark.Visible)
            {
                graphics.DrawString(watermark.Text, watermark.Font, watermark.Brush, textBoxRectangle, stringFormat);
            }
        }

        #endregion
    }

    public class WatermarkConverter : ExpandableObjectConverter
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
                return new ObjectWatermarkWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Watermark watermark;
            object result;

            result = null;
            watermark = value as Watermark;

            if ((watermark != null) && (destinationType == typeof(string)))
            {
                result = "Watermark Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(WatermarkConverter))]
    public class ObjectWatermarkWrapper
    {
        #region Constructors

        public ObjectWatermarkWrapper()
        {
        }

        public ObjectWatermarkWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}