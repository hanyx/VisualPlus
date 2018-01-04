namespace VisualPlus.Structure
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Xml.Linq;

    using VisualPlus.Enumerators;
    using VisualPlus.Extensibility;
    using VisualPlus.Managers;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.TypeConverters;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [Description("The theme structure.")]
    [DesignerCategory("code")]
    [ToolboxItem(false)]
    [TypeConverter(typeof(ThemeTypeConverter))]
    public class Theme
    {
        #region Variables

        private BackgroundSettings _backgroundSettings;
        private BorderSettings _borderSettings;
        private ControlColorStateSettings _controlColorStateSettings;
        private ThemeInformation _informationSettings;
        private ListItemSettings _listItemSettings;
        private OtherSettings _otherSettings;
        private string _rawTheme;
        private TextSettings _textSettings;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
        /// <param name="theme">The theme.</param>
        public Theme(Theme theme) : this()
        {
            _backgroundSettings = theme.BackgroundSettings;
            _borderSettings = theme.BorderSettings;
            _informationSettings = theme.InformationSettings;
            _textSettings = theme.TextSetting;
            _controlColorStateSettings = theme.ColorStateSettings;
            _otherSettings = theme.OtherSettings;
            _listItemSettings = theme.ListItemSettings;
        }

        /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
        /// <param name="theme">The theme.</param>
        public Theme(Themes theme) : this()
        {
            LoadThemeFromResources(theme);
        }

        /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
        /// <param name="filePath">The file.</param>
        public Theme(string filePath) : this()
        {
            Load(filePath);
        }

        /// <summary>Initializes a new instance of the <see cref="Theme" /> class.</summary>
        public Theme()
        {
            _rawTheme = string.Empty;
            _backgroundSettings = new BackgroundSettings();
            _borderSettings = new BorderSettings();
            _controlColorStateSettings = new ControlColorStateSettings();
            _textSettings = new TextSettings();
            _informationSettings = new ThemeInformation();
            _listItemSettings = new ListItemSettings();
            _otherSettings = new OtherSettings();
            LoadThemeFromResources(Themes.Visual);
        }

        #endregion

        #region Properties

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        public BackgroundSettings BackgroundSettings
        {
            get
            {
                return _backgroundSettings;
            }

            set
            {
                _backgroundSettings = value;
            }
        }

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        public BorderSettings BorderSettings
        {
            get
            {
                return _borderSettings;
            }

            set
            {
                _borderSettings = value;
            }
        }

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        public ControlColorStateSettings ColorStateSettings
        {
            get
            {
                return _controlColorStateSettings;
            }

            set
            {
                _controlColorStateSettings = value;
            }
        }

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        public ThemeInformation InformationSettings
        {
            get
            {
                return _informationSettings;
            }

            set
            {
                _informationSettings = value;
            }
        }

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        public ListItemSettings ListItemSettings
        {
            get
            {
                return _listItemSettings;
            }

            set
            {
                _listItemSettings = value;
            }
        }

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        public OtherSettings OtherSettings
        {
            get
            {
                return _otherSettings;
            }

            set
            {
                _otherSettings = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string RawTheme
        {
            get
            {
                return _rawTheme;
            }

            set
            {
                _rawTheme = value;
            }
        }

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        public TextSettings TextSetting
        {
            get
            {
                return _textSettings;
            }

            set
            {
                _textSettings = value;
            }
        }

        #endregion

        #region Events

        /// <summary>Loads the <see cref="Theme" /> from the file path.</summary>
        /// <param name="filePath">The file path.</param>
        public void Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new NoNullAllowedException(ExceptionMessenger.IsNullOrEmpty(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(ExceptionMessenger.FileNotFound(filePath));
            }

            try
            {
                if (File.Exists(filePath))
                {
                    XDocument _themeDocument = XDocument.Load(filePath);
                    _rawTheme = File.ReadAllText(filePath);
                    Deserialize(_themeDocument);
                }
                else
                {
                    VisualExceptionDialog.Show(new FileNotFoundException(ExceptionMessenger.FileNotFound(filePath)));
                }
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }
        }

        /// <summary>Saves the theme to a file.</summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            XDocument _theme = XDocument.Parse(_rawTheme);
            _theme.Save(filePath);
        }

        public override string ToString()
        {
            return _rawTheme;
        }

        /// <summary>Resolves to default family if font can't be resolved.</summary>
        /// <param name="fontName">The font name.</param>
        /// <param name="defaultFontName">The default font name.</param>
        /// <returns>The <see cref="Font" />.</returns>
        private static Font ResolveFontFamily(string fontName, string defaultFontName = "Arial")
        {
            return FontManager.FontInstalled(fontName) ? new Font(new FontFamily(fontName), 8.25F) : new Font(new FontFamily(defaultFontName), 8.25F);
        }

        /// <summary>Deserialize the theme file.</summary>
        /// <param name="themeContainer">The theme container.</param>
        private void Deserialize(XContainer themeContainer)
        {
            const string Header = @"VisualPlus-Theme/";
            const string Information = Header + @"Information/";
            const string StyleTable = Header + @"StyleTable/";
            const string Shared = StyleTable + @"Shared/";
            const string Toolkit = StyleTable + @"Toolkit/";

            try
            {
                _informationSettings.Name = themeContainer.GetValue(Information + "Name");
                _informationSettings.Author = themeContainer.GetValue(Information + "Author");

                _borderSettings.Normal = themeContainer.GetValue(Shared + "Border/Normal").ToColor();
                _borderSettings.Hover = themeContainer.GetValue(Shared + "Border/Hover").ToColor();

                _textSettings.Enabled = themeContainer.GetValue(Shared + "Font/Enabled").ToColor();
                _textSettings.Disabled = themeContainer.GetValue(Shared + "Font/Disabled").ToColor();
                _textSettings.Selected = themeContainer.GetValue(Shared + "Font/Selected").ToColor();
                _textSettings.Font = ResolveFontFamily(themeContainer.GetValue(Shared + "Font/FontFamily"));

                _controlColorStateSettings.Enabled = themeContainer.GetValue(Toolkit + "VisualButton/Enabled").ToColor();
                _controlColorStateSettings.Disabled = themeContainer.GetValue(Toolkit + "VisualButton/Disabled").ToColor();
                _controlColorStateSettings.Hover = themeContainer.GetValue(Toolkit + "VisualButton/Hover").ToColor();
                _controlColorStateSettings.Pressed = themeContainer.GetValue(Toolkit + "VisualButton/Pressed").ToColor();

                _otherSettings.FormBackground = themeContainer.GetValue(Toolkit + "VisualForm/Background").ToColor();
                _otherSettings.FormWindowBar = themeContainer.GetValue(Toolkit + "VisualForm/WindowBar").ToColor();

                _listItemSettings.Item = themeContainer.GetValue(Shared + "ListItem/Normal").ToColor();
                _listItemSettings.ItemHover = themeContainer.GetValue(Shared + "ListItem/Hover").ToColor();
                _listItemSettings.ItemSelected = themeContainer.GetValue(Shared + "ListItem/Selected").ToColor();
                _listItemSettings.ItemAlternate = themeContainer.GetValue(Shared + "ListItem/Alternate").ToColor();

                _backgroundSettings.Type1 = themeContainer.GetValue(Shared + "Background/Type1").ToColor();
                _backgroundSettings.Type2 = themeContainer.GetValue(Shared + "Background/Type2").ToColor();
                _backgroundSettings.Type3 = themeContainer.GetValue(Shared + "Background/Type3").ToColor();
                _backgroundSettings.Type4 = themeContainer.GetValue(Shared + "Background/Type4").ToColor();

                _otherSettings.Line = themeContainer.GetValue(Shared + "Line").ToColor();
                _otherSettings.Shadow = themeContainer.GetValue(Shared + "Shadow").ToColor();
                _otherSettings.LightText = themeContainer.GetValue(Shared + "LightText").ToColor();

                _otherSettings.ColumnHeader = themeContainer.GetValue(Shared + "ColumnHeader/Header").ToColor();
                _otherSettings.ColumnText = themeContainer.GetValue(Shared + "ColumnHeader/Text").ToColor();

                _otherSettings.ControlEnabled = themeContainer.GetValue(Shared + "Control/Enabled").ToColor();
                _otherSettings.ControlDisabled = themeContainer.GetValue(Shared + "Control/Disabled").ToColor();

                _otherSettings.BackCircle = themeContainer.GetValue(Toolkit + "VisualRadialProgress/BackCircle").ToColor();
                _otherSettings.ForeCircle = themeContainer.GetValue(Toolkit + "VisualRadialProgress/ForeCircle").ToColor();

                _otherSettings.ProgressBackground = themeContainer.GetValue(Shared + "ProgressBar/Background").ToColor();
                _otherSettings.Progress = themeContainer.GetValue(Shared + "ProgressBar/Working").ToColor();
                _otherSettings.ProgressDisabled = themeContainer.GetValue(Shared + "ProgressBar/Disabled").ToColor();

                _otherSettings.HatchBackColor = themeContainer.GetValue(Shared + "Hatch/BackColor").ToColor();
                _otherSettings.HatchForeColor = themeContainer.GetValue(Shared + "Hatch/ForeColor").ToColor();

                _otherSettings.FlatControlDisabled = themeContainer.GetValue(Shared + "FlatControl/Enabled").ToColor();
                _otherSettings.FlatControlEnabled = themeContainer.GetValue(Shared + "FlatControl/Enabled").ToColor();

                _otherSettings.BoxDisabled = themeContainer.GetValue(Shared + "Box/Disabled").ToColor();
                _otherSettings.BoxEnabled = themeContainer.GetValue(Shared + "Box/Enabled").ToColor();

                _otherSettings.TabPageEnabled = themeContainer.GetValue(Shared + "TabPage/Enabled").ToColor();
                _otherSettings.TabPageDisabled = themeContainer.GetValue(Shared + "TabPage/Disabled").ToColor();
                _otherSettings.TabPageHover = themeContainer.GetValue(Shared + "TabPage/Hover").ToColor();
                _otherSettings.TabPageSelected = themeContainer.GetValue(Shared + "TabPage/Selected").ToColor();
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }
        }

        /// <summary>Loads a <see cref="Theme" /> from resources.</summary>
        /// <param name="theme">The theme.</param>
        private void LoadThemeFromResources(Themes theme)
        {
            try
            {
                _rawTheme = ResourceManager.ReadResource(Assembly.GetExecutingAssembly().Location, $"VisualPlus.Resources.Themes.{theme.ToString()}.xml");
                XDocument _resourceDocumentTheme = XDocument.Parse(_rawTheme);
                Deserialize(_resourceDocumentTheme);
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }
        }

        #endregion
    }
}