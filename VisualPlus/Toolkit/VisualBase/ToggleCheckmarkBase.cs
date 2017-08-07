namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Extensibility;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public abstract class ToggleCheckmarkBase : ToggleBase, IAnimate, IControlStates
    {
        #region Variables

        private bool animation;
        private Rectangle box;
        private int boxSpacing = 2;
        private Checkmark checkMark;
        private Point mouseLocation;
        private VFXManager rippleEffectsManager;

        #endregion

        #region Constructors

        protected ToggleCheckmarkBase()
        {
            AutoSize = true;
            box = new Rectangle(0, 0, 14, 14);
            animation = Settings.DefaultValue.Animation;
            checkMark = new Checkmark(ClientRectangle);
            ConfigureAnimation();
            UpdateTheme(this, Settings.DefaultValue.DefaultStyle);
        }

        #endregion

        #region Properties
        [DefaultValue(false)]
        [Category(Localize.PropertiesCategory.Behavior)]
        [Description(Localize.Description.Checkmark.Checked)]
        public bool Checked
        {
            get
            {
                return Toggle;
            }

            set
            {
                if (Toggle != value)
                {
                    // Store new values
                    Toggle = value;

                    // Generate events
                    OnToggleChanged(new ToggleEventArgs(Toggle));

                    // Repaint
                    Invalidate();
                }
            }
        }

        [DefaultValue(Settings.DefaultValue.Animation)]
        [Category(Localize.PropertiesCategory.Behavior)]
        [Description(Localize.Description.Common.Animation)]
        public bool Animation
        {
            get
            {
                return animation;
            }

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

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Localize.PropertiesCategory.Appearance)]
        public Border Border
        {
            get
            {
                return ControlBorder;
            }

            set
            {
                ControlBorder = value;
                Invalidate();
            }
        }

        [Description(Localize.Description.Common.Size)]
        [Category(Localize.PropertiesCategory.Layout)]
        public Size Box
        {
            get
            {
                return box.Size;
            }

            set
            {
                box.Size = value;
                if (AutoSize)
                {
                    ConfigSize(Text.MeasureText(Font));
                }

                Invalidate();
            }
        }

        [Category(Localize.PropertiesCategory.Layout)]
        [Description(Localize.Description.Common.Spacing)]
        public int BoxSpacing
        {
            get
            {
                return boxSpacing;
            }

            set
            {
                boxSpacing = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(CheckMarkConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Localize.PropertiesCategory.Appearance)]
        public Checkmark CheckMark
        {
            get
            {
                return checkMark;
            }

            set
            {
                checkMark = value;
                Invalidate();
            }
        }

        [Description(Localize.Description.Common.ColorGradient)]
        [Category(Localize.PropertiesCategory.Appearance)]
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

        [Description(Localize.Description.Common.ColorGradient)]
        [Category(Localize.PropertiesCategory.Appearance)]
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

        [Description(Localize.Description.Common.ColorGradient)]
        [Category(Localize.PropertiesCategory.Appearance)]
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

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsBoxLarger { get; set; }

        [Description(Localize.Description.Common.ColorGradient)]
        [Category(Localize.PropertiesCategory.Appearance)]
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

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Size TextSize { get; set; }

        #endregion

        #region Events

        public void ConfigureAnimation()
        {
            VFXManager effectsManager = new VFXManager
                {
                    Increment = 0.05,
                    EffectType = EffectType.EaseInOut
                };
            rippleEffectsManager = new VFXManager(false)
                {
                    Increment = 0.10,
                    SecondaryIncrement = 0.08,
                    EffectType = EffectType.Linear
                };

            effectsManager.OnAnimationProgress += sender => Invalidate();
            rippleEffectsManager.OnAnimationProgress += sender => Invalidate();
            effectsManager.StartNewAnimation(Toggle ? AnimationDirection.In : AnimationDirection.Out);
        }

        public void DrawAnimation(Graphics graphics)
        {
            if (animation && rippleEffectsManager.IsAnimating())
            {
                for (var i = 0; i < rippleEffectsManager.GetAnimationCount(); i++)
                {
                    double animationValue = rippleEffectsManager.GetProgress(i);

                    Point animationSource = new Point(box.X + (box.Width / 2), box.Y + (box.Height / 2));
                    SolidBrush animationBrush = new SolidBrush(Color.FromArgb((int)(animationValue * 40), (bool)rippleEffectsManager.GetData(i)[0] ? Color.Black : checkMark.EnabledGradient.Colors[0]));

                    int height = box.Height;
                    int size = rippleEffectsManager.GetDirection(i) == AnimationDirection.InOutIn ? (int)(height * (0.8d + (0.2d * animationValue))) : height;

                    GraphicsPath path = GDI.DrawRoundedRectangle(animationSource.X - (size / 2), animationSource.Y - (size / 2), size, size, size / 2);
                    graphics.FillPath(animationBrush, path);
                    animationBrush.Dispose();
                }
            }
        }

        /// <summary>Returns the size of the check box glyph.</summary>
        /// <returns>The size of the check box glyph.</returns>
        public Size GetGlyphSize()
        {
            return box.Size;
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
                };
            MouseLeave += (sender, args) =>
                {
                    mouseLocation = new Point(-1, -1);
                    MouseState = MouseStates.Normal;
                };
            MouseDown += (sender, args) =>
                {
                    MouseState = MouseStates.Down;

                    if (animation && (args.Button == MouseButtons.Left) && GDI.IsMouseInBounds(mouseLocation, box))
                    {
                        rippleEffectsManager.SecondaryIncrement = 0;
                        rippleEffectsManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { Toggle });
                    }
                };
            MouseUp += (sender, args) =>
                {
                    MouseState = MouseStates.Hover;
                    rippleEffectsManager.SecondaryIncrement = 0.08;
                };
            MouseMove += (sender, args) =>
                {
                    mouseLocation = args.Location;
                    Cursor = GDI.IsMouseInBounds(mouseLocation, box) ? Cursors.Hand : Cursors.Default;
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

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (AutoSize)
            {
                box = new Rectangle(new Point(Padding.Left, (ClientRectangle.Height / 2) - (box.Height / 2)), box.Size);
                ConfigSize(Text.MeasureText(Font));
            }
            else
            {
                box = new Rectangle(new Point(Padding.Left, (ClientRectangle.Height / 2) - (box.Height / 2)), box.Size);
            }

            Graphics graphics = e.Graphics;
            graphics.Clear(Parent.BackColor);
            e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, Width + 1, Height + 1));

            LinearGradientBrush _boxBrush = GDI.GetControlBrush(graphics, Enabled, MouseState, ControlBrushCollection, ClientRectangle);
            Size textSize = GDI.MeasureText(graphics, Text, Font);
            TextSize = Text.MeasureText(Font);
            Point textPoint = new Point(box.Right + boxSpacing, (ClientRectangle.Height / 2) - (textSize.Height / 2));

            VisualToggleRenderer.DrawCheckBox(graphics, Border, CheckMark, box, Toggle, Enabled, _boxBrush, MouseState, Text, Font, ForeColor, textPoint);
            DrawAnimation(graphics);
        }

        protected override void OnThemeChanged(ThemeEventArgs e)
        {
            checkMark.EnabledGradient = StyleManager.CheckmarkStyle.EnabledGradient;
            checkMark.DisabledGradient = StyleManager.CheckmarkStyle.DisabledGradient;

            base.OnThemeChanged(new ThemeEventArgs(this, e.Style));
        }

        private void ConfigSize(Size textSize)
        {
            if (GDI.TextLargerThanRectangle(textSize, box))
            {
                IsBoxLarger = false;
                Size = new Size(box.X + box.Width + boxSpacing + textSize.Width, textSize.Height);
            }
            else
            {
                IsBoxLarger = true;
                Size = new Size(box.X + box.Width + boxSpacing + textSize.Width, box.Height);
            }
        }

        #endregion
    }
}