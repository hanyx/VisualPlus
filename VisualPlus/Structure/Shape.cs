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
    using VisualPlus.Localization;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Managers;
    using VisualPlus.Toolkit.Components;

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
            StylesManager styleManager = new StylesManager(Settings.DefaultValue.DefaultStyle);
            ConstructShape(ShapeType.Rounded, styleManager.Theme.BorderSettings.Normal, Settings.DefaultValue.Rounding.Default, Settings.DefaultValue.BorderThickness, true);
        }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:VisualPlus.Structure.Shape" /> class.</summary>
        /// <param name="shapeType">The shape type.</param>
        /// <param name="color">The color.</param>
        /// <param name="rounding">The rounding.</param>
        public Shape(ShapeType shapeType, Color color, int rounding) : this()
        {
            ConstructShape(shapeType, color, rounding, _thickness, _visible);
        }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:VisualPlus.Structure.Shape" /> class.</summary>
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

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BorderColorChangedEventHandler ColorChanged;

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BorderRoundingChangedEventHandler RoundingChanged;

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BorderThicknessChangedEventHandler ThicknessChanged;

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BorderTypeChangedEventHandler TypeChanged;

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event BorderVisibleChangedEventHandler VisibleChanged;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Color)]
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
        [Description(PropertyDescription.Rounding)]
        public int Rounding
        {
            get
            {
                return _rounding;
            }

            set
            {
                if (_rounding == value)
                {
                    return;
                }

                _rounding = ExceptionManager.ArgumentOutOfRangeException(value, Settings.MinimumRounding, Settings.MaximumRounding, true);
                RoundingChanged?.Invoke();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Thickness)]
        public int Thickness
        {
            get
            {
                return _thickness;
            }

            set
            {
                if (_thickness == value)
                {
                    return;
                }

                _thickness = ExceptionManager.ArgumentOutOfRangeException(value, Settings.MinimumBorderSize, Settings.MaximumBorderSize, true);
                ThicknessChanged?.Invoke();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(PropertyDescription.Shape)]
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
            return (sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType);
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

            if ((shape != null) && (destinationType == typeof(string)))
            {
                // result = borderStyle.ToString();
                result = "Shape Settings";
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