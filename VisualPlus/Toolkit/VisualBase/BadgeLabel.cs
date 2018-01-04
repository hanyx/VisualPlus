namespace VisualPlus.Toolkit.VisualBase
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Localization;
    using VisualPlus.Managers;
    using VisualPlus.Renders;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    #endregion

    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    public class BadgeLabel : Label
    {
        #region Variables

        public Action<Control> ClickEvent;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="BadgeLabel" /> class.</summary>
        public BadgeLabel()
        {
            StylesManager _styleManager = new StylesManager(Settings.DefaultValue.DefaultStyle);

            Background = Color.FromArgb(120, 183, 230);
            ForeColor = Color.White;
            Shape = new Shape();
            Text = "0";
            Font = _styleManager.Theme.TextSetting.Font;
            Location = new Point(0, 0);
            BackColor = Color.Transparent;
            Size = new Size(25, 20);
        }

        #endregion

        #region Properties

        [DefaultValue(typeof(Color), "Blue")]
        [Category(PropertyCategory.Appearance)]
        public Color Background { get; set; }

        [TypeConverter(typeof(ShapeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(PropertyCategory.Appearance)]
        public Shape Shape { get; set; }

        #endregion

        #region Events

        protected override void OnClick(EventArgs e)
        {
            ClickEvent(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Size _textSize = GraphicsManager.MeasureText(e.Graphics, Text, Font);
            VisualBadgeRenderer.DrawBadge(e.Graphics, new Rectangle(new Point(0, 0), new Size(Width - 1, Height - 1)), Background, Text, Font, ForeColor, Shape, new Point((Width / 2) - (_textSize.Width / 2), (Height / 2) - (_textSize.Height / 2)));
        }

        #endregion
    }
}