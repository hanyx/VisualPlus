#region Namespace

using System.Windows.Forms;

#endregion

namespace VisualPlus.EventArgs
{
    public class CursorChangedEventArgs : System.EventArgs
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
            get { return _cursor; }

            set { _cursor = value; }
        }

        #endregion
    }
}