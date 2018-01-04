namespace VisualPlus.Toolkit.Controls.DataVisualization
{
    #region Namespace

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.Localization;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("Enabled")]
    [Description("The Visual Progress Indicator")]
    [Designer(typeof(VisualProgressIndicatorDesigner))]
    [ToolboxBitmap(typeof(VisualProgressIndicator), "Resources.ToolboxBitmaps.VisualProgressIndicator.bmp")]
    [ToolboxItem(true)]
    public class VisualProgressIndicator : VisualStyleBase
    {
        #region Variables

        private SolidBrush animationColor = new SolidBrush(Color.DimGray);

        private Timer animationSpeed = new Timer();
        private SolidBrush baseColor = new SolidBrush(Color.DarkGray);
        private BufferedGraphics buffGraphics;
        private float circles = 45F;

        private Size circleSize = new Size(15, 15);
        private float diameter = 7.5F;
        private PointF[] floatPoint;
        private BufferedGraphicsContext graphicsContext = BufferedGraphicsManager.Current;
        private int indicatorIndex;
        private double rise;
        private double run;
        private PointF startingFloatPoint;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualProgressIndicator" /> class.</summary>
        public VisualProgressIndicator()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(80, 80);
            MinimumSize = new Size(0, 0);
            SetPoints();
            animationSpeed.Interval = 100;
            UpdateStyles();
        }

        #endregion

        #region Properties

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color AnimationColor
        {
            get
            {
                return animationColor.Color;
            }

            set
            {
                animationColor.Color = value;
            }
        }

        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.AnimationSpeed)]
        public int AnimationSpeed
        {
            get
            {
                return animationSpeed.Interval;
            }

            set
            {
                animationSpeed.Interval = value;
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
        public Color BaseColor
        {
            get
            {
                return baseColor.Color;
            }

            set
            {
                baseColor.Color = value;
            }
        }

        [DefaultValue(45F)]
        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Amount)]
        public float Circles
        {
            get
            {
                return circles;
            }

            set
            {
                circles = value;
                SetPoints();
                Invalidate();
            }
        }

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Size)]
        public Size CircleSize
        {
            get
            {
                return circleSize;
            }

            set
            {
                circleSize = value;
                Invalidate();
            }
        }

        [DefaultValue(7.5F)]
        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Diameter)]
        public float Diameter
        {
            get
            {
                return diameter;
            }

            set
            {
                diameter = value;
                SetPoints();
                Invalidate();
            }
        }

        private PointF EndPoint
        {
            get
            {
                float locationX = Convert.ToSingle(startingFloatPoint.Y + rise);
                float locationY = Convert.ToSingle(startingFloatPoint.X + run);

                return new PointF(locationY, locationX);
            }
        }

        #endregion

        #region Events

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            animationSpeed.Enabled = Enabled;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            animationSpeed.Tick += AnimationSpeedTick;
            animationSpeed.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.CompositingQuality = CompositingQuality.GammaCorrected;

            buffGraphics.Graphics.Clear(BackColor);
            int num2 = floatPoint.Length - 1;
            for (var i = 0; i <= num2; i++)
            {
                if (indicatorIndex == i)
                {
                    // Current circle
                    buffGraphics.Graphics.FillEllipse(animationColor, floatPoint[i].X, floatPoint[i].Y, circleSize.Width, circleSize.Height);
                }
                else
                {
                    // Other circles
                    buffGraphics.Graphics.FillEllipse(baseColor, floatPoint[i].X, floatPoint[i].Y, circleSize.Width, circleSize.Height);
                }
            }

            buffGraphics.Render(e.Graphics);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetStandardSize();
            UpdateGraphics();
            SetPoints();
        }

        private static X AssignValues<X>(ref X run, X length)
        {
            run = length;
            return length;
        }

        private void AnimationSpeedTick(object sender, EventArgs e)
        {
            if (indicatorIndex.Equals(0))
            {
                indicatorIndex = floatPoint.Length - 1;
            }
            else
            {
                indicatorIndex -= 1;
            }

            Invalidate(false);
        }

        private void SetPoints()
        {
            var stack = new Stack<PointF>();
            startingFloatPoint = new PointF(Width / 2f, Height / 2f);
            for (var i = 0f; i < 360f; i += circles)
            {
                SetValue(startingFloatPoint, (int)Math.Round((Width / 2.0) - 15.0), i);
                PointF endPoint = EndPoint;
                endPoint = new PointF(endPoint.X - diameter, endPoint.Y - diameter);
                stack.Push(endPoint);
            }

            floatPoint = stack.ToArray();
        }

        private void SetStandardSize()
        {
            int size = Math.Max(Width, Height);
            Size = new Size(size, size);
        }

        private void SetValue(PointF startFloatPoint, int length, double angle)
        {
            double circleRadian = (Math.PI * angle) / 180.0;

            startingFloatPoint = startFloatPoint;
            rise = AssignValues(ref run, length);
            rise = Math.Sin(circleRadian) * rise;
            run = Math.Cos(circleRadian) * run;
        }

        private void UpdateGraphics()
        {
            if ((Width <= 0) || (Height <= 0))
            {
                return;
            }

            Size bufferSize = new Size(Width + 1, Height + 1);
            graphicsContext.MaximumBuffer = bufferSize;
            buffGraphics = graphicsContext.Allocate(CreateGraphics(), ClientRectangle);
            buffGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        #endregion
    }
}