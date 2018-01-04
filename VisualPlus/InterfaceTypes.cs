namespace VisualPlus
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;

    using VisualPlus.Enumerators;
    using VisualPlus.Localization;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Components;

    #endregion

    /// <summary>The IThemeManager.</summary>
    public interface IThemeManager
    {
        #region Properties

        /// <summary>The style manager.</summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        StylesManager ThemeManager { get; set; }

        #endregion
    }

    /// <summary>The ITheme supported control.</summary>
    public interface IThemeSupport
    {
        #region Events

        /// <summary>Update the control theme.</summary>
        /// <param name="theme">The theme to update with.</param>
        void UpdateTheme(Theme theme);

        #endregion
    }

    public interface IInputMethods
    {
        #region Events

        void AppendText(string text);

        void Clear();

        void ClearUndo();

        void Copy();

        void Cut();

        void DeselectAll();

        int GetCharFromPosition(Point pt);

        int GetCharIndexFromPosition(Point pt);

        int GetFirstCharIndexFromLine(int lineNumber);

        int GetLineFromCharIndex(int index);

        Point GetPositionFromCharIndex(int index);

        void Paste();

        void ScrollToCaret();

        void Select(int start, int length);

        void SelectAll();

        void Undo();

        #endregion
    }

    public interface IAnimationSupport
    {
        #region Properties

        [DefaultValue(Settings.DefaultValue.Animation)]
        [Category(PropertyCategory.Behavior)]
        [Description(PropertyDescription.Animation)]
        bool Animation { get; set; }

        #endregion

        #region Events

        /// <summary>Configures the animation settings.</summary>
        /// <param name="effectIncrement">The effect Increment.</param>
        /// <param name="effectType">The effect Type.</param>
        void ConfigureAnimation(double[] effectIncrement, EffectType[] effectType);

        /// <summary>Draws the animation on the graphics.</summary>
        /// <param name="graphics">The specified graphics to draw on.</param>
        void DrawAnimation(Graphics graphics);

        #endregion
    }

    public interface IControlStateColor
    {
        #region Properties

        Color Background { get; set; }

        Color BackgroundDisabled { get; set; }

        Color BackgroundHover { get; set; }

        Color BackgroundPressed { get; set; }

        #endregion
    }

    public interface IImageControl
    {
        #region Properties

        Image Hover { get; set; }

        Image Normal { get; set; }

        Image Pressed { get; set; }

        #endregion
    }
}