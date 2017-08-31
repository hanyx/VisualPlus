namespace VisualPlus.Structure
{
    #region Namespace

    using System.Drawing;
    using System.Drawing.Text;

    #endregion

    public class TextState
    {
        #region Variables

        private Color _disabled;
        private Color _enabled;
        private TextRenderingHint _textRenderingHint;

        #endregion

        #region Constructors

        public TextState()
        {
            _disabled = Color.Empty;
            _enabled = Color.Empty;
            _textRenderingHint = Settings.DefaultValue.TextRenderingHint;
        }

        #endregion
    }
}