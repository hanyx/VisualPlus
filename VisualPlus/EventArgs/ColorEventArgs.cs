#region Namespace

using System.Drawing;

#endregion

namespace VisualPlus.EventArgs
{
    public class ColorEventArgs : System.EventArgs
    {
        #region Variables

        private Color _color;

        #endregion

        #region Constructors

        public ColorEventArgs(Color color)
        {
            _color = color;
        }

        #endregion

        #region Properties

        public Color Color
        {
            get { return _color; }

            set { _color = value; }
        }

        #endregion
    }
}