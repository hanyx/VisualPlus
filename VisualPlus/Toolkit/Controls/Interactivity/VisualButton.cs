namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Managers;
    using VisualPlus.Properties;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Button))]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Description("The Visual Button")]
    [Designer(ControlManager.FilterProperties.VisualButton)]
    public class VisualButton : VisualControlBase, IAnimationSupport, IThemeSupport
    {
        #region Variables

        private bool _animation;

        private ControlColorState _backColorState;
        private Border _border;
        private VFXManager _effectsManager;
        private VFXManager _hoverEffectsManager;
        private TextImageRelation _textImageRelation;
        private VisualBitmap _visualBitmap;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.Controls.Interactivity.VisualButton" />
        ///     class.
        /// </summary>
        public VisualButton()
        {
            Size = new Size(140, 45);
            _animation = Settings.DefaultValue.Animation;
            _border = new Border();
            _textImageRelation = TextImageRelation.Overlay;
            _backColorState = new ControlColorState();
            _visualBitmap = new VisualBitmap(Resources.Icon, new Size(24, 24))
                {
                    Visible = false,
                    Image = Resources.Icon
                };
            _visualBitmap.Point = new Point(0, (Height / 2) - (_visualBitmap.Size.Height / 2));

            ConfigureAnimation();
            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        #endregion

        #region Properties

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

        [TypeConverter(typeof(ControlColorStateConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlColorState BackColorState
        {
            get
            {
                return _backColorState;
            }

            set
            {
                if (value == _backColorState)
                {
                    return;
                }

                _backColorState = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
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

        [TypeConverter(typeof(VisualBitmapConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
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

        [Category(Propertys.Behavior)]
        [Description(Property.TextImageRelation)]
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
            StyleManager.Style = style;
            _border.Color = StyleManager.BorderStyle.Color;
            _border.HoverColor = StyleManager.BorderStyle.HoverColor;
            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;
            Font = StyleManager.Font;

            _backColorState.Enabled = StyleManager.ControlStyle.Background(0);
            _backColorState.Disabled = Color.FromArgb(224, 224, 224);
            _backColorState.Hover = Color.FromArgb(224, 224, 224);
            _backColorState.Pressed = Color.Silver;

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
            Graphics _graphics = e.Graphics;
            _graphics.Clear(Parent.BackColor);
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.TextRenderingHint = TextRenderingHint;
            ControlGraphicsPath = VisualBorderRenderer.GetBorderShape(ClientRectangle, _border);
            Rectangle _clientRectangle = new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, ClientRectangle.Width + 1, ClientRectangle.Height + 1);
            _graphics.FillRectangle(new SolidBrush(BackColor), _clientRectangle);

            Color _backColor = GDI.GetBackColorState(Enabled, BackColorState.Enabled, BackColorState.Hover, BackColorState.Pressed, BackColorState.Disabled, MouseState);
            VisualBackgroundRenderer.DrawBackground(e.Graphics, ClientRectangle, _backColor, BackgroundImage, Border, MouseState);

            Color _textColor = Enabled ? ForeColor : ForeColorDisabled;

            VisualControlRenderer.DrawInternalContent(e.Graphics, ClientRectangle, Text, Font, _textColor, Image, _textImageRelation);
            DrawAnimation(e.Graphics);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(BackColor);
        }

        #endregion
    }
}