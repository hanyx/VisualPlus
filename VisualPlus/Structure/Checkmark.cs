namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;

    using VisualPlus.Enumerators;
    using VisualPlus.Extensibility;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Renders;
    using VisualPlus.Styles;
    using VisualPlus.Toolkit.Components;

    #endregion

    [Description("The checkmark.")]
    [TypeConverter(typeof(CheckMarkConverter))]
    public class Checkmark : ICheckmark
    {
        #region Variables

        private readonly StyleManager _styleManager = new StyleManager(Settings.DefaultValue.DefaultStyle);

        private bool autoSize;
        private char checkCharacter;
        private Font checkCharacterFont;
        private Point checkLocation;
        private CheckType checkType;
        private Gradient disabledGradient;
        private Bitmap disabledImage;
        private Gradient enabledGradient;
        private Bitmap enabledImage;
        private Size imageSize;
        private int shapeRounding;
        private Size shapeSize;
        private ShapeType shapeType;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Checkmark" /> class.</summary>
        /// <param name="boundary">The boundary.</param>
        public Checkmark(Rectangle boundary)
        {
            enabledGradient = _styleManager.CheckmarkStyle.EnabledGradient;
            disabledGradient = _styleManager.CheckmarkStyle.DisabledGradient;

            autoSize = true;
            checkCharacter = '✔';
            checkCharacterFont = _styleManager.Font;
            checkType = CheckType.Character;

            shapeRounding = Settings.DefaultValue.Rounding.BoxRounding;
            shapeType = Settings.DefaultValue.BorderType;

            Bitmap bitmap = new Bitmap(Image.FromStream(new MemoryStream(Convert.FromBase64String(VisualToggleRenderer.GetBase64CheckImage()))));

            disabledImage = bitmap.FilterGrayScale();
            enabledImage = bitmap;

            checkLocation = new Point();
            imageSize = boundary.Size;
            shapeSize = boundary.Size;
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
        [Description(Property.Description.Common.AutoSize)]
        public bool AutoSize
        {
            get
            {
                return autoSize;
            }

            set
            {
                autoSize = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Checkmark.Character)]
        public char Character
        {
            get
            {
                return checkCharacter;
            }

            set
            {
                checkCharacter = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.ColorGradient)]
        public Gradient DisabledGradient
        {
            get
            {
                return disabledGradient;
            }

            set
            {
                disabledGradient = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Image)]
        public Bitmap DisabledImage
        {
            get
            {
                return disabledImage;
            }

            set
            {
                disabledImage = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.ColorGradient)]
        public Gradient EnabledGradient
        {
            get
            {
                return enabledGradient;
            }

            set
            {
                enabledGradient = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Image)]
        public Bitmap EnabledImage
        {
            get
            {
                return enabledImage;
            }

            set
            {
                enabledImage = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Strings.Font)]
        public Font Font
        {
            get
            {
                return checkCharacterFont;
            }

            set
            {
                checkCharacterFont = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Size)]
        public Size ImageSize
        {
            get
            {
                return imageSize;
            }

            set
            {
                imageSize = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Point)]
        public Point Location
        {
            get
            {
                return checkLocation;
            }

            set
            {
                checkLocation = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Border.Rounding)]
        public int ShapeRounding
        {
            get
            {
                return shapeRounding;
            }

            set
            {
                shapeRounding = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Size)]
        public Size ShapeSize
        {
            get
            {
                return shapeSize;
            }

            set
            {
                shapeSize = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Type)]
        public ShapeType ShapeType
        {
            get
            {
                return shapeType;
            }

            set
            {
                shapeType = value;
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Checkmark.CheckType)]
        public CheckType Style
        {
            get
            {
                return checkType;
            }

            set
            {
                checkType = value;
            }
        }

        #endregion
    }

    public class CheckMarkConverter : ExpandableObjectConverter
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
            Checkmark checkmark;
            object result;

            result = null;
            checkmark = value as Checkmark;

            if ((checkmark != null) && (destinationType == typeof(string)))
            {
                result = "CheckMark Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(CheckMarkConverter))]
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