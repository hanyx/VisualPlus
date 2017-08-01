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
    using VisualPlus.Properties;
    using VisualPlus.Structure;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public abstract class ButtonContentBase : VisualControlBase, IControlStates
    {
        #region Variables

        private TextImageRelation textImageRelation;
        private Point textPoint = new Point(0, 0);
        private VisualBitmap visualBitmap;

        #endregion

        #region Constructors

        protected ButtonContentBase()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;

            visualBitmap = new VisualBitmap(Resources.Icon, new Size(24, 24))
                {
                    Visible = false
                };
            visualBitmap.Point = new Point(0, (Height / 2) - (visualBitmap.Size.Height / 2));

            textImageRelation = TextImageRelation.Overlay;
        }

        #endregion

        #region Properties

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

        #endregion

        #region Events

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MouseState = MouseStates.Down;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseState = MouseStates.Hover;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            ConfigureComponents(graphics);
            DrawBackground(graphics);
            VisualBitmap.DrawImage(graphics, visualBitmap.Border, visualBitmap.Point, visualBitmap.Image, visualBitmap.Size, visualBitmap.Visible);
            graphics.DrawString(Text, Font, new SolidBrush(ForeColor), textPoint);
        }

        private void ConfigureComponents(Graphics graphics)
        {
            ControlGraphicsPath = Border.GetBorderShape(ClientRectangle, ControlBorder.Type, ControlBorder.Rounding);
            visualBitmap.Point = GDI.ApplyTextImageRelation(graphics, textImageRelation, new Rectangle(visualBitmap.Point, visualBitmap.Size), Text, Font, ClientRectangle, true);
            textPoint = GDI.ApplyTextImageRelation(graphics, textImageRelation, new Rectangle(visualBitmap.Point, visualBitmap.Size), Text, Font, ClientRectangle, false);
        }

        private void DrawBackground(Graphics graphics)
        {
            LinearGradientBrush controlGraphicsBrush = GDI.GetControlBrush(graphics, Enabled, MouseState, ControlBrushCollection, ClientRectangle);
            GDI.FillBackground(graphics, ControlGraphicsPath, controlGraphicsBrush);
            Border.DrawBorderStyle(graphics, ControlBorder, MouseState, ControlGraphicsPath);
        }

        #endregion
    }
}