namespace VisualPlus.Delegates
{
    #region Namespace

    using VisualPlus.EventArgs;

    #endregion

    public delegate void ControlDragEventHandler(DragControlEventArgs e);

    public delegate void ControlDragToggleEventHandler(ToggleEventArgs e);

    public delegate void ControlDragCursorChangedEventHandler(CursorChangedEventArgs e);
}