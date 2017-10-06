namespace VisualPlus.EventArgs
{
    #region Namespace

    using System;

    using VisualPlus.Enumerators;

    #endregion

    public class ThemeEventArgs : EventArgs
    {
        #region Variables

        private Styles _style;

        #endregion

        #region Constructors

        public ThemeEventArgs(Styles style)
        {
            _style = style;
        }

        #endregion

        #region Properties

        public Styles Style
        {
            get
            {
                return _style;
            }

            set
            {
                _style = value;
            }
        }

        #endregion
    }
}