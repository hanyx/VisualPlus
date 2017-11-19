namespace VisualPlus.EventArgs
{
    #region Namespace

    using System;
    using System.Windows.Forms;

    #endregion

    public class ControlBoxEventArgs : EventArgs
    {
        #region Variables

        private Form _form;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ControlBoxEventArgs" /> class.</summary>
        /// <param name="form">The form.</param>
        public ControlBoxEventArgs(Form form)
        {
            _form = form;
        }

        #endregion

        #region Properties

        public Form Form
        {
            get
            {
                return _form;
            }

            set
            {
                _form = value;
            }
        }

        #endregion
    }
}