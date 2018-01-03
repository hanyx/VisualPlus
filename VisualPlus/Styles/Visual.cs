namespace VisualPlus.Styles
{
    #region Namespace

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;

    #endregion

    public class Visual : IBorder, ICheckmark, IControl, IColorState, IControlColorState, IFont, IShape, IProgress, ITab, IWatermark
    {
        #region Variables

        private readonly Color defaultBackgroundColorNoDepth = Color.White;

        #endregion

        #region Properties

        public Color ActiveColor
        {
            get
            {
                return Color.Gray;
            }
        }

        public Color BackCircle
        {
            get
            {
                return Color.FromArgb(52, 73, 96);
            }
        }

        public Color BackProgress
        {
            get
            {
                return Color.FromArgb(224, 222, 220);
            }
        }

        public Color BoxDisabled
        {
            get
            {
                return Color.FromArgb(131, 129, 129);
            }
        }

        public Color BoxEnabled
        {
            get
            {
                return Color.FromArgb(241, 244, 249);
            }
        }

        public Color CheckColor
        {
            get
            {
                return ColorTranslator.FromHtml("#2D882D");
            }
        }

        [Obsolete]
        public Color Color
        {
            get
            {
                return Color.FromArgb(180, 180, 180);
            }
        }

        public Color ControlDisabled
        {
            get
            {
                return Color.FromArgb(243, 243, 243);
            }
        }

        public Color ControlEnabled
        {
            get
            {
                return Color.FromArgb(226, 226, 226);
            }
        }

        public Color ControlHover
        {
            get
            {
                return Color.FromArgb(181, 181, 181);
            }
        }

        public Color ControlPressed
        {
            get
            {
                return Color.FromArgb(137, 136, 136);
            }
        }

        public Color FlatButtonDisabled
        {
            get
            {
                return Color.FromArgb(243, 243, 243);
            }
        }

        public Color FlatButtonEnabled
        {
            get
            {
                return Color.FromArgb(119, 119, 118);
            }
        }

        public FontFamily FontFamily
        {
            get
            {
                return new FontFamily("Segoe UI");
            }
        }

        public float FontSize
        {
            get
            {
                return 8.25F;
            }
        }

        public FontStyle FontStyle
        {
            get
            {
                return FontStyle.Regular;
            }
        }

        public Color ForeCircle
        {
            get
            {
                return Color.FromArgb(48, 56, 68);
            }
        }

        [Obsolete]
        public Color ForeColor
        {
            get
            {
                return Color.Black;
            }
        }

        [Obsolete]
        public Color ForeColorDisabled
        {
            get
            {
                return Color.FromArgb(131, 129, 129);
            }
        }

        [Obsolete]
        public Color ForeColorSelected
        {
            get
            {
                return Color.FromArgb(217, 220, 227);
            }
        }

        public Color Hatch
        {
            get
            {
                return Color.FromArgb(20, Color.Black);
            }
        }

        [Obsolete]
        public Color HoverColor
        {
            get
            {
                return Color.FromArgb(120, 183, 230);
            }
        }

        public Color InactiveColor
        {
            get
            {
                return Color.LightGray;
            }
        }

        public Color ItemEnabled
        {
            get
            {
                return Color.White;
            }
        }

        public Color ItemHover
        {
            get
            {
                return Color.FromArgb(241, 241, 241);
            }
        }

        public Color Line
        {
            get
            {
                return Color.FromArgb(224, 222, 220);
            }
        }

        public Color Menu
        {
            get
            {
                return Color.FromArgb(55, 61, 73);
            }
        }

        public Color Progress
        {
            get
            {
                return ColorTranslator.FromHtml("#2D882D");
            }
        }

        public Color ProgressDisabled
        {
            get
            {
                return Color.FromArgb(131, 129, 129);
            }
        }

        public Color Shadow
        {
            get
            {
                return Color.FromArgb(250, 249, 249);
            }
        }

        public Themes StyleManagement
        {
            get
            {
                return Themes.Visual;
            }
        }

        public Color TabEnabled
        {
            get
            {
                return Color.FromArgb(55, 61, 73);
            }
        }

        public Color TabHover
        {
            get
            {
                return Color.FromArgb(35, 36, 38);
            }
        }

        public Color TabSelected
        {
            get
            {
                return Color.FromArgb(70, 76, 88);
            }
        }

        #endregion

        #region Events

        private static List<Color> GetBackgroundColor()
        {
            var list = new List<Color>
                {
                    ControlPaint.LightLight(Color.Gainsboro),
                    ControlPaint.Light(Color.Gainsboro),
                    Color.FromArgb(66, 64, 65),
                    Color.FromArgb(241, 244, 249)
                };

            return list;
        }

        Color IControl.Background(int depth)
        {
            if (depth < GetBackgroundColor().Count)
            {
                return GetBackgroundColor()[depth];
            }

            return defaultBackgroundColorNoDepth;
        }

        #endregion
    }
}