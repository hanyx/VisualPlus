namespace VisualPlus.Toolkit.Dialogs
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    #endregion

    /// <summary>The <see cref="VisualExceptionDialog" />.</summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Description("The Visual Exception Dialog")]
    [ToolboxBitmap(typeof(VisualExceptionDialog), "Resources.VisualExceptionDialog.bmp")]
    [ToolboxItem(false)]
    public class VisualExceptionDialog : Form
    {
        #region Variables

        private Button _copyButton;

        private Label _descriptionLabel;

        private Exception _exception;

        private int _imageSpacing = 10;

        private Label _messageLabel;

        private TextBox _messageTextBox;

        private Button _okButton;

        private PictureBox _pictureBoxImage;

        private Button _saveButton;

        private Label _stackLabel;

        private TextBox _textBoxStack;

        private int _textheight = 16;
        private Label _typeLabel;

        private TextBox _typeTextBox;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualExceptionDialog" /> class.</summary>
        /// <param name="e">The exception.</param>
        public VisualExceptionDialog(Exception e)
        {
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Padding = new Padding(10);
            Size = new Size(440, 410);
            BackColor = Color.White;
            Text = @"Comet Exception Log";

            _exception = e;

            var _defaultWidth = 360;

            InitializePictureBoxImage();
            InitializeDescriptionLabel(_defaultWidth);
            InitializeMessage(_defaultWidth);
            InitializeType(_defaultWidth);
            InitializeStackTrace(_defaultWidth);
            InitializeButtons();
        }

        #endregion

        #region Events

        /// <summary>Show the exception dialog.</summary>
        /// <param name="exception">The exception.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="dialogWindow">The dialog Window.</param>
        public static void Show(Exception exception, string caption = "Exception Dialog", bool dialogWindow = true)
        {
            Thread _threadShowDialog = new Thread(() => Display(exception, caption, dialogWindow))
                {
                    IsBackground = true
                };

            _threadShowDialog.SetApartmentState(ApartmentState.STA);
            _threadShowDialog.Start();
        }

        /// <summary>Copy the log to the clipboard.</summary>
        public void CopyLogToClipboard()
        {
            Clipboard.SetText(CreateLog());
        }

        /// <summary>Create a log entry.</summary>
        /// <returns>
        ///     <see cref="string" />
        /// </returns>
        public string CreateLog()
        {
            StringBuilder _log = new StringBuilder();
            _log.AppendLine("Message:");
            _log.AppendLine(_exception.Message);
            _log.Append(Environment.NewLine);
            _log.AppendLine("Type:");
            _log.AppendLine(_exception.GetType().FullName);
            _log.Append(Environment.NewLine);
            _log.AppendLine("Stack Trace:");
            _log.AppendLine(_exception.StackTrace);

            _log.Append(Environment.NewLine);
            _log.AppendLine("Help Link: " + _exception.HelpLink);
            _log.AppendLine("Source: " + _exception.Source);
            _log.AppendLine("Target Site: " + _exception.TargetSite);

            return _log.ToString();
        }

        /// <summary>Saves the log to a file.</summary>
        /// <param name="filePath">The file Path.</param>
        public void SaveLog(string filePath)
        {
            File.WriteAllText(filePath, CreateLog());
        }

        /// <summary>Display the <see cref="VisualExceptionDialog" />.</summary>
        /// <param name="exception">The exception.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="dialogWindow">The dialog Window.</param>
        private static void Display(Exception exception, string caption, bool dialogWindow)
        {
            VisualExceptionDialog _exceptionDialog = new VisualExceptionDialog(exception)
                {
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };

            if (dialogWindow)
            {
                _exceptionDialog.ShowDialog();
            }
            else
            {
                _exceptionDialog.Show();
            }
        }

        /// <summary>The Copy button is clicked.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void CopyButton_Click(object sender, EventArgs e)
        {
            CopyLogToClipboard();
        }

        /// <summary>Initializes the ok button.</summary>
        private void InitializeButtons()
        {
            var _buttonSpacing = 7;
            Size _buttonSize = new Size(75, 23);

            _okButton = new Button
                {
                    BackColor = SystemColors.Control,
                    Text = @"OK",
                    Size = _buttonSize,
                    Location = new Point(_textBoxStack.Right - _buttonSize.Width, _textBoxStack.Bottom + 10),
                    TabIndex = 0
                };

            _okButton.Click += OkButton_Click;

            Controls.Add(_okButton);

            _saveButton = new Button
                {
                    BackColor = SystemColors.Control,
                    Text = @"Save",
                    Size = _buttonSize,
                    Location = new Point(_okButton.Left - _buttonSpacing - _buttonSize.Width, _textBoxStack.Bottom + 10),
                    TabIndex = 1
                };

            _saveButton.Click += SaveButton_Click;

            Controls.Add(_saveButton);

            _copyButton = new Button
                {
                    BackColor = SystemColors.Control,
                    Text = @"Copy",
                    Size = _buttonSize,
                    Location = new Point(_saveButton.Left - _buttonSpacing - _buttonSize.Width, _textBoxStack.Bottom + 10),
                    TabIndex = 2
                };

            _copyButton.Click += CopyButton_Click;

            Controls.Add(_copyButton);
        }

        /// <summary>Initializes the description label.</summary>
        /// <param name="width">The width.</param>
        private void InitializeDescriptionLabel(int width)
        {
            _descriptionLabel = new Label
                {
                    Text = @"An unhandled exception has occurred in a component in your application.",
                    Location = new Point(_pictureBoxImage.Right + _imageSpacing, Padding.Top),
                    Size = new Size(width, _textheight),
                    BorderStyle = BorderStyle.None
                };

            Controls.Add(_descriptionLabel);
        }

        /// <summary>Initializes the text box message.</summary>
        /// <param name="width">The width.</param>
        private void InitializeMessage(int width)
        {
            _messageLabel = new Label
                {
                    Text = @"Message:",
                    BorderStyle = BorderStyle.None,
                    Location = new Point(_pictureBoxImage.Right + _imageSpacing, _descriptionLabel.Bottom + 10),
                    Size = new Size(width, _textheight)
                };

            Controls.Add(_messageLabel);

            string _message = _exception?.Message ?? "The exception was null.";

            _messageTextBox = new TextBox
                {
                    ReadOnly = true,
                    BackColor = BackColor,
                    BorderStyle = BorderStyle.None,
                    Text = _message,
                    Location = new Point(_messageLabel.Location.X, _messageLabel.Bottom),
                    Size = new Size(width, 20)
                };

            Controls.Add(_messageTextBox);
        }

        /// <summary>Initializes the picture box image.</summary>
        private void InitializePictureBoxImage()
        {
            _pictureBoxImage = new PictureBox
                {
                    Image = SystemIcons.Error.ToBitmap(),
                    Location = new Point(Padding.Left, Padding.Top),
                    Size = new Size(32, 32),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

            Controls.Add(_pictureBoxImage);
        }

        /// <summary>Initializes the text box stack trace.</summary>
        /// <param name="width">The width.</param>
        private void InitializeStackTrace(int width)
        {
            _stackLabel = new Label
                {
                    Text = @"Stack Trace:",
                    Location = new Point(_typeTextBox.Location.X, _typeTextBox.Bottom + 10),
                    Size = new Size(width, _textheight),
                    BorderStyle = BorderStyle.None
                };

            Controls.Add(_stackLabel);

            _textBoxStack = new TextBox
                {
                    BackColor = BackColor,
                    Text = _exception.StackTrace,
                    ReadOnly = true,
                    Multiline = true,
                    Location = new Point(_stackLabel.Location.X, _stackLabel.Bottom),
                    Size = new Size(width, 200),
                    ScrollBars = ScrollBars.Both
                };

            Controls.Add(_textBoxStack);
        }

        /// <summary>Initializes the type message.</summary>
        /// <param name="width">The width.</param>
        private void InitializeType(int width)
        {
            _typeLabel = new Label
                {
                    Text = @"Type:",
                    Location = new Point(_pictureBoxImage.Right + _imageSpacing, _messageTextBox.Bottom + 10),
                    Size = new Size(width, _textheight),
                    BorderStyle = BorderStyle.None
                };

            Controls.Add(_typeLabel);

            string _message = _exception?.GetType().ToString() ?? "The exception was null.";

            _typeTextBox = new TextBox
                {
                    ReadOnly = true,
                    BackColor = BackColor,
                    BorderStyle = BorderStyle.None,
                    Text = _message,
                    Location = new Point(_pictureBoxImage.Right + _imageSpacing, _typeLabel.Bottom),
                    Size = new Size(width, 20)
                };

            Controls.Add(_typeTextBox);
        }

        /// <summary>The OK button is clicked.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>The Copy button is clicked.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.Title = @"Save exception log...";
            _saveFileDialog.Filter = @"Text Files|*.log;*.txt|All Files|*.*";

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveLog(_saveFileDialog.FileName);
            }
        }

        #endregion
    }
}