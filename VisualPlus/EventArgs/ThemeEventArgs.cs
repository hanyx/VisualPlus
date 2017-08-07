namespace VisualPlus.EventArgs
{
    #region Namespace

    using System;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;

    #endregion

    public class ThemeEventArgs : EventArgs
    {
        #region Variables

        private Control _control;
        private Styles _style;

        #endregion

        #region Constructors

        public ThemeEventArgs(Control control, Styles style)
        {
            _control = control;
            _style = style;
        }

        #endregion

        #region Properties

        public Control Control
        {
            get
            {
                return _control;
            }

            set
            {
                _control = value;
            }
        }

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