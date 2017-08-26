namespace VisualPlus.EventArgs
{
    public class ThemeEventArgs : System.EventArgs
    {
        #region Variables

        private Enumerators.Styles _style;

        #endregion

        #region Constructors

        public ThemeEventArgs(Enumerators.Styles style)
        {
            _style = style;
        }

        #endregion

        #region Properties

        public Enumerators.Styles Style
        {
            get { return _style; }

            set { _style = value; }
        }

        #endregion
    }
}