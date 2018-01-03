namespace VisualPlus
{
    #region Namespace

    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;

    using VisualPlus.Enumerators;

    #endregion

    internal class Settings
    {
        #region Variables

        public static readonly int MaximumAlpha = 255;
        public static readonly int MaximumBorderSize = 24;
        public static readonly int MaximumCheckBoxBorderRounding = 12;
        public static readonly int MaximumCheckBoxSize = 11;
        public static readonly int MaximumRounding = 30;
        public static readonly int MinimumAlpha = 1;
        public static readonly int MinimumBorderSize = 1;
        public static readonly int MinimumCheckBoxBorderRounding = 1;
        public static readonly int MinimumCheckBoxSize = 3;
        public static readonly int MinimumRounding = 1;

        public static readonly string TemplatesFolder = Environment.GetFolderPath(Environment.SpecialFolder.Templates) + @"\VisualPlus Themes\";

        #endregion

        #region Methods

        public struct DefaultValue
        {
            public const bool Animation = true;
            public const int BorderThickness = 1;
            public const bool BorderHoverVisible = true;
            public const ShapeType BorderType = ShapeType.Rounded;
            public const bool BorderVisible = true;
            public const bool TextVisible = true;
            public const float ProgressSize = 5F;
            public const bool TitleBoxVisible = true;
            public const bool HatchVisible = true;
            public const int BarAmount = 5;
            public const Themes DefaultStyle = Themes.Visual;
            public static readonly Size HatchSize = new Size(2, 2);
            public const HatchStyle HatchStyle = System.Drawing.Drawing2D.HatchStyle.DarkDownwardDiagonal;
            public const bool Moveable = false;
            public const bool WatermarkVisible = false;

            public static readonly string WatermarkText = "Watermark text";
            public static TextRenderingHint TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            public struct Rounding
            {
                public const int Default = 6;
                public const int BoxRounding = 3;
                public const int RoundedRectangle = 12;
                public const int ToggleBorder = 20;
                public const int ToggleButton = 18;
            }
        }

        #endregion
    }
}