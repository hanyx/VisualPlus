﻿namespace VisualPlus.Toolkit.Controls.Layout
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    using VisualPlus.EventArgs;
    using VisualPlus.Localization;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GroupBox))]
    [DefaultEvent("Enter")]
    [DefaultProperty("Text")]
    [Description("The Visual GroupBox")]
    public class VisualGroupBox : NestedControlsBase, IThemeSupport
    {
        #region Variables

        private Border _border;
        private BorderEdge _borderEdge;
        private GroupBoxStyle _boxStyle;
        private Image _image;
        private StringAlignment _stringAlignment;
        private StringAlignment _textAlignment;
        private TextImageRelation _textImageRelation;
        private StringAlignment _textLineAlignment;
        private int _titleBoxHeight;
        private Rectangle _titleBoxRectangle;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualGroupBox" /> class.</summary>
        public VisualGroupBox()
        {
            _boxStyle = GroupBoxStyle.Default;
            _stringAlignment = StringAlignment.Center;
            _titleBoxHeight = 25;
            _borderEdge = new BorderEdge();
            _textImageRelation = TextImageRelation.ImageBeforeText;
            _textAlignment = StringAlignment.Center;
            _textLineAlignment = StringAlignment.Center;
            Size = new Size(220, 180);
            _border = new Border();
            Padding = new Padding(5, _titleBoxHeight + _border.Thickness, 5, 5);
            Controls.Add(_borderEdge);

            UpdateTheme(ThemeManager.Theme);
        }

        public enum GroupBoxStyle
        {
            /// <summary>The default.</summary>
            Default,

            /// <summary>The classic.</summary>
            Classic
        }

        #endregion

        #region Properties

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
        [Description(PropertyDescription.Type)]
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
        [Description(PropertyDescription.Color)]
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

        [Category(PropertyCategory.Appearance)]
        [Description(PropertyDescription.Color)]
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

        [Category(PropertyCategory.Layout)]
        [Description(PropertyDescription.Size)]
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

        #endregion

        #region Events

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

                _borderEdge.BackColor = theme.OtherSettings.Line;

                BackColorState.Enabled = theme.ColorStateSettings.Enabled;
                BackColorState.Disabled = theme.ColorStateSettings.Disabled;
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }

            Invalidate();
            OnThemeChanged(new ThemeEventArgs(theme));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            try
            {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.GammaCorrected;

            Size textArea = GraphicsManager.MeasureText(graphics, Text, Font);
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
                _borderEdge.Location = new Point(_titleBoxRectangle.X + _border.Thickness, _titleBoxRectangle.Bottom);
                _borderEdge.Size = new Size(Width - _border.Thickness - 1, 1);
            }

            VisualBorderRenderer.DrawBorderStyle(e.Graphics, _border, ControlGraphicsPath, MouseState);

                if (_boxStyle == GroupBoxStyle.Classic)
                {
                    Size _newSize;
                    if (_image != null)
                    {
                        _newSize = _image.Size;
                    }
                    else
                    {
                        _newSize = new Size(0, 0);
                    }

                    Point _titleBoxBackground = GraphicsManager.GetTextImageRelationLocation(graphics, _textImageRelation, new Rectangle(new Point(0, 0), _newSize), Text, Font, _titleBoxRectangle, false);
                    graphics.FillRectangle(new SolidBrush(BackColorState.Enabled), new Rectangle(new Point(_titleBoxBackground.X, _titleBoxBackground.Y), new Size(_titleBoxRectangle.Width, _titleBoxRectangle.Height)));
                }

                if (_image != null)
                {
                    VisualControlRenderer.DrawContent(e.Graphics, _titleBoxRectangle, Text, Font, ForeColor, _image, _image.Size, _textImageRelation);
                }
                else
                {
                    VisualControlRenderer.DrawContentText(e.Graphics, _titleBoxRectangle, Text, Font, ForeColor, _textAlignment, _textLineAlignment);
                }
            }
            catch (Exception exception)
            {
                VisualExceptionDialog.Show(exception);
                throw;
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
                        groupBoxPoint = new Point(0, textArea.Height / 2);
                        groupBoxSize = new Size(Width - _border.Thickness, Height - (textArea.Height / 2) - _border.Thickness);

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
                        titlePoint = new Point(titlePoint.X, 0);

                        break;
                    }

                case GroupBoxStyle.Classic:
                    {
                        const int Spacing = 5;

                        titlePoint = new Point(titlePoint.X, 0);

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
                                    titlePoint.X = Width - textArea.Width - Spacing;
                                    break;
                                }

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            return new Rectangle(titlePoint, titleSize);
        }

        #endregion
    }
}