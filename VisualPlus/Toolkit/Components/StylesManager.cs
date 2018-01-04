namespace VisualPlus.Toolkit.Components
{
    #region Namespace

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using VisualPlus.Delegates;
    using VisualPlus.Enumerators;
    using VisualPlus.EventArgs;
    using VisualPlus.Managers;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Controls.DataManagement;
    using VisualPlus.Toolkit.Controls.DataVisualization;
    using VisualPlus.Toolkit.Controls.Editors;
    using VisualPlus.Toolkit.Controls.Interactivity;
    using VisualPlus.Toolkit.Controls.Layout;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.TypeConverters;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Component))]
    [Description("The style manager component enables you to manage the control themes.")]
    public class StylesManager : Component, ICloneable
    {
        #region Variables

        private string _customThemePath;
        private List<Form> _formCollection;
        private Theme _theme;
        private Themes _themeType;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="StylesManager" /> class.</summary>
        /// <param name="container">The container.</param>
        public StylesManager(IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>Initializes a new instance of the <see cref="StylesManager" /> class.</summary>
        /// <param name="filePath">The custom theme.</param>
        public StylesManager(string filePath) : this()
        {
            _theme = new Theme(filePath);
        }

        /// <summary>Initializes a new instance of the <see cref="StylesManager" /> class.</summary>
        /// <param name="theme">The custom theme.</param>
        public StylesManager(Theme theme) : this()
        {
            _theme = new Theme(theme);
        }

        /// <summary>Initializes a new instance of the <see cref="StylesManager" /> class.</summary>
        /// <param name="form">The form.</param>
        public StylesManager(Form form) : this()
        {
            AddFormToManage(form);
        }

        /// <summary>Initializes a new instance of the <see cref="StylesManager" /> class.</summary>
        /// <param name="theme">The style.</param>
        public StylesManager(Themes theme) : this()
        {
            _theme = new Theme(theme);
        }

        /// <summary>Initializes a new instance of the <see cref="StylesManager" /> class.</summary>
        /// <param name="form">The form.</param>
        /// <param name="filePath">The custom theme.</param>
        public StylesManager(Form form, string filePath) : this()
        {
            _theme = new Theme(filePath);
            AddFormToManage(form);
        }

        /// <summary>Initializes a new instance of the <see cref="StylesManager" /> class.</summary>
        /// <param name="form">The form.</param>
        /// <param name="theme">The style.</param>
        public StylesManager(Form form, Themes theme) : this()
        {
            _theme = new Theme(theme);
            AddFormToManage(form);
        }

        /// <summary>Prevents a default instance of the <see cref="StylesManager" /> class from being created.</summary>
        private StylesManager()
        {
            try
            {
                ConstructDefaultThemeFile();

                if (_customThemePath == null)
                {
                    string _themePath = Settings.TemplatesFolder + @"DefaultTheme.xml";
                    _theme = new Theme(_themePath);
                    _customThemePath = _themePath;
                }

                _formCollection = new List<Form>();

                _themeType = Settings.DefaultValue.DefaultStyle;
                _theme = new Theme(_themeType);
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }
        }

        public event ThemeChangedEventHandler ThemeChanged;

        #endregion

        #region Properties

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public List<Control> Controls
        {
            get
            {
                var _controlsList = new List<Control>();

                foreach (Form _forms in _formCollection)
                {
                    _controlsList.AddRange(_forms.Controls.Cast<Control>());
                }

                return _controlsList;
            }
        }

        [Editor(typeof(ThemesEditor), typeof(UITypeEditor))]
        public string CustomThemePath
        {
            get
            {
                return _customThemePath;
            }

            set
            {
                if (value == _customThemePath)
                {
                    return;
                }

                _customThemePath = value;
                _theme.Load(_customThemePath);
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public List<Form> Forms
        {
            get
            {
                return _formCollection;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public List<Control> SupportedControls
        {
            get
            {
                var _controlsList = new List<Control>();

                foreach (Form _forms in _formCollection)
                {
                    _controlsList.AddRange(_forms.Controls.Cast<Control>().Where(_control => ControlManager.HasMethod(_control, "UpdateTheme")));
                }

                return _controlsList;
            }
        }

        [TypeConverter(typeof(ThemeTypeConverter))]
        public Theme Theme
        {
            get
            {
                return _theme;
            }

            set
            {
                if (value == _theme)
                {
                    return;
                }

                _theme = value;
                _theme = new Theme(_themeType);
                Update();
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Themes ThemeType
        {
            get
            {
                return _themeType;
            }

            set
            {
                if (value == _themeType)
                {
                    return;
                }

                _themeType = value;
                _theme = new Theme(_themeType);
                Update();
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public List<Control> UnsupportedControls
        {
            get
            {
                var _controlsList = new List<Control>();

                foreach (Form _forms in _formCollection)
                {
                    _controlsList.AddRange(_forms.Controls.Cast<Control>().Where(_control => !ControlManager.HasMethod(_control, "UpdateTheme")));
                }

                return _controlsList;
            }
        }

        #endregion

        #region Events

        /// <summary>Opens the templates directory in the windows explorer.</summary>
        public static void OpenTemplatesDirectory()
        {
            Process.Start(Settings.TemplatesFolder);
        }

        /// <summary>Adds a form to the collection to manage.</summary>
        /// <param name="form">The form.</param>
        public void AddFormToManage(Form form)
        {
            if (!_formCollection.Contains(form))
            {
                _formCollection.Add(form);
            }

            Update();
        }

        /// <summary>Creates a copy of the current object.</summary>
        /// <returns>The <see cref="object" />.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>Open the ThemesEditor dialog to pick a custom theme file.</summary>
        public void OpenCustomTheme()
        {
            using (OpenFileDialog _openFileDialog = new OpenFileDialog())
            {
                _openFileDialog.Filter = @"Theme File|*.xml";

                if (_openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _customThemePath = _openFileDialog.FileName;
                ReadTheme();
            }
        }

        /// <summary>Reads the theme from the custom file path.</summary>
        public void ReadTheme()
        {
            _theme = new Theme(_customThemePath);
            OnThemeChanged(new ThemeEventArgs(_theme));
        }

        /// <summary>Saves the theme to a file path.</summary>
        /// <param name="filePath">The file path.</param>
        public void SaveTheme(string filePath)
        {
            _theme.Save(filePath);
        }

        public override string ToString()
        {
            StringBuilder _stringBuilder = new StringBuilder();
            _stringBuilder.AppendLine("Theme name: " + _theme.InformationSettings.Name);
            _stringBuilder.AppendLine("Theme author: " + _theme.InformationSettings.Author);
            _stringBuilder.Append(Environment.NewLine);
            _stringBuilder.AppendLine("Total forms: " + Forms.Count);
            _stringBuilder.AppendLine("Total controls: " + Controls.Count);
            _stringBuilder.AppendLine("Supported controls: " + SupportedControls.Count);
            _stringBuilder.AppendLine("Unsupported controls: " + UnsupportedControls.Count);
            return _stringBuilder.ToString();
        }

        /// <summary>Updates all the <see cref="Control" />/s in the <see cref="Form" />.</summary>
        public void Update()
        {
            if (_formCollection.Count == 0)
            {
                return;
            }

            foreach (Form _form in _formCollection)
            {
                if (_form == null)
                {
                    throw new ArgumentNullException(nameof(_form));
                }

                if (_form.Controls.Count == 0)
                {
                    return;
                }

                foreach (Control _control in _form.Controls)
                {
                    if (ControlManager.HasMethod(_control, "UpdateTheme"))
                    {
                        UpdateControl(_control, _theme);
                    }
                }
            }

            OnThemeChanged(new ThemeEventArgs(_theme));
        }

        /// <summary>The theme changed event.</summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnThemeChanged(ThemeEventArgs e)
        {
            ThemeChanged?.Invoke(e);
        }

        /// <summary>Creates a default theme file in the templates folder.</summary>
        private void ConstructDefaultThemeFile()
        {
            string _defaultThemePath = Settings.TemplatesFolder + @"DefaultTheme.xml";

            if (File.Exists(_defaultThemePath))
            {
                return;
            }

            Theme _defaultTheme = new Theme(Themes.Visual);
            string _text = _defaultTheme.RawTheme;

            if (!Directory.Exists(Settings.TemplatesFolder))
            {
                Directory.CreateDirectory(Settings.TemplatesFolder);
            }

            File.WriteAllText(_defaultThemePath, _text);
        }

        /// <summary>Updates the supported controls style.</summary>
        /// <param name="control">The control.</param>
        /// <param name="theme">The theme.</param>
        private void UpdateControl(Control control, Theme theme)
        {
            (control as VisualButton)?.UpdateTheme(theme);

            (control as VisualListBox)?.UpdateTheme(theme);

            (control as VisualGroupBox)?.UpdateTheme(theme);

            (control as VisualRichTextBox)?.UpdateTheme(theme);

            (control as VisualListView)?.UpdateTheme(theme);

            (control as VisualRadialProgress)?.UpdateTheme(theme);

            (control as VisualGauge)?.UpdateTheme(theme);

            (control as VisualProgressBar)?.UpdateTheme(theme);

            (control as VisualTextBox)?.UpdateTheme(theme);

            (control as VisualCheckBox)?.UpdateTheme(theme);

            (control as VisualCheckedListBox)?.UpdateTheme(theme);

            (control as VisualComboBox)?.UpdateTheme(theme);

            (control as VisualLabel)?.UpdateTheme(theme);

            (control as VisualNumericUpDown)?.UpdateTheme(theme);

            (control as VisualRadioButton)?.UpdateTheme(theme);

            // (control as VisualProgressStepper)?.UpdateTheme(theme);
            (control as VisualToggle)?.UpdateTheme(theme);

            (control as VisualTrackBar)?.UpdateTheme(theme);

            (control as VisualPanel)?.UpdateTheme(theme);

            (control as VisualSeparator)?.UpdateTheme(theme);
        }

        #endregion
    }
}