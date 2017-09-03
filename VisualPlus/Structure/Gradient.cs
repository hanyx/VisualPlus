namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Runtime.InteropServices;

    using VisualPlus.Delegates;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;

    #endregion

    [TypeConverter(typeof(GradientConverter))]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The Gradient structure.")]
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
            CreateGradient(0, _defaultColors, _defaultPosition);
        }

        /// <summary>Initializes a new instance of the <see cref="Gradient" /> class.</summary>
        /// <param name="colors">The colors.</param>
        /// <param name="positions">The positions.</param>
        public Gradient(Color[] colors, float[] positions)
        {
            CreateGradient(0, colors, positions);
        }

        /// <summary>Initializes a new instance of the <see cref="Gradient" /> class.</summary>
        /// <param name="angle">The angle.</param>
        /// <param name="colors">The colors.</param>
        /// <param name="positions">The positions.</param>
        public Gradient(float angle, Color[] colors, float[] positions)
        {
            CreateGradient(angle, colors, positions);
        }

        /// <summary>Initializes a new instance of the <see cref="Gradient" /> class.</summary>
        /// <param name="angle">The angle.</param>
        /// <param name="colors">The colors.</param>
        /// <param name="positions">The positions.</param>
        /// <param name="rectangle">The rectangle.</param>
        public Gradient(float angle, Color[] colors, float[] positions, Rectangle rectangle)
        {
            CreateGradient(angle, colors, positions);
            GradientBrush = CreateGradientBrush(this, rectangle);
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
                OnAngleChanged();
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
                OnColorsChanged();
            }
        }

        [Description("The brush that can be used to paint the gradient.")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public LinearGradientBrush GradientBrush { get; set; }

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
                OnPositionsChanged();
            }
        }

        #endregion

        #region Events

        /// <summary>Creates a gradient brush.</summary>
        /// <param name="gradient">The gradient.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>Returns a custom gradient brush.</returns>
        public static LinearGradientBrush CreateGradientBrush(Gradient gradient, Rectangle rectangle)
        {
            return CreateGradientBrush(gradient.Angle, gradient.Colors, gradient.Positions, rectangle);
        }

        /// <summary>Creates a gradient brush.</summary>
        /// <param name="angle">The angle.</param>
        /// <param name="colors">The colors.</param>
        /// <param name="positions">The positions.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>Returns a custom gradient brush.</returns>
        public static LinearGradientBrush CreateGradientBrush(float angle, Color[] colors, float[] positions, Rectangle rectangle)
        {
            var _points = GetGradientPoints(rectangle);
            LinearGradientBrush _linearGradientBrush = new LinearGradientBrush(_points[0], _points[1], Color.Black, Color.Black);

            ColorBlend _colorBlend = new ColorBlend
                {
                    Positions = positions,
                    Colors = colors
                };

            _linearGradientBrush.InterpolationColors = _colorBlend;
            _linearGradientBrush.RotateTransform(angle);

            return _linearGradientBrush;
        }

        protected virtual void OnAngleChanged()
        {
            AngleChanged?.Invoke();
        }

        protected virtual void OnColorsChanged()
        {
            ColorsChanged?.Invoke();
        }

        protected virtual void OnPositionsChanged()
        {
            PositionsChanged?.Invoke();
        }

        /// <summary>Gets the gradients points from the rectangle.</summary>
        /// <param name="rectangle">Rectangle points to set.</param>
        /// <returns>Gradient points.</returns>
        private static Point[] GetGradientPoints(Rectangle rectangle)
        {
            return new[] { new Point { X = rectangle.Width, Y = 0 }, new Point { X = rectangle.Width, Y = rectangle.Height } };
        }

        /// <summary>Creates the gradient.</summary>
        /// <param name="angle">The angle.</param>
        /// <param name="colors">The colors.</param>
        /// <param name="positions">The positions.</param>
        private void CreateGradient(float angle, Color[] colors, float[] positions)
        {
            if (colors.Length != positions.Length)
            {
                throw new Exception("You must have an equal amount of colors that you have positions.");
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