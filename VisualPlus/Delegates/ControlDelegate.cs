namespace VisualPlus.Delegates
{
    #region Namespace

    using VisualPlus.EventArgs;

    #endregion

    public delegate void ForeColorDisabledChangedEventHandler(ColorEventArgs e);

    public delegate void MouseStateChangedEventHandler(MouseStateEventArgs e);

    public delegate void TextRenderingChangedEventHandler(TextRenderingEventArgs e);

    public delegate void BackgroundChangedEventHandler(ColorEventArgs e);

    public delegate void ToggleChangedEventHandler(ToggleEventArgs e);

    public delegate void BackColorStateChangedEventHandler(ColorEventArgs e);

    public delegate void ValueChangedEventHandler(ValueChangedEventArgs e);
}