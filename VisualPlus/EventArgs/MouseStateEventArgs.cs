namespace VisualPlus.EventArgs
{
    #region Namespace

    using System;

    using VisualPlus.Enumerators;

    #endregion

    public class MouseStateEventArgs : EventArgs
    {
        #region Variables

        private MouseStates _mouseStates;

        #endregion

        #region Constructors

        public MouseStateEventArgs(MouseStates mouseStates)
        {
            _mouseStates = mouseStates;
        }

        #endregion

        #region Properties

        public MouseStates MouseStates
        {
            get
            {
                return _mouseStates;
            }

            set
            {
                _mouseStates = value;
            }
        }

        #endregion
    }
}