﻿namespace VisualPlus.Toolkit.Controls.Layout
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Localization;
    using VisualPlus.Managers;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(ScrollBar))]
    [DefaultEvent("Scroll")]
    [DefaultProperty("Value")]
    [Description("The Visual ScrollBar")]
    public class VisualScrollBar : ProgressBase
    {
        #region Variables

        private Orientation _orientation;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualScrollBar" /> class.</summary>
        public VisualScrollBar()
        {
            Height = 125;
            Width = 20;
            Minimum = 0;
            Maximum = 100;
            Value = 0;

            _orientation = Orientation.Vertical;
        }

        #endregion

        #region Properties

        [Category(PropertyCategory.Appearance)]
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
                Size = GraphicsManager.FlipOrientationSize(_orientation, Size);
                Invalidate();
            }
        }

        #endregion
    }
}