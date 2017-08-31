namespace VisualPlus.EventArgs
{
    #region Namespace

    using System;
    using System.Windows.Forms;

    #endregion

    public class CursorChangedEventArgs : EventArgs
    {
        #region Variables

        private Cursor _cursor;

        #endregion

        #region Constructors

        public CursorChangedEventArgs(Cursor cursor)
        {
            _cursor = cursor;
        }

        #endregion

        #region Properties

        public Cursor Cursor
        {
            get
            {
                return _cursor;
            }

            set
            {
                _cursor = value;
            }
        }

        #endregion
    }
}