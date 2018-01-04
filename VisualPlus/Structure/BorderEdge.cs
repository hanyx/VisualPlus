namespace VisualPlus.Structure
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Localization;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(false)]
    [DefaultEvent("Paint")]
    [DefaultProperty("Orientation")]
    [DesignerCategory("code")]
    [Description("Displays a vertical or horizontal border edge.")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public class BorderEdge : VisualStyleBase
    {
        #region Variables

        private Orientation _orientation;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="BorderEdge" /> class.</summary>
        public BorderEdge()
        {
            SetStyle(ControlStyles.Selectable, false);
            Cursor = Cursors.Default;
            BackColor = Color.Black;
            Size = new Size(50, 50);
            _orientation = Orientation.Horizontal;
        }

        #endregion

        #region Properties

        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Orientation)]
        public Orientation Orientation
        {
            get
            {
                return _orientation;
            }

            set
            {
                _orientation = value;

                if (_orientation == Orientation.Horizontal)
                {
                    if (Width < Height)
                    {
                        int _temp = Width;
                        Width = Height;
                        Height = _temp;
                    }
                }
                else
                {
                    if (Width > Height)
                    {
                        int _temp = Width;
                        Width = Height;
                        Height = _temp;
                    }
                }

                Invalidate();
            }
        }

        #endregion

        #region Events

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(new Pen(BackColor), Location.X, Location.Y, Width, Height);
        }

        #endregion
    }
}