namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;

    using VisualPlus.Delegates;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;

    #endregion

    [Description("The gradient.")]
    [TypeConverter(typeof(GradientConverter))]
    public class Gradient
    {
        #region Variables

        private float _angle;
        private Color[] _colors;
        private float[] _positions;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Gradient" /> class.</summary>
        public Gradient()
        {
            var _defaultColors = new[]
                {
                    Color.Red,
                    Color.Green,
                    Color.Blue
                };

            var _defaultPosition = new[] { 0, 1 / 2f, 1 };

            ConstructGradient(_defaultColors, _defaultPosition, 0);
        }

        /// <summary>Initializes a new instance of the <see cref="Gradient" /> class.</summary>
        /// <param name="colors">The colors.</param>
        /// <param name="positions">The positions.</param>
        public Gradient(Color[] colors, float[] positions)
        {
            ConstructGradient(colors, positions, 0);
        }

        /// <summary>Initializes a new instance of the <see cref="Gradient" /> class.</summary>
        /// <param name="colors">The colors.</param>
        /// <param name="positions">The positions.</param>
        /// <param name="angle">The angle.</param>
        public Gradient(Color[] colors, float[] positions, float angle)
        {
            ConstructGradient(colors, positions, angle);
        }

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event GradientAngleChangedEventHandler AngleChanged;

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event GradientColorChangedEventHandler ColorsChanged;

        [Category(Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event GradientPositionsChangedEventHandler PositionsChanged;

        #endregion

        #region Properties

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Angle)]
        public float Angle
        {
            get
            {
                return _angle;
            }

            set
            {
                _angle = value;
                AngleChanged?.Invoke();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Colors)]
        public Color[] Colors
        {
            get
            {
                return _colors;
            }

            set
            {
                _colors = value;
                ColorsChanged?.Invoke();
            }
        }

        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description(Property.Positions)]
        public float[] Positions
        {
            get
            {
                return _positions;
            }

            set
            {
                _positions = value;
                PositionsChanged?.Invoke();
            }
        }

        #endregion

        #region Events

        /// <summary>Creates a gradient brush.</summary>
        /// <param name="colors">The colors.</param>
        /// <param name="points">The points.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="positions">The positions.</param>
        /// <returns>Returns a custom gradient brush.</returns>
        public static LinearGradientBrush CreateGradientBrush(Color[] colors, Point[] points, float angle, float[] positions)
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(points[0], points[1], Color.Black, Color.Black);

            ColorBlend colorBlend = new ColorBlend
                {
                    Positions = positions,
                    Colors = colors
                };

            linearGradientBrush.InterpolationColors = colorBlend;
            linearGradientBrush.RotateTransform(angle);

            return linearGradientBrush;
        }

        /// <summary>Constructs the gradient.</summary>
        /// <param name="colors">The colors.</param>
        /// <param name="positions">The positions.</param>
        /// <param name="angle">The angle.</param>
        private void ConstructGradient(Color[] colors, float[] positions, float angle)
        {
            if (colors.Length != positions.Length)
            {
                throw new Exception("You must have an equal amount of colors that you have positions for them.");
            }

            _colors = colors;
            _positions = positions;
            _angle = angle;
        }

        #endregion
    }

    public class GradientConverter : ExpandableObjectConverter
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
                return new ObjectGradientWrapper(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Gradient gradient;
            object result;

            result = null;
            gradient = value as Gradient;

            if ((gradient != null) && (destinationType == typeof(string)))
            {
                // result = borderStyle.ToString();
                result = "Gradient Settings";
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    [TypeConverter(typeof(GradientConverter))]
    public class ObjectGradientWrapper
    {
        #region Constructors

        public ObjectGradientWrapper()
        {
        }

        public ObjectGradientWrapper(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion
    }
}