namespace VisualPlus.Toolkit.Controls.Layout
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
        private BorderEdge _borderEdge;
        private GroupBoxStyle _boxStyle;
        private StringAlignment _stringAlignment;
        private TitleAlignments _titleAlignment;
        private int _titleBoxHeight;
        private Rectangle _titleBoxRectangle;
        private bool _titleBoxVisible;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:VisualPlus.Toolkit.Controls.Layout.VisualGroupBox" /> class.</summary>
        public VisualGroupBox()
        {
            _boxStyle = GroupBoxStyle.Default;
            _stringAlignment = StringAlignment.Center;
            _titleAlignment = TitleAlignments.Top;
            _titleBoxVisible = Settings.DefaultValue.TitleBoxVisible;
            _titleBoxHeight = 25;
            _borderEdge = new BorderEdge();

            Size = new Size(220, 180);
            _border = new Border();
            Padding = new Padding(5, _titleBoxHeight + _border.Thickness, 5, 5);

            Controls.Add(_borderEdge);

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
                return _boxStyle;
            }

            set
            {
                _boxStyle = value;

                if (_boxStyle == GroupBoxStyle.Classic)
                {
                    _borderEdge.Visible = false;
                }
                else
                {
                    _borderEdge.Visible = true;
                }

                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public bool Separator
        {
            get
            {
                return _borderEdge.Visible;
            }

            set
            {
                _borderEdge.Visible = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Color)]
        public Color SeparatorColor
        {
            get
            {
                return _borderEdge.BackColor;
            }

            set
            {
                _borderEdge.BackColor = value;
                Invalidate();
            }
        }

        [Category(Propertys.Appearance)]
        [Description(Property.Alignment)]
        public StringAlignment TextAlignment
        {
            get
            {
                return _stringAlignment;
            }

            set
            {
                _stringAlignment = value;
                Invalidate();
            }
        }

        [Category(Propertys.Layout)]
        [Description(Property.Alignment)]
        public TitleAlignments TitleAlignment
        {
            get
            {
                return _titleAlignment;
            }

            set
            {
                _titleAlignment = value;
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
                return _titleBoxHeight;
            }

            set
            {
                _titleBoxHeight = value;
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
                return _titleBoxVisible;
            }

            set
            {
                _titleBoxVisible = value;
                Invalidate();
            }
        }

        #endregion

        #region Events

        public void UpdateTheme(Styles style)
        {
            StyleManager = new VisualStyleManager(style);
            _border.Color = StyleManager.ShapeStyle.Color;
            _border.HoverColor = StyleManager.BorderStyle.HoverColor;
            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;

            BackColorState.Enabled = StyleManager.ControlStyle.Background(0);
            BackColorState.Disabled = StyleManager.ControlStyle.Background(0);

            _borderEdge.BackColor = StyleManager.ControlStyle.Line;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.GammaCorrected;

            Size textArea = GDI.MeasureText(graphics, Text, Font);
            Rectangle group = ConfigureStyleBox(textArea);
            Rectangle title = ConfigureStyleTitleBox(textArea);

            _titleBoxRectangle = new Rectangle(title.X, title.Y, title.Width - 1, title.Height);

            Rectangle _clientRectangle = new Rectangle(group.X, group.Y, group.Width, group.Height);
            ControlGraphicsPath = VisualBorderRenderer.CreateBorderTypePath(_clientRectangle, _border);
            graphics.FillRectangle(new SolidBrush(BackColor), _clientRectangle);

            Color _backColor = Enabled ? BackColorState.Enabled : BackColorState.Disabled;
            VisualBackgroundRenderer.DrawBackground(e.Graphics, _backColor, BackgroundImage, MouseState, group, Border);

            if (_borderEdge.Visible)
            {
                switch (TitleAlignment)
                {
                    case TitleAlignments.Bottom:
                        {
                            _borderEdge.Location = new Point(_titleBoxRectangle.X + _border.Thickness, _titleBoxRectangle.Y);
                            _borderEdge.Size = new Size(Width - _border.Thickness - 1, 1);
                            break;
                        }

                    case TitleAlignments.Top:
                        {
                            _borderEdge.Location = new Point(_titleBoxRectangle.X + _border.Thickness, _titleBoxRectangle.Bottom);
                            _borderEdge.Size = new Size(Width - _border.Thickness - 1, 1);
                            break;
                        }

                    default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                }
            }

            VisualBorderRenderer.DrawBorderStyle(e.Graphics, _border, ControlGraphicsPath, MouseState);

            if (_boxStyle == GroupBoxStyle.Classic)
            {
                graphics.FillRectangle(new SolidBrush(BackColorState.Enabled), _titleBoxRectangle);
                graphics.DrawString(Text, Font, new SolidBrush(ForeColor), _titleBoxRectangle);
            }
            else
            {
                StringFormat stringFormat = new StringFormat
                    {
                        Alignment = _stringAlignment,
                        LineAlignment = StringAlignment.Center
                    };

                graphics.DrawString(Text, Font, new SolidBrush(ForeColor), _titleBoxRectangle, stringFormat);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(BackColor);
        }

        private Rectangle ConfigureStyleBox(Size textArea)
        {
            Size groupBoxSize;
            Point groupBoxPoint = new Point(0, 0);

            switch (_boxStyle)
            {
                case GroupBoxStyle.Default:
                    {
                        groupBoxSize = new Size(ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                        break;
                    }

                case GroupBoxStyle.Classic:
                    {
                        if (_titleAlignment == TitleAlignments.Top)
                        {
                            groupBoxPoint = new Point(0, textArea.Height / 2);
                            groupBoxSize = new Size(Width - _border.Thickness, Height - (textArea.Height / 2) - _border.Thickness);
                        }
                        else
                        {
                            groupBoxPoint = new Point(0, 0);
                            groupBoxSize = new Size(Width - _border.Thickness, Height - (textArea.Height / 2));
                        }

                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Rectangle groupBoxRectangle = new Rectangle(groupBoxPoint, groupBoxSize);

            return groupBoxRectangle;
        }

        private Rectangle ConfigureStyleTitleBox(Size textArea)
        {
            Size titleSize = new Size(Width, _titleBoxHeight);
            Point titlePoint = new Point(0, 0);

            switch (_boxStyle)
            {
                case GroupBoxStyle.Default:
                    {
                        // Declare Y
                        if (_titleAlignment == TitleAlignments.Top)
                        {
                            titlePoint = new Point(titlePoint.X, 0);
                        }
                        else
                        {
                            titlePoint = new Point(titlePoint.X, Height - _titleBoxHeight);
                        }

                        break;
                    }

                case GroupBoxStyle.Classic:
                    {
                        _titleBoxVisible = false;
                        var spacing = 5;

                        if (_titleAlignment == TitleAlignments.Top)
                        {
                            titlePoint = new Point(titlePoint.X, 0);
                        }
                        else
                        {
                            titlePoint = new Point(titlePoint.X, Height - textArea.Height);
                        }

                        // +1 extra whitespace in case of FontStyle=Bold
                        titleSize = new Size(textArea.Width + 2, textArea.Height);

                        switch (_stringAlignment)
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

        // private Color _titleColor;
    }
}