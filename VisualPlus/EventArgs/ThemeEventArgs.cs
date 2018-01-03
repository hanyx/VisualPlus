namespace VisualPlus.EventArgs
{
    #region Namespace

    using System;

    using VisualPlus.Structure;

    #endregion

    public class ThemeEventArgs : EventArgs
    {
        #region Variables

        private Theme _theme;

        #endregion

        #region Constructors

        public ThemeEventArgs(Theme theme)
        {
            _theme = theme;
        }

        #endregion

        #region Properties

        public Theme Theme
        {
            get
            {
                return _theme;
            }

            set
            {
                _theme = value;
            }
        }

        #endregion
    }
}