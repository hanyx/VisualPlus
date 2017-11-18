namespace VisualPlus.Toolkit.Child
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;

    using VisualPlus.Designer;

    #endregion

    [Designer(typeof(VisualTabPageDesigner))]
    public class VisualTabPage : TabPage
    {
        #region Variables

        private Color backColor;

        #endregion

        #region Constructors

        public VisualTabPage()
        {
            backColor = Color.Transparent;
            UpdateStyles();
        }

        #endregion

        #region Properties

        [Browsable(false)]
        public new Color BackColor
        {
            get
            {
                return backColor;
            }

            set
            {
                backColor = value;
            }
        }

        [Category("VisualPlus")]
        [Bindable(false)]
        public Color BackgroundColor { get; set; }

        #endregion

        #region Events

        /// <summary>Updates the properties after an Invalidate.</summary>
        public void UpdateProperties()
        {
            try
            {
                Invalidate();
            }
            catch (Exception e)
            {
                throw new Exception(e.StackTrace);
            }
        }

        /// <summary>Creates a control instance.</summary>
        /// <returns>
        ///     <see cref="Control.ControlCollection" />
        /// </returns>
        protected override ControlCollection CreateControlsInstance()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            DoubleBuffered = true;

            return base.CreateControlsInstance();
        }

        /// <summary>Raises the Paint event.</summary>
        /// <param name="e">The paint event arguments.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics _graphics = e.Graphics;
            _graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using (SolidBrush _backgroundBrush = new SolidBrush(BackgroundColor))
            {
                _graphics.FillRectangle(_backgroundBrush, ClientRectangle);
            }
        }

        #endregion
    }
}