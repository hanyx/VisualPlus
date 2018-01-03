namespace VisualPlus.Structure
{
    #region Namespace

    using System.Drawing;
    using System.Drawing.Text;

    #endregion

    public class TextStyle
    {
        #region Variables

        private Color _disabled;
        private Color _enabled;
        private Color _hover;
        private StringFormat _stringFormat;
        private TextRenderingHint _textRenderingHint;

        #endregion

        #region Constructors

        public TextStyle()
        {
            _disabled = Color.Empty;
            _enabled = Color.Empty;
            _hover = Color.Empty;
            _stringFormat = StringFormat.GenericDefault;
            _textRenderingHint = Settings.DefaultValue.TextRenderingHint;
        }

        #endregion

        #region Properties

        public Color Disabled
        {
            get
            {
                return _disabled;
            }

            set
            {
                _disabled = value;
            }
        }

        public Color Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
            }
        }

        public Color Hover
        {
            get
            {
                return _hover;
            }

            set
            {
                _hover = value;
            }
        }

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