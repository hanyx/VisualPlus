namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;

    using VisualPlus.Delegates;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization.Category;
    using VisualPlus.Managers;
    using VisualPlus.Toolkit.Components;

    using Property = VisualPlus.Localization.Descriptions.Property;

    #endregion

    [TypeConverter(typeof(ShapeConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The shape.")]
    public class Shape
    {
        #region Variables

        private Color _color;
        private int _rounding;
        private ShapeType _shapeType;
        private int _thickness;
        private bool _visible;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Shape" /> class.</summary>
        public Shape()
        {
            VisualStyleManager styleManager = new VisualStyleManager(Settings.DefaultValue.DefaultStyle);
            ConstructShape(ShapeType.Rounded, styleManager.BorderStyle.Color, Settings.DefaultValue.Rounding.Default, Settings.DefaultValue.BorderThickness, true);
        }

        /// <summary>Initializes a new instance of the <see cref="Shape" /> class.</summary>
        /// <param name="shapeType">The shape type.</param>
        /// <param name="color">The color.</param>
        /// <param name="rounding">The rounding.</param>
        public Shape(ShapeType shapeType, Color color, int rounding) : this()
        {
            ConstructShape(shapeType, color, rounding, _thickness, _visible);
        }

        /// <summary>Initializes a new instance of the <see cref="Shape" /> class.</summary>
        /// <param name="shapeType">The shape type.</param>
        /// <param name="color">The color.</param>
        /// <param name="rounding">The rounding.</param>
        /// <param name="thickness">The thickness.</param>
        public Shape(ShapeType shapeType, Color color, int rounding, int thickness) : this()
        {
            ConstructShape(shapeType, color, rounding, thickness, _visible);
        }

        /// <summary>Initializes a new instance of the <see cref="Shape" /> class.</summary>
        /// <param name="shapeType">The shape type.</param>
        /// <param name="color">The color.</param>
        /// <param name="rounding">The rounding.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="visible">The visibility.</param>
        public Shape(ShapeType shapeType, Color color, int rounding, int thickness, bool visible)
        {
            ConstructShape(shapeType, color, rounding, thickness, visible);
        }

        [Category(Event.PropertyChanged)]
        [Description(Localization.Descriptions.Event.PropertyEventChanged)]
        public event BorderColorChangedEventHandler ColorChanged;

        [Category(Event.PropertyChanged)]
        [Description(Localization.Descriptions.Event.PropertyEventChanged)]
        public event BorderRoundingChangedEventHandler RoundingChanged;

        [Category(Event.PropertyChanged)]
        [Description(Localization.Descriptions.Event.PropertyEventChanged)]
        public event BorderThicknessChangedEventHandler ThicknessChanged;

        [Category(Event.PropertyChanged)]
        [Description(Localization.Descriptions.Event.PropertyEventChanged)]
        public event BorderTypeChangedEventHandler TypeChanged;

        [Category(Event.PropertyChanged)]
        [Description(Localization.Descriptions.Event.PropertyEventChanged)]
        public event BorderVisibleChangedEventHandler VisibleChanged;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Color)]
        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                ColorChanged?.Invoke(new ColorEventArgs(_color));
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Border.Rounding)]
        public int Rounding
        {
            get
            {
                return _rounding;
            }

            set
            {
                if (ExceptionManager.ArgumentOutOfRangeException(value, Settings.MinimumRounding, Settings.MaximumRounding))
                {
                    _rounding = value;
                    RoundingChanged?.Invoke();
                }
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Border.Thickness)]
        public int Thickness
        {
            get
            {
                return _thickness;
            }

            set
            {
                if (ExceptionManager.ArgumentOutOfRangeException(value, Settings.MinimumBorderSize, Settings.MaximumBorderSize))
                {
                    _thickness = value;
                    ThicknessChanged?.Invoke();
                }
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Border.Shape)]
        public ShapeType Type
        {
            get
            {
                return _shapeType;
            }

            set
            {
                _shapeType = value;
                TypeChanged?.Invoke();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Description.Common.Visible)]
        public bool Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                _visible = value;
                VisibleChanged?.Invoke();
            }
        }

        #endregion

        #region Events

        /// <summary>Constructs the shape.</summary>
        /// <param name="shapeType">The shape type.</param>
        /// <param name="color">The color.</param>
        /// <param name="rounding">The rounding.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="visible">The visibility.</param>
        private void ConstructShape(ShapeType shapeType, Color color, int rounding, int thickness, bool visible)
        {
            _color = color;
            _rounding = rounding;
            _thickness = thickness;
            _shapeType = shapeType;
            _visible = visible;
        }

        #endregion
    }

    public class ShapeConverter : ExpandableObjectConverter
    {
        #region Events

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;

            if (stringValue != null)
            {
                return new ObjectBorderShapeWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Shape shape;
            object result;

            result = null;
            shape = value as Shape;

            if (shape != null && destinationType == typeof(string))
            {
                // result = borderStyle.ToString();
                result = "Border Shape Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(ShapeConverter))]
    public class ObjectBorderShapeWrapper
    {
        #region Constructors

        public ObjectBorderShapeWrapper()
        {
        }

        public ObjectBorderShapeWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}