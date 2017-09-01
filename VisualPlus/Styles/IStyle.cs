namespace VisualPlus.Styles
{
    #region Namespace

    using System.Drawing;

    #endregion

    public interface IShape
    {
        #region Properties

        Color Color { get; }

        #endregion
    }

    public interface IBorder
    {
        #region Properties

        Color HoverColor { get; }

        #endregion
    }

    public interface ICheckmark
    {
        #region Properties

        Color CheckColor { get; }

        #endregion
    }

    public interface IColorState
    {
        #region Properties

        Color ControlDisabled { get; }

        Color ControlEnabled { get; }

        #endregion
    }

    public interface IControlColorState
    {
        #region Properties

        Color ControlHover { get; }

        Color ControlPressed { get; }

        #endregion
    }

    public interface IControl
    {
        #region Properties

        Color BoxDisabled { get; }

        Color BoxEnabled { get; }

        Color FlatButtonDisabled { get; }

        Color FlatButtonEnabled { get; }

        Color ItemEnabled { get; }

        Color ItemHover { get; }

        Color Line { get; }

        Color Shadow { get; }

        #endregion

        #region Events

        Color Background(int depth);

        #endregion
    }

    public interface IFont
    {
        #region Properties

        FontFamily FontFamily { get; }

        float FontSize { get; }

        FontStyle FontStyle { get; }

        Color ForeColor { get; }

        Color ForeColorDisabled { get; }

        Color ForeColorSelected { get; }

        #endregion
    }

    public interface IProgress
    {
        #region Properties

        Color BackCircle { get; }

        Color BackProgress { get; }

        Color ForeCircle { get; }

        Color Hatch { get; }

        Color Progress { get; }

        Color ProgressDisabled { get; }

        #endregion
    }

    public interface ITab
    {
        #region Properties

        Color Menu { get; }

        Color TabEnabled { get; }

        Color TabHover { get; }

        Color TabSelected { get; }

        #endregion
    }

    public interface IWatermark
    {
        #region Properties

        Color ActiveColor { get; }

        Color InactiveColor { get; }

        #endregion
    }
}