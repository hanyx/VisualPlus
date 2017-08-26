#region Namespace

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VisualPlus.Enumerators;
using VisualPlus.Localization.Category;
using VisualPlus.Localization.Descriptions;
using VisualPlus.Managers;
using VisualPlus.Renders;
using VisualPlus.Structure;
using VisualPlus.Toolkit.Components;
using VisualPlus.Toolkit.VisualBase;

#endregion

namespace VisualPlus.Toolkit.Controls.Interactivity
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Control))]
    [DefaultEvent("Click")]
    [DefaultProperty("ShapeForm")]
    [Description("The Visual Shape")]
    public class VisualShape : VisualControlBase
    {
        #region Variables

        private Gradient _background;
        private Border _border;
        private bool animation;
        private GraphicsPath controlGraphicsPath;
        private VFXManager effectsManager;
        private VFXManager hoverEffectsManager;
        private ShapeType shapeType;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualShape" /> class.</summary>
        public VisualShape()
        {
            shapeType = ShapeType.Rectangle;
            BackColor = Color.Transparent;
            Size = new Size(100, 100);

            animation = Settings.DefaultValue.Animation;

            _border = new Border();
            ConfigureAnimation();

            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        public enum ShapeType
        {
            /// <summary>The circle.</summary>
            Circle,

            /// <summary>The rectangle.</summary>
            Rectangle,

            /// <summary>The triangle.</summary>
            Triangle
        }

        #endregion

        #region Properties

        [DefaultValue(Settings.DefaultValue.Animation)]
        [Category(Propertys.Behavior)]
        [Description(Property.Animation)]
        public bool Animation
        {
            get { return animation; }

            set
            {
                animation = value;

                AutoSize = AutoSize; // Make AutoSize directly set the bounds.

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
            }
        }

        [TypeConverter(typeof(GradientConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public Gradient BackgroundGradient
        {
            get { return _background; }

            set
            {
                _background = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public Border Border
        {
            get { return _border; }

            set
            {
                _border = value;
                Invalidate();
            }
        }

        [Category(Propertys.Behavior)]
        [Description("The type of shape.")]
        public ShapeType ShapeForm
        {
            get { return shapeType; }

            set
            {
                shapeType = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        public void UpdateTheme(Enumerators.Styles style)
        {
            StyleManager = new VisualStyleManager(style);

            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;

            Background = StyleManager.ControlStyle.Background(0);
            BackgroundDisabled = StyleManager.ControlStyle.Background(0);
            _background = StyleManager.ControlStatesStyle.ControlEnabled;

            ControlBrushCollection = new[]
            {
                StyleManager.ControlStatesStyle.ControlEnabled,
                StyleManager.ControlStatesStyle.ControlHover,
                StyleManager.ControlStatesStyle.ControlPressed,
                StyleManager.ControlStatesStyle.ControlDisabled
            };
            _border.Color = StyleManager.BorderStyle.Color;
            _border.HoverColor = StyleManager.BorderStyle.HoverColor;
            Invalidate();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode)
            {
                return;
            }

            MouseState = MouseStates.Normal;
            MouseEnter += (sender, args) =>
            {
                MouseState = MouseStates.Hover;
                hoverEffectsManager.StartNewAnimation(AnimationDirection.In);
                Invalidate();
            };
            MouseLeave += (sender, args) =>
            {
                MouseState = MouseStates.Normal;
                hoverEffectsManager.StartNewAnimation(AnimationDirection.Out);
                Invalidate();
            };
            MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    MouseState = MouseStates.Down;
                    effectsManager.StartNewAnimation(AnimationDirection.In, args.Location);
                    Invalidate();
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseStates.Hover;
                Invalidate();
            };
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            MouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            ConfigureComponents(graphics);

            DrawBackground(graphics);
            DrawAnimation(graphics);
        }

        private void ConfigureAnimation()
        {
            effectsManager = new VFXManager(false)
            {
                Increment = 0.03,
                EffectType = EffectType.EaseOut
            };

            hoverEffectsManager = new VFXManager
            {
                Increment = 0.07,
                EffectType = EffectType.Linear
            };

            hoverEffectsManager.OnAnimationProgress += sender => Invalidate();
            effectsManager.OnAnimationProgress += sender => Invalidate();
        }

        private void ConfigureComponents(Graphics graphics)
        {
            var gradientPoints = new[] {new Point {X = ClientRectangle.Width, Y = 0}, new Point {X = ClientRectangle.Width, Y = ClientRectangle.Height}};
            LinearGradientBrush gradientBrush = Gradient.CreateGradientBrush(_background.Colors, gradientPoints, _background.Angle, _background.Positions);
            controlGraphicsPath = new GraphicsPath();

            switch (shapeType)
            {
                case ShapeType.Circle:
                {
                    Rectangle circleRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

                    graphics.FillEllipse(gradientBrush, circleRectangle);
                    controlGraphicsPath.AddEllipse(circleRectangle);

                    break;
                }

                case ShapeType.Rectangle:
                {
                    controlGraphicsPath = VisualBorderRenderer.GetBorderShape(ClientRectangle, _border.Type, _border.Rounding);
                    graphics.FillPath(gradientBrush, controlGraphicsPath);

                    break;
                }

                case ShapeType.Triangle:
                {
                    Rectangle triangleRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                    var points = new Point[3];

                    points[0].X = triangleRectangle.X + (triangleRectangle.Width / 2);
                    points[0].Y = triangleRectangle.Y;

                    points[1].X = triangleRectangle.X;
                    points[1].Y = triangleRectangle.Y + triangleRectangle.Height;

                    points[2].X = triangleRectangle.X + triangleRectangle.Width;
                    points[2].Y = triangleRectangle.Y + triangleRectangle.Height;

                    graphics.FillPolygon(gradientBrush, points);

                    controlGraphicsPath.AddPolygon(points);
                    break;
                }
            }

            VisualBorderRenderer.DrawBorderStyle(graphics, _border, MouseState, controlGraphicsPath);
        }

        private void DrawAnimation(Graphics graphics)
        {
            if (effectsManager.IsAnimating() && animation)
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                for (var i = 0; i < effectsManager.GetAnimationCount(); i++)
                {
                    double animationValue = effectsManager.GetProgress(i);
                    Point animationSource = effectsManager.GetSource(i);

                    using (Brush rippleBrush = new SolidBrush(Color.FromArgb((int) (101 - (animationValue * 100)), Color.Black)))
                    {
                        var rippleSize = (int) (animationValue * Width * 2);
                        graphics.SetClip(controlGraphicsPath);
                        graphics.FillEllipse(rippleBrush, new Rectangle(animationSource.X - (rippleSize / 2), animationSource.Y - (rippleSize / 2), rippleSize, rippleSize));
                    }
                }

                graphics.SmoothingMode = SmoothingMode.None;
            }
        }

        private void DrawBackground(Graphics graphics)
        {
        }

        #endregion

        // TODO: Add rotation
    }
}