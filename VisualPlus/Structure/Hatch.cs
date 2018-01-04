namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Runtime.InteropServices;

    using VisualPlus.Localization;
    using VisualPlus.Toolkit.Components;

    #endregion

    [TypeConverter(typeof(HatchConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The Hatch structure.")]
    public class Hatch
    {
        #region Variables

        private Color _backColor;
        private Color _foreColor;
        private Size _size;
        private HatchStyle _style;
        private bool _visible;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Hatch" /> class.</summary>
        public Hatch()
        {
            StylesManager _styleManager = new StylesManager(Settings.DefaultValue.DefaultStyle);
            _visible = Settings.DefaultValue.HatchVisible;
            _size = Settings.DefaultValue.HatchSize;
            _style = Settings.DefaultValue.HatchStyle;
            _backColor = _styleManager.Theme.OtherSettings.HatchBackColor;
            _foreColor = Color.FromArgb(40, _styleManager.Theme.OtherSettings.HatchForeColor);
        }

        /// <summary>Initializes a new instance of the <see cref="Hatch" /> class.</summary>
        /// <param name="visible">The visiblity of the hatch.</param>
        /// <param name="size">The size of the hatch.</param>
        /// <param name="style">The style of the hatch.</param>
        /// <param name="backColor">The back Color.</param>
        /// <param name="foreColor">The fore Color.</param>
        public Hatch(bool visible, Size size, HatchStyle style, Color backColor, Color foreColor)
        {
            _visible = visible;
            _size = size;
            _style = style;
            _backColor = backColor;
            _foreColor = foreColor;
        }

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Color)]
        public Color BackColor
        {
            get
            {
                return _backColor;
            }

            set
            {
                _backColor = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Color)]
        public Color ForeColor
        {
            get
            {
                return _foreColor;
            }

            set
            {
                _foreColor = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Size)]
        public Size Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.HatchStyle)]
        public HatchStyle Style
        {
            get
            {
                return _style;
            }

            set
            {
                _style = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Visible)]
        public bool Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                _visible = value;
            }
        }

        #endregion
    }

    public class HatchConverter : ExpandableObjectConverter
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
                return new ObjectHatchWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Hatch _hatch;
            object result;

            result = null;
            _hatch = value as Hatch;

            if ((_hatch != null) && (destinationType == typeof(string)))
            {
                result = "Hatch Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(HatchConverter))]
    public class ObjectHatchWrapper
    {
        #region Constructors

        public ObjectHatchWrapper()
        {
        }

        public ObjectHatchWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}