namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.Enumerators;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Managers;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Description("The Visual Label")]
    [Designer(typeof(VisualLabelDesigner))]
    [ToolboxBitmap(typeof(VisualLabel), "Resources.ToolboxBitmaps.VisualLabel.bmp")]
    [ToolboxItem(true)]
    public class VisualLabel : VisualControlBase
    {
        #region Variables

        private Orientation _orientation;
        private bool _outline;
        private Color _outlineColor;
        private Point _outlineLocation;
        private bool _reflection;
        private Color _reflectionColor;
        private int _reflectionSpacing;
        private bool _shadow;
        private Color _shadowColor;
        private int _shadowDepth;
        private int _shadowDirection;
        private Point _shadowLocation;
        private float _shadowSmooth;
        private int shadowOpacity;
        private Rectangle textBoxRectangle;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.Controls.Interactivity.VisualLabel" />
        ///     class.
        /// </summary>
        public VisualLabel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

            UpdateStyles();
            _orientation = Orientation.Horizontal;
            _outlineColor = Color.Red;
            _outlineLocation = new Point(0, 0);
            _reflectionColor = Color.FromArgb(120, 0, 0, 0);
            _shadowColor = Color.Black;
            _shadowDepth = 4;
            _shadowDirection = 315;
            _shadowLocation = new Point(0, 0);
            shadowOpacity = 100;
            _shadowSmooth = 1.5f;

            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        #endregion

        #region Properties

        [Category(Propertys.Appearance)]
        [Description(Property.Orientation)]
        public Orientation Orientation
        {
            get
            {
                return _orientation;
            }

            set
            {
                _orientation = value;
                Size = GraphicsManager.FlipOrientationSize(_orientation, Size);
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Outline)]
        public bool Outline
        {
            get
            {
                return _outline;
            }

            set
            {
                _outline = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color OutlineColor
        {
            get
            {
                return _outlineColor;
            }

            set
            {
                _outlineColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Point)]
        public Point OutlineLocation
        {
            get
            {
                return _outlineLocation;
            }

            set
            {
                _outlineLocation = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        [Category(Propertys.Behavior)]
        [Description(Property.Toggle)]
        public bool Reflection
        {
            get
            {
                return _reflection;
            }

            set
            {
                _reflection = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ReflectionColor
        {
            get
            {
                return _reflectionColor;
            }

            set
            {
                _reflectionColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Spacing)]
        public int ReflectionSpacing
        {
            get
            {
                return _reflectionSpacing;
            }

            set
            {
                _reflectionSpacing = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        [Category(Propertys.Appearance)]
        [Description(Property.Toggle)]
        public bool Shadow
        {
            get
            {
                return _shadow;
            }

            set
            {
                _shadow = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color ShadowColor
        {
            get
            {
                return _shadowColor;
            }

            set
            {
                _shadowColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Direction)]
        public int ShadowDirection
        {
            get
            {
                return _shadowDirection;
            }

            set
            {
                _shadowDirection = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Point)]
        public Point ShadowLocation
        {
            get
            {
                return _shadowLocation;
            }

            set
            {
                _shadowLocation = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Opacity)]
        public int ShadowOpacity
        {
            get
            {
                return shadowOpacity;
            }

            set
            {
                if (shadowOpacity == value)
                {
                    return;
                }

                shadowOpacity = ExceptionManager.ArgumentOutOfRangeException(value, Settings.MinimumAlpha, Settings.MaximumAlpha, true);
                Invalidate();
            }
        }

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        public void UpdateTheme(Styles style)
        {
            StyleManager = new VisualStyleManager(style);

            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;
            Font = StyleManager.Font;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.Clear(Parent.BackColor);
            graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

            Color _foreColor = Enabled ? ForeColor : ForeColorDisabled;

            if (_reflection && (_orientation == Orientation.Vertical))
            {
                textBoxRectangle = new Rectangle(GraphicsManager.MeasureText(graphics, Text, Font).Height, 0, ClientRectangle.Width, ClientRectangle.Height);
            }
            else
            {
                textBoxRectangle = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            }

            // Draw the text outline
            if (_outline)
            {
                DrawOutline(graphics);
            }

            // Draw the shadow
            if (_shadow)
            {
                DrawShadow(graphics);
            }

            // Draw the reflection text.
            if (_reflection)
            {
                DrawReflection(graphics);
            }

            graphics.DrawString(Text, Font, new SolidBrush(_foreColor), textBoxRectangle, GetStringFormat());
        }

        private void DrawOutline(Graphics graphics)
        {
            GraphicsPath outlinePath = new GraphicsPath();

            switch (_orientation)
            {
                case Orientation.Horizontal:
                    {
                        outlinePath.AddString(
                            Text,
                            Font.FontFamily,
                            (int)Font.Style,
                            (graphics.DpiY * Font.SizeInPoints) / 72,
                            _outlineLocation,
                            new StringFormat());

                        break;
                    }

                case Orientation.Vertical:
                    {
                        outlinePath.AddString(
                            Text,
                            Font.FontFamily,
                            (int)Font.Style,
                            (graphics.DpiY * Font.SizeInPoints) / 72,
                            _outlineLocation,
                            new StringFormat(StringFormatFlags.DirectionVertical));

                        break;
                    }
            }

            graphics.DrawPath(new Pen(OutlineColor), outlinePath);
        }

        private void DrawReflection(Graphics graphics)
        {
            Point reflectionLocation = new Point(0, 0);
            Bitmap reflectionBitmap = new Bitmap(Width, Height);
            Graphics imageGraphics = Graphics.FromImage(reflectionBitmap);

            // Setup text render
            imageGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            // Rotate reflection
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        imageGraphics.TranslateTransform(0, GraphicsManager.MeasureText(graphics, Text, Font).Height);
                        imageGraphics.ScaleTransform(1, -1);

                        reflectionLocation = new Point(0, textBoxRectangle.Y - (GraphicsManager.MeasureText(graphics, Text, Font).Height / 2) - _reflectionSpacing);
                        break;
                    }

                case Orientation.Vertical:
                    {
                        imageGraphics.ScaleTransform(-1, 1);
                        reflectionLocation = new Point((textBoxRectangle.X - (GraphicsManager.MeasureText(graphics, Text, Font).Width / 2)) + _reflectionSpacing, 0);
                        break;
                    }
            }

            // Draw reflected string
            imageGraphics.DrawString(Text, Font, new SolidBrush(_reflectionColor), reflectionLocation, GetStringFormat());

            // Draw the reflection image
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(reflectionBitmap, ClientRectangle, 0, 0, reflectionBitmap.Width, reflectionBitmap.Height, GraphicsUnit.Pixel);
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        private void DrawShadow(Graphics graphics)
        {
            // Create shadow into a bitmap
            Bitmap shadowBitmap = new Bitmap(Math.Max((int)(Width / _shadowSmooth), 1), Math.Max((int)(Height / _shadowSmooth), 1));
            Graphics imageGraphics = Graphics.FromImage(shadowBitmap);

            // Setup text render
            imageGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            // Create transformation matrix
            Matrix transformMatrix = new Matrix();
            transformMatrix.Scale(1 / _shadowSmooth, 1 / _shadowSmooth);
            transformMatrix.Translate((float)(_shadowDepth * Math.Cos(_shadowDirection)), (float)(_shadowDepth * Math.Sin(_shadowDirection)));
            imageGraphics.Transform = transformMatrix;

            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        imageGraphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(shadowOpacity, _shadowColor)), _shadowLocation);
                        break;
                    }

                case Orientation.Vertical:
                    {
                        imageGraphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(shadowOpacity, _shadowColor)), _shadowLocation, new StringFormat(StringFormatFlags.DirectionVertical));
                        break;
                    }
            }

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(shadowBitmap, ClientRectangle, 0, 0, shadowBitmap.Width, shadowBitmap.Height, GraphicsUnit.Pixel);
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        private StringFormat GetStringFormat()
        {
            StringFormat stringFormat = new StringFormat();

            switch (_orientation)
            {
                case Orientation.Horizontal:
                    {
                        stringFormat = new StringFormat();
                        break;
                    }

                case Orientation.Vertical:
                    {
                        stringFormat = new StringFormat(StringFormatFlags.DirectionVertical);
                        break;
                    }
            }

            return stringFormat;
        }

        #endregion
    }
}