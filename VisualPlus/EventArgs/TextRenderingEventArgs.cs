namespace VisualPlus.EventArgs
{
    #region Namespace

    using System;
    using System.Drawing.Text;

    #endregion

    public class TextRenderingEventArgs : EventArgs
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
            get
            {
                return _textRenderingHint;
            }

            set
            {
                _textRenderingHint = value;
            }
        }

        #endregion
    }
}