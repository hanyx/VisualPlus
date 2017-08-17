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
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(NumericUpDown))]
    [DefaultEvent("Click")]
    [DefaultProperty("Value")]
    [Description("The Visual NumericUpDown")]
    [Designer(ControlManager.FilterProperties.VisualNumericUpDown)]
    public class VisualNumericUpDown : VisualControlBase
    {
        #region Variables

        private Border _border;

        private Gradient backgroundGradient;
        private Border buttonBorder;
        private Font buttonFont;
        private Color buttonForeColor;
        private Gradient buttonGradient;
        private Orientation buttonOrientation;
        private GraphicsPath buttonPath;
        private Rectangle buttonRectangle;
        private int buttonWidth = 50;
        private Point[] decrementButtonPoints = new Point[2];
        private Point[] incrementButtonPoints = new Point[2];
        private bool keyboardNum;
        private long maximumValue;
        private long minimumValue;
        private long numericValue;
        private int xValue;
        private int yValue;

        #endregion

        #region Constructors

        public VisualNumericUpDown()
        {
            BackColor = Color.Transparent;
            minimumValue = 0;
            maximumValue = 100;
            Size = new Size(125, 25);
            MinimumSize = new Size(0, 0);

            buttonOrientation = Orientation.Horizontal;

            buttonBorder = new Border
                {
                    HoverVisible = false,
                    Type = ShapeType.Rectangle
                };

            _border = new Border();

            UpdateTheme(Settings.DefaultValue.DefaultStyle);
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(GradientConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Appearance)]
        public Gradient BackgroundGradient
        {
            get
            {
                return backgroundGradient;
            }

            set
            {
                backgroundGradient = value;
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

        [TypeConverter(typeof(GradientConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Appearance)]
        public Gradient Button
        {
            get
            {
                return buttonGradient;
            }

            set
            {
                buttonGradient = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Property.Appearance)]
        public Border ButtonBorder
        {
            get
            {
                return buttonBorder;
            }

            set
            {
                buttonBorder = value;
                Invalidate();
            }
        }

        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Strings.Font)]
        public Font ButtonFont
        {
            get
            {
                return buttonFont;
            }

            set
            {
                buttonFont = value;
                Invalidate();
            }
        }

        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.Color)]
        public Color ButtonForeColor
        {
            get
            {
                return buttonForeColor;
            }

            set
            {
                buttonForeColor = value;
                Invalidate();
            }
        }

        [Category(Property.Appearance)]
        [Description(Localization.Descriptions.Property.Description.Common.Alignment)]
        public Orientation ButtonOrientation
        {
            get
            {
                return buttonOrientation;
            }

            set
            {
                buttonOrientation = value;
                Invalidate();
            }
        }

        [Category(Property.Layout)]
        [Description(Localization.Descriptions.Property.Description.Common.Size)]
        public int ButtonWidth
        {
            get
            {
                return buttonWidth;
            }

            set
            {
                buttonWidth = value;
                Invalidate();
            }
        }

        [Category(Property.Behavior)]
        public long MaximumValue
        {
            get
            {
                return maximumValue;
            }

            set
            {
                if (value > minimumValue)
                {
                    maximumValue = value;
                }

                if (numericValue > maximumValue)
                {
                    numericValue = maximumValue;
                }

                Invalidate();
            }
        }

        [Category(Property.Behavior)]
        public long MinimumValue
        {
            get
            {
                return minimumValue;
            }

            set
            {
                if (value < maximumValue)
                {
                    minimumValue = value;
                }

                if (numericValue < minimumValue)
                {
                    numericValue = MinimumValue;
                }

                Invalidate();
            }
        }

        [Category(Property.Behavior)]
        public long Value
        {
            get
            {
                return numericValue;
            }

            set
            {
                if ((value <= maximumValue) & (value >= minimumValue))
                {
                    numericValue = value;
                }

                Invalidate();
            }
        }

        #endregion

        #region Events

        public void Decrement(int value)
        {
            numericValue -= value;
            Invalidate();
        }

        public void Increment(int value)
        {
            numericValue += value;
            Invalidate();
        }

        public void UpdateTheme(Styles style)
        {
            StyleManager = new StyleManager(style);

            buttonForeColor = Color.Gray;
            backgroundGradient = StyleManager.ControlStyle.BoxEnabled;
            buttonGradient = StyleManager.ControlStatesStyle.ControlEnabled;
            buttonFont = new Font(StyleManager.Font.FontFamily, 14, FontStyle.Bold);

            ControlBrushCollection = new[]
                {
                    StyleManager.ControlStatesStyle.ControlEnabled,
                    StyleManager.ControlStatesStyle.ControlHover,
                    StyleManager.ControlStatesStyle.ControlPressed,
                    StyleManager.ControlStatesStyle.ControlDisabled
                };

            ForeColor = StyleManager.FontStyle.ForeColor;
            ForeColorDisabled = StyleManager.FontStyle.ForeColorDisabled;
            Background = StyleManager.ControlStyle.Background(3);
            BackgroundDisabled = StyleManager.ControlStyle.Background(0);
            _border.Color = StyleManager.BorderStyle.Color;
            _border.HoverColor = StyleManager.BorderStyle.HoverColor;

            buttonBorder.Color = StyleManager.BorderStyle.Color;
            buttonBorder.HoverColor = StyleManager.BorderStyle.HoverColor;
            Invalidate();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            try
            {
                if (keyboardNum)
                {
                    numericValue = long.Parse(numericValue + e.KeyChar.ToString());
                }

                if (numericValue > maximumValue)
                {
                    numericValue = maximumValue;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Back)
            {
                string temporaryValue = numericValue.ToString();
                temporaryValue = temporaryValue.Remove(Convert.ToInt32(temporaryValue.Length - 1));
                if (temporaryValue.Length == 0)
                {
                    temporaryValue = "0";
                }

                numericValue = Convert.ToInt32(temporaryValue);
            }

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnMouseClick(e);

            switch (buttonOrientation)
            {
                case Orientation.Vertical:
                    {
                        // Check if mouse in X position.
                        if ((xValue > Width - buttonRectangle.Width) && (xValue < Width))
                        {
                            // Determine the button middle separator by checking for the Y position.
                            if ((yValue > buttonRectangle.Y) && (yValue < Height / 2))
                            {
                                if (Value + 1 <= maximumValue)
                                {
                                    numericValue++;
                                }
                            }
                            else if ((yValue > Height / 2) && (yValue < Height))
                            {
                                if (Value - 1 >= minimumValue)
                                {
                                    numericValue--;
                                }
                            }
                        }
                        else
                        {
                            keyboardNum = !keyboardNum;
                            Focus();
                        }

                        break;
                    }

                case Orientation.Horizontal:
                    {
                        // Check if mouse in X position.
                        if ((xValue > Width - buttonRectangle.Width) && (xValue < Width))
                        {
                            // Determine the button middle separator by checking for the X position.
                            if ((xValue > buttonRectangle.X) && (xValue < buttonRectangle.X + (buttonRectangle.Width / 2)))
                            {
                                if (Value + 1 <= maximumValue)
                                {
                                    numericValue++;
                                }
                            }
                            else if ((xValue > buttonRectangle.X + (buttonRectangle.Width / 2)) && (xValue < Width))
                            {
                                if (Value - 1 >= minimumValue)
                                {
                                    numericValue--;
                                }
                            }
                        }
                        else
                        {
                            keyboardNum = !keyboardNum;
                            Focus();
                        }

                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseState = MouseStates.Normal;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            xValue = e.Location.X;
            yValue = e.Location.Y;
            Invalidate();

            // IBeam cursor toggle
            if (e.X < buttonRectangle.X)
            {
                Cursor = Cursors.IBeam;
            }
            else
            {
                Cursor = Cursors.Hand;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                if (Value + 1 <= maximumValue)
                {
                    numericValue++;
                }

                Invalidate();
            }
            else
            {
                if (Value - 1 >= minimumValue)
                {
                    numericValue--;
                }

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            ControlGraphicsPath = VisualBorderRenderer.GetBorderShape(ClientRectangle, _border);
            buttonRectangle = new Rectangle(Width - buttonWidth, 0, buttonWidth, Height);

            Size incrementSize = GDI.MeasureText(graphics, "+", buttonFont);
            Size decrementSize = GDI.MeasureText(graphics, "-", buttonFont);

            incrementButtonPoints[0] = new Point((buttonRectangle.X + (buttonRectangle.Width / 2)) - (incrementSize.Width / 2), (buttonRectangle.Y + (buttonRectangle.Height / 2)) - (buttonRectangle.Height / 4) - (incrementSize.Height / 2));
            decrementButtonPoints[0] = new Point((buttonRectangle.X + (buttonRectangle.Width / 2)) - (decrementSize.Width / 2), (buttonRectangle.Y + (buttonRectangle.Height / 2) + (buttonRectangle.Height / 4)) - (decrementSize.Height / 2));

            incrementButtonPoints[1] = new Point((buttonRectangle.X + (buttonRectangle.Width / 4)) - (incrementSize.Width / 2), (Height / 2) - (incrementSize.Height / 2));
            decrementButtonPoints[1] = new Point((buttonRectangle.X + (buttonRectangle.Width / 2) + (buttonRectangle.Width / 4)) - (decrementSize.Width / 2), (Height / 2) - (decrementSize.Height / 2));

            var verticalSeparator = new Point[2];
            verticalSeparator[0] = new Point(buttonRectangle.X, buttonRectangle.Y + (buttonRectangle.Height / 2));
            verticalSeparator[1] = new Point(buttonRectangle.X + buttonRectangle.Width, buttonRectangle.Y + (buttonRectangle.Height / 2));

            var horizontalSeparator = new Point[2];
            horizontalSeparator[0] = new Point(buttonRectangle.X + (buttonRectangle.Width / 2), buttonRectangle.Y);
            horizontalSeparator[1] = new Point(buttonRectangle.X + (buttonRectangle.Width / 2), buttonRectangle.Y + buttonRectangle.Height);

            Point[] tempSeparator;
            int toggleInt;
            switch (buttonOrientation)
            {
                case Orientation.Vertical:
                    {
                        toggleInt = 0;
                        tempSeparator = verticalSeparator;
                        break;
                    }

                case Orientation.Horizontal:
                    {
                        toggleInt = 1;
                        tempSeparator = horizontalSeparator;
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

            buttonPath = new GraphicsPath();
            buttonPath.AddRectangle(buttonRectangle);
            buttonPath.CloseAllFigures();

            Gradient backgroundCheckTemp = Enabled ? backgroundGradient : ControlBrushCollection[3];
            Gradient buttonCheckTemp = Enabled ? buttonGradient : ControlBrushCollection[3];

            graphics.SetClip(ControlGraphicsPath);

            var gradientPoints = GDI.GetGradientPoints(ClientRectangle);

            LinearGradientBrush backgroundGradientBrush = Gradient.CreateGradientBrush(backgroundCheckTemp.Colors, gradientPoints, backgroundCheckTemp.Angle, backgroundCheckTemp.Positions);
            graphics.FillPath(backgroundGradientBrush, ControlGraphicsPath);

            LinearGradientBrush buttonGradientBrush = Gradient.CreateGradientBrush(buttonCheckTemp.Colors, gradientPoints, buttonCheckTemp.Angle, buttonCheckTemp.Positions);
            graphics.FillPath(buttonGradientBrush, buttonPath);

            VisualBorderRenderer.DrawBorderStyle(graphics, buttonBorder, MouseState, buttonPath);

            graphics.ResetClip();

            graphics.DrawString("+", buttonFont, new SolidBrush(buttonForeColor), incrementButtonPoints[toggleInt]);
            graphics.DrawString("-", buttonFont, new SolidBrush(buttonForeColor), decrementButtonPoints[toggleInt]);

            graphics.DrawLine(new Pen(StyleManager.BorderStyle.Color), tempSeparator[0], tempSeparator[1]);

            VisualBorderRenderer.DrawBorderStyle(graphics, _border, MouseState, ControlGraphicsPath);

            // Draw value string
            Rectangle textBoxRectangle = new Rectangle(6, 0, Width - 1, Height - 1);

            StringFormat stringFormat = new StringFormat
                {
                    // Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

            graphics.DrawString(Convert.ToString(Value), Font, new SolidBrush(ForeColor), textBoxRectangle, stringFormat);
        }

        #endregion
    }
}