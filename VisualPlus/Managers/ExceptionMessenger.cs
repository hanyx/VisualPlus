namespace VisualPlus.Managers
{
    #region Namespace

    using System;
    using System.Text;

    #endregion

    public class ExceptionMessenger
    {
        #region Events

        /// <summary>Create file not found string.</summary>
        /// <param name="path">The package path.</param>
        /// <returns>
        ///     <see cref="string" />
        /// </returns>
        public static string FileNotFound(string path)
        {
            StringBuilder _fileNotFound = new StringBuilder();
            _fileNotFound.AppendLine("Unable to locate the file using the following path. " + path);
            return _fileNotFound.ToString();
        }

        /// <summary>Create is null or empty string.</summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     <see cref="string" />
        /// </returns>
        public static string IsNull(object value)
        {
            StringBuilder _isNullOrEmpty = new StringBuilder();
            _isNullOrEmpty.AppendLine("The object is null.");
            _isNullOrEmpty.Append(Environment.NewLine);
            _isNullOrEmpty.AppendLine("Object: " + nameof(value));
            _isNullOrEmpty.AppendLine("Type: " + value.GetType());
            return _isNullOrEmpty.ToString();
        }

        /// <summary>Create is null or empty string.</summary>
        /// <param name="text">The text.</param>
        /// <returns>
        ///     <see cref="string" />
        /// </returns>
        public static string IsNullOrEmpty(string text)
        {
            StringBuilder _isNullOrEmpty = new StringBuilder();
            _isNullOrEmpty.AppendLine("The string is null or empty. " + nameof(text));
            return _isNullOrEmpty.ToString();
        }

        #endregion
    }
}