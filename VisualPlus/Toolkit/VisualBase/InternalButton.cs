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
    public abstract class InternalButton : VisualStyleBase
    {
        #region Variables

        private TextImageRelation textImageRelation;
        private Point textPoint = new Point(0, 0);
        private VisualBitmap visualBitmap;

        #endregion

        #region Constructors

        protected InternalButton()
        {
            visualBitmap = new VisualBitmap(Resources.Icon, new Size(24, 24))
                {
                    Visible = false,
                    Image = Resources.Icon
                };

            visualBitmap.Point = new Point(0, (Height / 2) - (visualBitmap.Size.Height / 2));

            textImageRelation = TextImageRelation.Overlay;
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(VisualBitmapConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Localize.PropertiesCategory.Appearance)]
        public VisualBitmap Image
        {
            get
            {
                return visualBitmap;
            }

            set
            {
                visualBitmap = value;
                Invalidate();
            }
        }

        [Category(Localize.PropertiesCategory.Behavior)]
        [Description(Localize.Description.Common.TextImageRelation)]
        public TextImageRelation TextImageRelation
        {
            get
            {
                return textImageRelation;
            }

            set
            {
                textImageRelation = value;
                Invalidate();
            }
        }

        internal bool ColorGradientToggle { get; set; }

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
            
            visualBitmap.Point = GDI.ApplyTextImageRelation(graphics, textImageRelation, new Rectangle(visualBitmap.Point, visualBitmap.Size), Text, Font, ClientRectangle, true);
            textPoint = GDI.ApplyTextImageRelation(graphics, textImageRelation, new Rectangle(visualBitmap.Point, visualBitmap.Size), Text, Font, ClientRectangle, false);

            if (ColorGradientToggle)
            {
                graphics.Clear(Parent.BackColor);
                graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

                LinearGradientBrush controlGraphicsBrush = GDI.GetControlBrush(graphics, Enabled, MouseState, ControlBrushCollection, ClientRectangle);
                GDI.FillBackground(graphics, ControlGraphicsPath, controlGraphicsBrush);
                Border.DrawBorderStyle(graphics, ControlBorder, MouseState, ControlGraphicsPath);
            }

            VisualBitmap.DrawImage(graphics, visualBitmap.Border, visualBitmap.Point, visualBitmap.Image, visualBitmap.Size, visualBitmap.Visible);
            graphics.DrawString(Text, Font, new SolidBrush(ForeColor), textPoint);
        }

        #endregion
    }
}