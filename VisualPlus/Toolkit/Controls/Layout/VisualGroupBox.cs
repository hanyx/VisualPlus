namespace VisualPlus.Toolkit.Controls.Layout
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization.Category;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GroupBox))]
    [DefaultEvent("Enter")]
    [DefaultProperty("Text")]
    [Description("The Visual GroupBox")]
    public class VisualGroupBox : NestedControlsBase
    {
        #region Variables

        private Border _border;
        private GroupBoxStyle groupBoxStyle;
        private StringAlignment stringAlignment;
        private TitleAlignments titleAlign;
        private Border titleBorder;
        private int titleBoxHeight;
        private GraphicsPath titleBoxPath;
        private Rectangle titleBoxRectangle;
        private bool titleBoxVisible;
        private Gradient titleGradient;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.Controls.Layout.VisualGroupBox" /> class.</summary>
        public VisualGroupBox()
        {
            groupBoxStyle = GroupBoxStyle.Default;
            stringAlignment = StringAlignment.Center;
            titleAlign = TitleAlignments.Top;
            titleBoxVisible = Settings.DefaultValue.TitleBoxVisible;
            titleBoxHeight = 25;

            Size = new Size(220, 180);
            titleBorder = new Border();
            _border = new Border();
            Padding = new Padding(5, titleBoxHeight + _border.Thickness, 5, 5);
            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        public enum GroupBoxStyle
        {
            /// <summary>The default.</summary>
            Default,

            /// <summary>The classic.</summary>
            Classic
        }

        public enum TitleAlignments
        {
            /// <summary>The bottom.</summary>
            Bottom,

            /// <summary>The top.</summary>
            Top
        }

        #endregion

        #region Properties

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

        [Category(Propertys.Appearance)]
        [Description(Property.Type)]
        public GroupBoxStyle BoxStyle
        {
            get
            {
                return groupBoxStyle;
            }

            set
            {
                groupBoxStyle = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Alignment)]
        public StringAlignment TextAlignment
        {
            get
            {
                return stringAlignment;
            }

            set
            {
                stringAlignment = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Alignment)]
        public TitleAlignments TitleAlignment
        {
            get
            {
                return titleAlign;
            }

            set
            {
                titleAlign = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public Border TitleBorder
        {
            get
            {
                return titleBorder;
            }

            set
            {
                titleBorder = value;
                Invalidate();
            }
        }

        [DefaultValue("25")]
        [Category(Propertys.Layout)]
        [Description(Property.Size)]
        public int TitleBoxHeight
        {
            get
            {
                return titleBoxHeight;
            }

            set
            {
                titleBoxHeight = value;
                Invalidate();
            }
        }

        [DefaultValue(Settings.DefaultValue.TitleBoxVisible)]
        [Category(Propertys.Behavior)]
        [Description(Property.Visible)]
        public bool TitleBoxVisible
        {
            get
            {
                return titleBoxVisible;
            }

            set
            {
                titleBoxVisible = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(GradientConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Propertys.Appearance)]
        public Gradient TitleGradient
        {
            get
            {
                return titleGradient;
            }

            set
            {
                titleGradient = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        public void UpdateTheme(Styles style)
        {
            StyleManager = new VisualStyleManager(style);
            _border.Color = StyleManager.BorderStyle.Color;
            _border.HoverColor = StyleManager.BorderStyle.HoverColor;
            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;

            BackColorState.Enabled = StyleManager.ControlStyle.Background(0);
            BackColorState.Disabled = StyleManager.ControlStyle.Background(0);

            titleGradient = StyleManager.ControlStatesStyle.ControlDisabled;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.Clear(Parent.BackColor);
            graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            ControlGraphicsPath = VisualBorderRenderer.GetBorderShape(ClientRectangle, _border.Type, _border.Rounding);
            graphics.CompositingQuality = CompositingQuality.GammaCorrected;

            Size textArea = GDI.MeasureText(graphics, Text, Font);
            Rectangle group = ConfigureStyleBox(textArea);
            Rectangle title = ConfigureStyleTitleBox(textArea);

            titleBoxRectangle = new Rectangle(title.X, title.Y, title.Width, title.Height);
            titleBoxPath = VisualBorderRenderer.GetBorderShape(titleBoxRectangle, titleBorder.Type, titleBorder.Rounding);

            Color _backColor = Enabled ? BackColorState.Enabled : BackColorState.Disabled;
            VisualBackgroundRenderer.DrawBackground(e.Graphics, ClientRectangle, _backColor, BackgroundImage, Border, MouseState);

            if (titleBoxVisible)
            {
                var gradientPoints = new[] { new Point { X = titleBoxRectangle.Width, Y = 0 }, new Point { X = titleBoxRectangle.Width, Y = titleBoxRectangle.Height } };
                LinearGradientBrush gradientBrush = Gradient.CreateGradientBrush(titleGradient.Colors, gradientPoints, titleGradient.Angle, titleGradient.Positions);

                graphics.FillPath(gradientBrush, titleBoxPath);

                if (titleBorder.Visible)
                {
                    if ((MouseState == MouseStates.Hover) && titleBorder.HoverVisible)
                    {
                        VisualBorderRenderer.DrawBorder(graphics, titleBoxPath, titleBorder.Thickness, titleBorder.HoverColor);
                    }
                    else
                    {
                        VisualBorderRenderer.DrawBorder(graphics, titleBoxPath, titleBorder.Thickness, titleBorder.Color);
                    }
                }
            }

            if (groupBoxStyle == GroupBoxStyle.Classic)
            {
                graphics.FillRectangle(new SolidBrush(BackColorState.Enabled), titleBoxRectangle);
                graphics.DrawString(Text, Font, new SolidBrush(ForeColor), titleBoxRectangle);
            }
            else
            {
                StringFormat stringFormat = new StringFormat
                    {
                        Alignment = stringAlignment,
                        LineAlignment = StringAlignment.Center
                    };

                graphics.DrawString(Text, Font, new SolidBrush(ForeColor), titleBoxRectangle, stringFormat);
            }
        }

        private Rectangle ConfigureStyleBox(Size textArea)
        {
            Size groupBoxSize = new Size(Width, Height);
            Point groupBoxPoint = new Point(0, 0);

            switch (groupBoxStyle)
            {
                case GroupBoxStyle.Default:
                    {
                        break;
                    }

                case GroupBoxStyle.Classic:
                    {
                        if (titleAlign == TitleAlignments.Top)
                        {
                            groupBoxPoint = new Point(0, textArea.Height / 2);
                            groupBoxSize = new Size(Width, Height - (textArea.Height / 2));
                        }
                        else
                        {
                            groupBoxPoint = new Point(0, 0);
                            groupBoxSize = new Size(Width, Height - (textArea.Height / 2));
                        }

                        break;
                    }
            }

            Rectangle groupBoxRectangle = new Rectangle(groupBoxPoint, groupBoxSize);

            return groupBoxRectangle;
        }

        private Rectangle ConfigureStyleTitleBox(Size textArea)
        {
            Size titleSize = new Size(Width, titleBoxHeight);
            Point titlePoint = new Point(0, 0);

            switch (groupBoxStyle)
            {
                case GroupBoxStyle.Default:
                    {
                        // Declare Y
                        if (titleAlign == TitleAlignments.Top)
                        {
                            titlePoint = new Point(titlePoint.X, 0);
                        }
                        else
                        {
                            titlePoint = new Point(titlePoint.X, Height - titleBoxHeight);
                        }

                        break;
                    }

                case GroupBoxStyle.Classic:
                    {
                        titleBoxVisible = false;
                        var spacing = 5;

                        if (titleAlign == TitleAlignments.Top)
                        {
                            titlePoint = new Point(titlePoint.X, 0);
                        }
                        else
                        {
                            titlePoint = new Point(titlePoint.X, Height - textArea.Height);
                        }

                        // +1 extra whitespace in case of FontStyle=Bold
                        titleSize = new Size(textArea.Width + 1, textArea.Height);

                        switch (stringAlignment)
                        {
                            case StringAlignment.Near:
                                {
                                    titlePoint.X += 5;
                                    break;
                                }

                            case StringAlignment.Center:
                                {
                                    titlePoint.X = (Width / 2) - (textArea.Width / 2);
                                    break;
                                }

                            case StringAlignment.Far:
                                {
                                    titlePoint.X = Width - textArea.Width - spacing;
                                    break;
                                }
                        }

                        break;
                    }
            }

            Rectangle titleRectangle = new Rectangle(titlePoint, titleSize);
            return titleRectangle;
        }

        #endregion
    }
}