namespace VisualPlus.Managers
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    #endregion

    [Description("The math manager.")]
    public sealed class MathManager
    {
        #region Events

        /// <summary>Converts a degree to a radian.</summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The <see cref="float" />.</returns>
        public static float DegreeToRadians(float angle)
        {
            return (float)((angle * Math.PI) / 180);
        }

        /// <summary>Retrieves the number closest from the value collection.</summary>
        /// <param name="value">The intial value to compare with.</param>
        /// <param name="valueCollection">The value collection to search.</param>
        /// <returns>The <see cref="int" />.</returns>
        public static int FindClosestValue(int value, int[] valueCollection)
        {
            return valueCollection.Aggregate((x, y) => Math.Abs(x - value) < Math.Abs(y - value) ? x : y);
        }

        /// <summary>Gets the fraction.</summary>
        /// <param name="value">Current value.</param>
        /// <param name="total">Total value.</param>
        /// <param name="digits">The number of fractional digits in the return number.</param>
        /// <returns>The <see cref="float" />.</returns>
        public static float GetFraction(double value, double total, int digits)
        {
            // Convert to double value
            double factor = value / 100;

            // Multiply by self
            factor = total * factor;

            // Round to digits
            factor = Math.Round(factor, digits);

            return (float)factor;
        }

        /// <summary>Gets the fraction.</summary>
        /// <param name="value">Current value.</param>
        /// <param name="total">Total value.</param>
        /// <returns>The <see cref="int" />.</returns>
        public static int GetFraction(double value, double total)
        {
            // Convert to decimal value
            double factor = value / 100;

            // Multiply by amount of bars
            factor = total * factor;

            // Round to fraction
            factor = Math.Round(factor, 0);

            return Convert.ToInt32(factor);
        }

        /// <summary>Gets half a radian angle.</summary>
        /// <param name="value">The progress value.</param>
        /// <returns>The <see cref="int" />.</returns>
        public static int GetHalfRadianAngle(int value)
        {
            return int.Parse(Math.Round((value * 180.0) / 100.0, 0).ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>Converts a radian angle to a degree.</summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The <see cref="float" />.</returns>
        public static float RadianToDegree(float angle)
        {
            return (float)(angle * (180.0 / Math.PI));
        }

        #endregion
    }
}