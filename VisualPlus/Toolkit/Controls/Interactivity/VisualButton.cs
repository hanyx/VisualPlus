namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Designer;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Description("The Visual Button")]
    [Designer(typeof(VisualButtonDesigner))]
    [ToolboxBitmap(typeof(VisualButton), "Resources.ToolboxBitmaps.VisualButton.bmp")]
    [ToolboxItem(true)]
    public class VisualButton : VisualStyleBase, IAnimationSupport, IThemeSupport
    {
        #region Variables

        private bool _animation;
        private ControlColorState _backColorState;
        private Border _border;
        private VFXManager _effectsManager;
        private VFXManager _hoverEffectsManager;
        private Image _image;
        private StringAlignment _textAlignment;
        private TextImageRelation _textImageRelation;
        private StringAlignment _textLineAlignment;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualButton" /> class.</summary>
        public VisualButton()
        {
            Size = new Size(140, 45);
            _border = new Border();
            _backColorState = new ControlColorState();
            _animation = Settings.DefaultValue.Animation;
            _textImageRelation = TextImageRelation.Overlay;
            _textAlignment = StringAlignment.Center;
            _textLineAlignment = StringAlignment.Center;
            ConfigureAnimation(new[] { 0.03, 0.07 }, new[] { EffectType.EaseOut, EffectType.EaseInOut });

            UpdateTheme(ThemeManager.Theme);
        }

        #endregion

        #region Properties

        [DefaultValue(Settings.DefaultValue.Animation)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Animation)]
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
        [Category(PropertyCategory.Appearance)]
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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Image)]
        public Image Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Alignment)]
        public StringAlignment TextAlignment
        {
            get
            {
                return _textAlignment;
            }

            set
            {
                _textAlignment = value;
                Invalidate();
            }
        }

        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.TextImageRelation)]
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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Alignment)]
        public StringAlignment TextLineAlignment
        {
            get
            {
                return _textLineAlignment;
            }

            set
            {
                _textLineAlignment = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        public void ConfigureAnimation(double[] effectIncrement, EffectType[] effectType)
        {
            _effectsManager = new VFXManager(false)
                {
                    Increment = effectIncrement[0],
                    EffectType = effectType[0]
                };

            _hoverEffectsManager = new VFXManager
                {
                    Increment = effectIncrement[1],
                    EffectType = effectType[1]
                };

            _hoverEffectsManager.OnAnimationProgress += sender => Invalidate();
            _effectsManager.OnAnimationProgress += sender => Invalidate();
        }

        public void DrawAnimation(Graphics graphics)
        {
            if (!_effectsManager.IsAnimating() || !_animation)
            {
                return;
            }

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            for (var i = 0; i < _effectsManager.GetAnimationCount(); i++)
            {
                double _value = _effectsManager.GetProgress(i);
                Point _source = _effectsManager.GetSource(i);

                using (Brush _rippleBrush = new SolidBrush(Color.FromArgb((int)(101 - (_value * 100)), Color.Black)))
                {
                    var _rippleSize = (int)(_value * Width * 2);
                    graphics.SetClip(ControlGraphicsPath);
                    graphics.FillEllipse(_rippleBrush, new Rectangle(_source.X - (_rippleSize / 2), _source.Y - (_rippleSize / 2), _rippleSize, _rippleSize));
                }
            }

            graphics.SmoothingMode = SmoothingMode.None;
        }

        public void UpdateTheme(Theme theme)
        {
            try
            {
                _border.Color = theme.BorderSettings.Normal;
                _border.HoverColor = theme.BorderSettings.Hover;

                ForeColor = theme.TextSetting.Enabled;
                TextStyle.Enabled = theme.TextSetting.Enabled;
                TextStyle.Disabled = theme.TextSetting.Disabled;

                Font = theme.TextSetting.Font;

                _backColorState.Enabled = theme.ColorStateSettings.Enabled;
                _backColorState.Disabled = theme.ColorStateSettings.Disabled;
                _backColorState.Hover = theme.ColorStateSettings.Hover;
                _backColorState.Pressed = theme.ColorStateSettings.Pressed;
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }

            Invalidate();
            OnThemeChanged(new ThemeEventArgs(theme));
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

            try
            {
                Graphics _graphics = e.Graphics;
                _graphics.Clear(Parent.BackColor);
                _graphics.SmoothingMode = SmoothingMode.HighQuality;
                _graphics.TextRenderingHint = TextStyle.TextRenderingHint;
                Rectangle _clientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                ControlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);
                _graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, ClientRectangle.Width + 1, ClientRectangle.Height + 1));

                Color _backColor = ControlColorState.BackColorState(BackColorState, Enabled, MouseState);

                e.Graphics.SetClip(ControlGraphicsPath);
                VisualBackgroundRenderer.DrawBackground(e.Graphics, _backColor, BackgroundImage, MouseState, _clientRectangle, _border);

                Color _textColor = Enabled ? ForeColor : TextStyle.Disabled;

                if (_image != null)
                {
                    VisualControlRenderer.DrawContent(e.Graphics, ClientRectangle, Text, Font, _textColor, _image, _image.Size, _textImageRelation);
                }
                else
                {
                    VisualControlRenderer.DrawContentText(e.Graphics, ClientRectangle, Text, Font, _textColor, _textAlignment, _textLineAlignment);
                }

                VisualBorderRenderer.DrawBorderStyle(e.Graphics, _border, ControlGraphicsPath, MouseState);
                DrawAnimation(e.Graphics);
                e.Graphics.ResetClip();
            }
            catch (Exception exception)
            {
                VisualExceptionDialog.Show(exception);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(BackColor);
        }

        #endregion
    }
}