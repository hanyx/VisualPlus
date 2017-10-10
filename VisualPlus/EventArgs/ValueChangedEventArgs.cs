namespace VisualPlus.EventArgs
{
    #region Namespace

    using System;

    #endregion

    public class ValueChangedEventArgs : EventArgs
    {
        #region Variables

        private long _value;

        #endregion

        #region Constructors

        public ValueChangedEventArgs(long value)
        {
            _value = value;
        }

        #endregion

        #region Properties

        public long Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        #endregion
    }
}