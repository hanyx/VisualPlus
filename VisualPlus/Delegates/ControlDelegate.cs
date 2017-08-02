namespace VisualPlus.Delegates
{
    #region Namespace

    using VisualPlus.EventArgs;

    #endregion

    public delegate void ForeColorDisabledChangedEventHandler(ColorEventArgs e);

    public delegate void MouseStateChangedEventHandler(MouseStateEventArgs e);

    public delegate void StyleManagerChangedEventHandler();

    public delegate void TextRenderingChangedEventHandler(TextRenderingEventArgs e);

    public delegate void BackgroundChangedEventHandler(ColorEventArgs e);
}