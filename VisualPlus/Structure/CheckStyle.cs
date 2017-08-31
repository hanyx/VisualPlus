namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Renders;
    using VisualPlus.Toolkit.Components;

    #endregion

    [Description("The check style structure.")]
    [TypeConverter(typeof(CheckStyleConverter))]
    public class CheckStyle
    {
        #region Variables

        private bool _autoSize;
        private char _character;
        private Font _characterFont;
        private CheckType _checkType;
        private int _shapeRounding;
        private ShapeType _shapeType;
        private Rectangle bounds;
        private Color color;
        private Image image;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="CheckStyle" /> class.</summary>
        /// <param name="boundary">The boundary.</param>
        public CheckStyle(Rectangle boundary)
        {
            VisualStyleManager _styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);

            color = _styleManager.CheckmarkStyle.EnabledGradient.Colors[0];

            _autoSize = true;
            _character = '✔';
            _characterFont = _styleManager.Font;
            _checkType = CheckType.Character;

            _shapeRounding = Settings.DefaultValue.Rounding.BoxRounding;
            _shapeType = Settings.DefaultValue.BorderType;

            Bitmap _bitmap = new Bitmap(Image.FromStream(new MemoryStream(Convert.FromBase64String(VisualToggleRenderer.GetBase64CheckImage()))));

            image = _bitmap;

            bounds = boundary;
        }

        public enum CheckType
        {
            /// <summary>The character.</summary>
            Character,

            /// <summary>The image.</summary>
            Image,

            /// <summary>The shape.</summary>
            Shape
        }

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.AutoSize)]
        public bool AutoSize
        {
            get
            {
                return _autoSize;
            }

            set
            {
                _autoSize = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Size)]
        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }

            set
            {
                bounds = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Character)]
        public char Character
        {
            get
            {
                return _character;
            }

            set
            {
                _character = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Color)]
        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Font)]
        public Font Font
        {
            get
            {
                return _characterFont;
            }

            set
            {
                _characterFont = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Image)]
        public Image Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Rounding)]
        public int ShapeRounding
        {
            get
            {
                return _shapeRounding;
            }

            set
            {
                _shapeRounding = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Type)]
        public ShapeType ShapeType
        {
            get
            {
                return _shapeType;
            }

            set
            {
                _shapeType = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.CheckType)]
        public CheckType Style
        {
            get
            {
                return _checkType;
            }

            set
            {
                _checkType = value;
            }
        }

        #endregion
    }

    public class CheckStyleConverter : ExpandableObjectConverter
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
                return new ObjectCheckMarkWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            CheckStyle checkStyle;
            object result;

            result = null;
            checkStyle = value as CheckStyle;

            if ((checkStyle != null) && (destinationType == typeof(string)))
            {
                result = "Check Style Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(CheckStyleConverter))]
    public class ObjectCheckMarkWrapper
    {
        #region Constructors

        public ObjectCheckMarkWrapper()
        {
        }

        public ObjectCheckMarkWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}