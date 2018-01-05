namespace VisualPlus.Toolkit.Controls.Interactivity
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using VisualPlus.Delegates;
    using VisualPlus.Designer;
    using VisualPlus.EventArgs;
    using VisualPlus.Localization.Descriptions;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.Dialogs;
    using VisualPlus.Toolkit.VisualBase;
    using VisualPlus.TypeConverters;

    #endregion

    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("MaximizeVisible")]
    [Description("The Visual ControlBox")]
    [Designer(typeof(VisualControlBoxDesigner))]
    [ToolboxBitmap(typeof(VisualControlBox), "Resources.ToolboxBitmaps.VisualControlBox.bmp")]
    [ToolboxItem(true)]
    public class VisualControlBox : VisualStyleBase, IThemeSupport
    {
        #region Variables

        private Size _buttonSize;
        private ControlBoxButton _closeButton;
        private ControlBoxButton _helpButton;
        private bool _initialized;
        private ControlBoxButton _maximizeButton;
        private ControlBoxButton _minimizeButton;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualControlBox" /> class.</summary>
        public VisualControlBox()
        {
            SetStyle(
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            InitializeControlBox();

            UpdateTheme(ThemeManager.Theme);
        }

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event ControlBoxEventHandler CloseClick;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event ControlBoxEventHandler HelpClick;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event ControlBoxEventHandler MaximizeClick;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event ControlBoxEventHandler MinimizeClick;

        [Category(Localization.Category.Events.PropertyChanged)]
        [Description(Event.PropertyEventChanged)]
        public event ControlBoxEventHandler RestoredFormWindow;

        #endregion

        #region Properties

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ControlBoxButton CloseButton
        {
            get
            {
                return _closeButton;
            }

            set
            {
                _closeButton = value;
            }
        }

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlBoxButton HelpButton
        {
            get
            {
                return _helpButton;
            }

            set
            {
                _helpButton = value;
            }
        }

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlBoxButton MaximizeButton
        {
            get
            {
                return _maximizeButton;
            }

            set
            {
                _maximizeButton = value;
            }
        }

        [TypeConverter(typeof(BasicSettingsTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlBoxButton MinimizeButton
        {
            get
            {
                return _minimizeButton;
            }

            set
            {
                _minimizeButton = value;
            }
        }

        /// <summary>Gets the parent form.</summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Form ParentForm
        {
            get
            {
                return Parent.FindForm();
            }
        }

        #endregion

        #region Events

        public void UpdateTheme(Theme theme)
        {
            try
            {
                _closeButton.BackColorState = new ControlColorState
                    {
                        Disabled = theme.OtherSettings.CloseButtonBack.Disabled,
                        Enabled = theme.OtherSettings.CloseButtonBack.Enabled,
                        Hover = theme.OtherSettings.CloseButtonBack.Hover,
                        Pressed = theme.OtherSettings.CloseButtonBack.Pressed
                    };

                _closeButton.ForeColorState = new ControlColorState
                    {
                        Disabled = theme.OtherSettings.CloseButtonFore.Disabled,
                        Enabled = theme.OtherSettings.CloseButtonFore.Enabled,
                        Hover = theme.OtherSettings.CloseButtonFore.Hover,
                        Pressed = theme.OtherSettings.CloseButtonFore.Pressed
                    };

                _maximizeButton.BackColorState = new ControlColorState
                    {
                        Disabled = theme.OtherSettings.MaximizeButtonBack.Disabled,
                        Enabled = theme.OtherSettings.MaximizeButtonBack.Enabled,
                        Hover = theme.OtherSettings.MaximizeButtonBack.Hover,
                        Pressed = theme.OtherSettings.MaximizeButtonBack.Pressed
                    };

                _maximizeButton.ForeColorState = new ControlColorState
                    {
                        Disabled = theme.OtherSettings.MaximizeButtonFore.Disabled,
                        Enabled = theme.OtherSettings.MaximizeButtonFore.Enabled,
                        Hover = theme.OtherSettings.MaximizeButtonFore.Hover,
                        Pressed = theme.OtherSettings.MaximizeButtonFore.Pressed
                    };

                _minimizeButton.BackColorState = new ControlColorState
                    {
                        Disabled = theme.OtherSettings.MinimizeButtonBack.Disabled,
                        Enabled = theme.OtherSettings.MinimizeButtonBack.Enabled,
                        Hover = theme.OtherSettings.MinimizeButtonBack.Hover,
                        Pressed = theme.OtherSettings.MinimizeButtonBack.Pressed
                    };

                _minimizeButton.ForeColorState = new ControlColorState
                    {
                        Disabled = theme.OtherSettings.MinimizeButtonFore.Disabled,
                        Enabled = theme.OtherSettings.MinimizeButtonFore.Enabled,
                        Hover = theme.OtherSettings.MinimizeButtonFore.Hover,
                        Pressed = theme.OtherSettings.MinimizeButtonFore.Pressed
                    };

                _helpButton.BackColorState = new ControlColorState
                    {
                        Disabled = theme.OtherSettings.HelpButtonBack.Disabled,
                        Enabled = theme.OtherSettings.HelpButtonBack.Enabled,
                        Hover = theme.OtherSettings.HelpButtonBack.Hover,
                        Pressed = theme.OtherSettings.HelpButtonBack.Pressed
                    };

                _helpButton.ForeColorState = new ControlColorState
                    {
                        Disabled = theme.OtherSettings.HelpButtonFore.Disabled,
                        Enabled = theme.OtherSettings.HelpButtonFore.Enabled,
                        Hover = theme.OtherSettings.HelpButtonFore.Hover,
                        Pressed = theme.OtherSettings.HelpButtonFore.Pressed
                    };
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
            }

            Invalidate();
            OnThemeChanged(new ThemeEventArgs(theme));
        }

        /// <summary>The OnCloseClick.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnCloseClick(object sender, EventArgs e)
        {
            CloseClick?.Invoke(new ControlBoxEventArgs(ParentForm));
            ParentForm.Close();
        }

        /// <summary>The OnCloseClick.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnHelpClick(object sender, EventArgs e)
        {
            HelpClick?.Invoke(new ControlBoxEventArgs(ParentForm));
        }

        /// <summary>The OnMaximizeClick.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnMaximizeClick(object sender, EventArgs e)
        {
            if (ParentForm.WindowState == FormWindowState.Normal)
            {
                if (_maximizeButton.BoxType == ControlBoxButton.ControlBoxType.Default)
                {
                    _maximizeButton.Text = @"2";
                }

                ParentForm.WindowState = FormWindowState.Maximized;
                MaximizeClick?.Invoke(new ControlBoxEventArgs(ParentForm));
            }
            else
            {
                if (_maximizeButton.BoxType == ControlBoxButton.ControlBoxType.Default)
                {
                    _maximizeButton.Text = @"1";
                }

                ParentForm.WindowState = FormWindowState.Normal;
                RestoredFormWindow?.Invoke(new ControlBoxEventArgs(ParentForm));
            }
        }

        /// <summary>The OnMinimizeClick.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnMinimizeClick(object sender, EventArgs e)
        {
            ParentForm.WindowState = FormWindowState.Minimized;
            MinimizeClick?.Invoke(new ControlBoxEventArgs(ParentForm));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (_initialized)
            {
                Size = GetAdjustedSize();
            }
            else
            {
                Size = new Size(100, 25);
            }
        }

        private void Button_VisibleChanged(object sender, EventArgs e)
        {
            if (_helpButton.Visible)
            {
                _minimizeButton.Location = new Point(_buttonSize.Width, 0);
            }
            else
            {
                _minimizeButton.Location = new Point(0, 0);
            }

            if (_minimizeButton.Visible)
            {
                _maximizeButton.Location = new Point(_minimizeButton.Right, 0);
            }
            else
            {
                _maximizeButton.Location = new Point(_minimizeButton.Right, 0);
            }

            if (_maximizeButton.Visible)
            {
                _closeButton.Location = new Point(_maximizeButton.Right, 0);
            }
            else
            {
                _closeButton.Location = new Point(_minimizeButton.Right, 0);
            }

            OnResize(new EventArgs());
        }

        /// <summary>Retrieves the adjusted <see cref="Control" />-<see cref="Size" />.</summary>
        /// <param name="height">The height.</param>
        /// <returns>The <see cref="Size" />.</returns>
        private Size GetAdjustedSize(int height = 25)
        {
            try
            {
                var _x = 0;

                if (_helpButton.Visible)
                {
                    _x += _helpButton.Width;
                }

                if (_minimizeButton.Visible)
                {
                    _x += _minimizeButton.Width;
                }

                if (_maximizeButton.Visible)
                {
                    _x += _maximizeButton.Width;
                }

                if (_closeButton.Visible)
                {
                    _x += _closeButton.Width;
                }

                return new Size(_x, height);
            }
            catch (Exception e)
            {
                VisualExceptionDialog.Show(e);
                return new Size(25, 25);
            }
        }

        /// <summary>Automatically places the <see cref="VisualControlBox"/> on the <see cref="Form"/> corner location.</summary>
        public void AutoPlaceOnForm()
        {
            Location = new Point(ParentForm.Width - Width, 0);
        }

        /// <summary>Initializes the <see cref="VisualControlBox" />.</summary>
        private void InitializeControlBox()
        {
            DoubleBuffered = true;
            UpdateStyles();

            Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackColor = Color.Transparent;

            Size = new Size(100, 25);

            Padding = new Padding(0);

            _buttonSize = new Size(24, Height);

            _helpButton = new ControlBoxButton
                {
                    Location = new Point(0, 0),
                    Size = _buttonSize,
                    Text = @"s",
                    OffsetLocation = new Point(0, 1),
                    Visible = false
                };

            _helpButton.Click += OnHelpClick;
            _helpButton.VisibleChanged += Button_VisibleChanged;

            _minimizeButton = new ControlBoxButton
                {
                    Location = new Point(_buttonSize.Width, 0),
                    Size = _buttonSize,
                    Text = @"0",
                    OffsetLocation = new Point(2, 0)
                };

            _minimizeButton.Click += OnMinimizeClick;
            _minimizeButton.VisibleChanged += Button_VisibleChanged;

            _maximizeButton = new ControlBoxButton
                {
                    Location = new Point(_buttonSize.Width * 2, 0),
                    Size = _buttonSize,
                    Text = @"1",
                    OffsetLocation = new Point(1, 1)
                };

            _maximizeButton.Click += OnMaximizeClick;
            _maximizeButton.VisibleChanged += Button_VisibleChanged;

            _closeButton = new ControlBoxButton
                {
                    Location = new Point(_buttonSize.Width * 3, 0),
                    Size = _buttonSize,
                    Text = @"r",
                    OffsetLocation = new Point(1, 2)
                };

            _closeButton.Click += OnCloseClick;
            _closeButton.VisibleChanged += Button_VisibleChanged;

            _initialized = true;

            Controls.Add(_helpButton);
            Controls.Add(_minimizeButton);
            Controls.Add(_maximizeButton);
            Controls.Add(_closeButton);
        }

        #endregion
    }
}