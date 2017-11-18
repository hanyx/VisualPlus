namespace VisualPlus.Managers
{
    #region Namespace

    using System;
    using System.ComponentModel;

    #endregion

    [Description("The exception manager.")]
    public sealed class ExceptionManager
    {
        #region Events

        /// <summary>Returns a bool indicating whether the value is in range.</summary>
        /// <param name="value">The main value.</param>
        /// <param name="minimum">Minimum value.</param>
        /// <param name="maximum">Maximum value.</param>
        /// <param name="round">Round to nearest value when out of range.</param>
        /// <returns>The <see cref="int" />.</returns>
        public static int ArgumentOutOfRangeException(int value, int minimum, int maximum, bool round)
        {
            if ((value >= minimum) && (value <= maximum))
            {
                return value;
            }
            else
            {
                if (round)
                {
                    return MathManager.FindClosestValue(value, new[] { minimum, maximum });
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $@"The value ({value}) must be in range of ({minimum}) to ({maximum}).");
                }
            }
        }

        /// <summary>Returns a bool indicating whether the value is in range.</summary>
        /// <param name="value">The main value.</param>
        /// <param name="minimum">Minimum value.</param>
        /// <param name="maximum">Maximum value.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool ArgumentOutOfRangeException(float value, float minimum, float maximum)
        {
            if ((value >= minimum) && (value <= maximum))
            {
                return true;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value), $@"The value ({value}) must be in range of ({minimum}) to ({maximum}).");
            }
        }

        #endregion
    }
}