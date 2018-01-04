namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization;
    using VisualPlus.Renders;
    using VisualPlus.Toolkit.Components;

    #endregion

    [Description("The check style structure.")]
    [TypeConverter(typeof(CheckStyleConverter))]
    public class CheckStyle
    {
        #region Variables

        private bool _autoSize;
        private Rectangle _bounds;
        private char _character;
        private Font _characterFont;
        private CheckType _checkType;
        private Color _color;
        private Image _image;
        private int _shapeRounding;
        private ShapeType _shapeType;
        private float _thickness;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="CheckStyle" /> class.</summary>
        /// <param name="boundary">The boundary.</param>
        public CheckStyle(Rectangle boundary)
        {
            StylesManager _styleManager = new StylesManager(Settings.DefaultValue.DefaultStyle);

            _color = _styleManager.Theme.OtherSettings.Progress;

            _autoSize = true;
            _character = '✔';
            _characterFont = _styleManager.Theme.TextSetting.Font;
            _checkType = CheckType.Character;

            _shapeRounding = Settings.DefaultValue.Rounding.BoxRounding;
            _shapeType = Settings.DefaultValue.BorderType;
            _thickness = 2.0F;

            Bitmap _bitmap = new Bitmap(Image.FromStream(new MemoryStream(Convert.FromBase64String(VisualToggleRenderer.GetBase64CheckImage()))));
            _image = _bitmap;
            _bounds = boundary;
        }

        public enum CheckType
        {
            /// <summary>The character.</summary>
            Character,

            /// <summary>The checkmark.</summary>
            Checkmark,

            /// <summary>The image.</summary>
            Image,

            /// <summary>The shape.</summary>
            Shape
        }

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.AutoSize)]
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
        [Description(PropertyDescription.Size)]
        public Rectangle Bounds
        {
            get
            {
                return _bounds;
            }

            set
            {
                _bounds = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Character)]
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
        [Description(PropertyDescription.Color)]
        public Color CheckColor
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Font)]
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
        [Description(PropertyDescription.Image)]
        public Image Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Rounding)]
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
        [Description(PropertyDescription.Type)]
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
        [Description(PropertyDescription.CheckType)]
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

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Thickness)]
        public float Thickness
        {
            get
            {
                return _thickness;
            }

            set
            {
                _thickness = value;
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