#region Namespace

using System.Drawing.Text;

#endregion

namespace VisualPlus.EventArgs
{
    public class TextRenderingEventArgs : System.EventArgs
    {
        #region Variables

        private TextRenderingHint _textRenderingHint;

        #endregion

        #region Constructors

        public TextRenderingEventArgs(TextRenderingHint textRenderingHint)
        {
            _textRenderingHint = textRenderingHint;
        }

        #endregion

        #region Properties

        public TextRenderingHint TextRenderingHint
        {
            get { return _textRenderingHint; }

            set { _textRenderingHint = value; }
        }

        #endregion
    }
}