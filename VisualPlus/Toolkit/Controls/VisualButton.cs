namespace VisualPlus.Toolkit.Controls
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization.Category;
    using VisualPlus.Managers;
    using VisualPlus.Properties;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Button))]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Description("The Visual Button")]
    [Designer(ControlManager.FilterProperties.VisualButton)]
    public class VisualButton : VisualControlBase, IAnimate, IControlStates
    {
        #region Variables

        private bool _animation;

        private Border _border;
        private VFXManager _effectsManager;
        private VFXManager _hoverEffectsManager;
        private TextImageRelation _textImageRelation;
        private VisualBitmap _visualBitmap;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualButton"/> class.</summary>
        public VisualButton()
        {
            Size = new Size(140, 45);
            _animation = Settings.DefaultValue.Animation;
            ColorGradientToggle = true;

            _visualBitmap = new VisualBitmap(Resources.Icon, new Size(24, 24))
                {
                    Visible = false,
                    Image = Resources.Icon
                };

            _border = new Border();
            _visualBitmap.Point = new Point(0, (Height / 2) - (_visualBitmap.Size.Height / 2));

            _textImageRelation = TextImageRelation.Overlay;

            ConfigureAnimation();
            UpdateTheme(StyleManager.Style);
        }

        #endregion

        #region Properties

        [DefaultValue(Settings.DefaultValue.Animation)]
        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.Description.Common.Animation)]
        public bool Animation
        {
            get
            {
                return _animation;
            }

            set
            {
                _animation = value;
                AutoSize = AutoSize; // Make AutoSize directly set the bounds.

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Appearance)]
        public Border Border
        {
            get
            {
                return _border;
            }

            set
            {
                _border = value;
                Invalidate();
            }
        }

        [Description(Localization.Descriptions.Property.Description.Common.ColorGradient)]
        [Category(Property.Appearance)]
        public Gradient DisabledGradient
        {
            get
            {
                return ControlBrushCollection[3];
            }

            set
            {
                ControlBrushCollection[3] = value;
            }
        }

        [Description(Localization.Descriptions.Property.Description.Common.ColorGradient)]
        [Category(Property.Appearance)]
        public Gradient EnabledGradient
        {
            get
            {
                return ControlBrushCollection[0];
            }

            set
            {
                ControlBrushCollection[0] = value;
            }
        }

        [DefaultValue(true)]
        [Category(Property.Behavior)]
        [Description("Gets or sets the color gradient toggle.")]
        public bool GradientToggle
        {
            get
            {
                return ColorGradientToggle;
            }

            set
            {
                ColorGradientToggle = value;
                Invalidate();
            }
        }

        [Description(Localization.Descriptions.Property.Description.Common.ColorGradient)]
        [Category(Property.Appearance)]
        public Gradient HoverGradient
        {
            get
            {
                return ControlBrushCollection[1];
            }

            set
            {
                ControlBrushCollection[1] = value;
            }
        }

        [TypeConverter(typeof(VisualBitmapConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Appearance)]
        public VisualBitmap Image
        {
            get
            {
                return _visualBitmap;
            }

            set
            {
                _visualBitmap = value;
                Invalidate();
            }
        }

        [Description(Localization.Descriptions.Property.Description.Common.ColorGradient)]
        [Category(Property.Appearance)]
        public Gradient PressedGradient
        {
            get
            {
                return ControlBrushCollection[2];
            }

            set
            {
                ControlBrushCollection[2] = value;
            }
        }

        [Category(Property.Behavior)]
        [Description(Localization.Descriptions.Property.Description.Common.TextImageRelation)]
        public TextImageRelation TextImageRelation
        {
            get
            {
                return _textImageRelation;
            }

            set
            {
                _textImageRelation = value;
                Invalidate();
            }
        }

        internal bool ColorGradientToggle { get; set; }

        #endregion

        #region Events

        public void ConfigureAnimation()
        {
            _effectsManager = new VFXManager(false)
                {
                    Increment = 0.03,
                    EffectType = EffectType.EaseOut
                };
            _hoverEffectsManager = new VFXManager
                {
                    Increment = 0.07,
                    EffectType = EffectType.Linear
                };

            _hoverEffectsManager.OnAnimationProgress += sender => Invalidate();
            _effectsManager.OnAnimationProgress += sender => Invalidate();
        }

        public void DrawAnimation(Graphics graphics)
        {
            if (_effectsManager.IsAnimating() && _animation)
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                for (var i = 0; i < _effectsManager.GetAnimationCount(); i++)
                {
                    double animationValue = _effectsManager.GetProgress(i);
                    Point animationSource = _effectsManager.GetSource(i);

                    using (Brush rippleBrush = new SolidBrush(Color.FromArgb((int)(101 - (animationValue * 100)), Color.Black)))
                    {
                        var rippleSize = (int)(animationValue * Width * 2);
                        graphics.SetClip(ControlGraphicsPath);
                        graphics.FillEllipse(rippleBrush, new Rectangle(animationSource.X - (rippleSize / 2), animationSource.Y - (rippleSize / 2), rippleSize, rippleSize));
                    }
                }

                graphics.SmoothingMode = SmoothingMode.None;
            }
        }

        public void UpdateTheme(Styles style)
        {
            StyleManager = new VisualStyleManager(style);
            _border.Color = StyleManager.BorderStyle.Color;
            _border.HoverColor = StyleManager.BorderStyle.HoverColor;
            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;

            Background = StyleManager.ControlStyle.Background(0);
            BackgroundDisabled = StyleManager.ControlStyle.Background(0);

            ControlBrushCollection = new[]
                {
                    StyleManager.ControlStatesStyle.ControlEnabled,
                    StyleManager.ControlStatesStyle.ControlHover,
                    StyleManager.ControlStatesStyle.ControlPressed,
                    StyleManager.ControlStatesStyle.ControlDisabled
                };

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
                    _hoverEffectsManager.StartNewAnimation(AnimationDirection.In);
                    Invalidate();
                };
            MouseLeave += (sender, args) =>
                {
                    MouseState = MouseStates.Normal;
                    _hoverEffectsManager.StartNewAnimation(AnimationDirection.Out);
                    Invalidate();
                };
            MouseDown += (sender, args) =>
                {
                    if (args.Button == MouseButtons.Left)
                    {
                        MouseState = MouseStates.Down;
                        _effectsManager.StartNewAnimation(AnimationDirection.In, args.Location);
                        Invalidate();
                    }
                };
            MouseUp += (sender, args) =>
                {
                    MouseState = MouseStates.Hover;
                    Invalidate();
                };
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MouseState = MouseStates.Down;
            Invalidate();
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseState = MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlGraphicsPath = VisualBorderRenderer.GetBorderShape(ClientRectangle, _border);
            BackgroundStateGradientBrush = GDI.GetControlBrush(e.Graphics, Enabled, MouseState, ControlBrushCollection, ClientRectangle);
            VisualControlRenderer.DrawButton(e.Graphics, ClientRectangle, Text, Font, ForeColor, Image, _border, _textImageRelation, BackgroundStateColor, BackgroundStateGradientBrush, ColorGradientToggle, MouseState);
            DrawAnimation(e.Graphics);
        }

        #endregion
    }
}