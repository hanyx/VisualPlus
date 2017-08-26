namespace VisualPlus.EventArgs
{
    public class ToggleEventArgs : System.EventArgs
    {
        #region Variables

        private bool _state;

        #endregion

        #region Constructors

        public ToggleEventArgs(bool state)
        {
            _state = state;
        }

        #endregion

        #region Properties

        public bool State
        {
            get { return _state; }

            set { _state = value; }
        }

        #endregion
    }
}